using System.Data;
using System.Collections.Generic;
using _2122_Senior_Project_06.Models;
using _2122_Senior_Project_06.Exceptions;
using _2122_Senior_Project_06.Types;

namespace _2122_Senior_Project_06.SqlDatabase
{
    /// <summary>
    /// Class for interacting with the user account data table.
    /// </summary>
    /// <remarks>All parameters are encoded before being sent to the database.</remarks>
    public class JournalsDataTable
    {
        private static string tableName = "Journals";

        /// <summary>
        /// Gets all journals associated with an account.
        /// </summary>
        /// <param name="userID">The account the journals belongs to.</param>
        /// <returns>A list of journals belonging to the user.</returns>
        public static List<JournalEntry> GetAllJournals(string userID)
        {
            string requirements = string.Format("{0} = '{1}'", JournalsItems.UserID, userID);
            List<JournalEntry> results = DecryptRequestList(tableName, null, requirements);
            return results;
        }

        /// <summary>
        /// Gets the journal associated with the id.
        /// </summary>
        /// <param name="journalID">The journal id to find.</param>
        /// <returns>All the journal's contents.</returns>
        public static JournalEntry GetJournalEntry(string journalID)
        {
            string requirements = string.Format("{0} = '{1}'", JournalsItems.JournalID, journalID);
            List<JournalEntry> results = DecryptRequestList(tableName, null, requirements);
            return results[0];
        }

        /// <summary>
        /// Adds a new data entry to the database.
        /// </summary>
        /// <param name="entry">The journal entry to add.</param>
        public static void AddNewEntry(JournalEntry entry)
        {
            DatabaseAccess.AddEntryToTable(tableName, entry.ToSqlString(false));
        }

        /// <summary>
        /// Updates a journal entry in the database with provided information.
        /// </summary>
        /// <param name="updatedInfo">The updated journal entry values.</param>
        public static void UpdateJournalEntry(JournalEntry updatedInfo)
        {
            string requirements = string.Format("{0} = '{1}'", JournalsItems.JournalID, updatedInfo.JournalID);
            JournalEntry oldInfo = GetJournalEntry(updatedInfo.JournalID);
            oldInfo.UpdateInfo(updatedInfo);
            DatabaseAccess.UpdateEntryInTable(tableName, oldInfo.ToSqlString(true), requirements);

        }

        /// <summary>
        /// Deletes a journal entry from the table.
        /// </summary>
        /// <param name="journalID">The journal entry id to delete.</param>
        public static void DeleteEntry(string journalID)
        {
            string requirements = string.Format("{0} = '{1}'", JournalsItems.JournalID, journalID);
            DatabaseAccess.DeleteEntryFromTable(tableName, requirements);
        }

        /// <summary>
        /// Checks if UID is being used in the database.
        /// </summary>
        /// <param name="uid">The uid to look for.</param>
        /// <returns>True if uid is in the database.</returns>
        public static bool JournalIDInUse(string journalID)
        {
            string requirements = string.Format("{0} = '{1}'", JournalsItems.JournalID, journalID);
            List<JournalEntry> associatedAccounts = DecryptRequestList(tableName, null, requirements);
            return associatedAccounts.Count != 0;
        }

        public static List<JournalEntry> GetMetricsWithUserId(string userID)
        {
            string itemsToSelect = string.Format("{0}, {1}, {2}", JournalsItems.HadAttack, JournalsItems.Activity, JournalsItems.WasEffective);
            string requirements = string.Format("{0} = '{1}'", JournalsItems.UserID, userID);
            List<JournalEntry> userMetrics = DecryptRequestList(tableName, itemsToSelect, requirements);
            return userMetrics;
        }

        /// <summary>
        /// Decrypts values from List of List of strings and converts to appropriate type.
        /// </summary>
        /// <param name="tableName">The name of the table to select from.</param>
        /// <param name="itemsToSelect">The item(s) to select.</param>
        /// <param name="requirements">The required entries to select from.</param>
        /// <returns></returns>
        private static List<JournalEntry> DecryptRequestList(string tableName, string itemsToSelect = null, string requirements = null)
        {
            List<JournalEntry> desiredValues = null;

            DataTable allEntriesFound = DatabaseAccess.Select(tableName,itemsToSelect, requirements);
            if(allEntriesFound != null)
            {
                desiredValues = new List<JournalEntry>();
                foreach(DataRow entryInfo in allEntriesFound.Rows)
                {
                    JournalEntry result = new JournalEntry(entryInfo); 
                    desiredValues.Add(result);
                }
            }
            return desiredValues;
        }
    }
}