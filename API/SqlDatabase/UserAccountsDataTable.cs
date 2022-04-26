using System.Data;
using _2122_Senior_Project_06.Exceptions;
using _2122_Senior_Project_06.Types;

namespace _2122_Senior_Project_06.SqlDatabase
{
    /// <summary>
    /// Class for interacting with the user account data table.
    /// </summary>
    /// <remarks>All parameters are encoded before being sent to the database.</remarks>
    public class UserAccountsDataTable
    {
        private static string tableName = "UserAccounts";

        /// <summary>
        /// Gets the account password based on the provided email.
        /// </summary>
        /// <param name="email">The provided email.</param>
        /// <returns>The account password.</returns>
        public static string GetPassFromEmail(string email)
        {
            string encodedEmail = Sys_Security.Encoder(email);
            string requirements = string.Format("{0} = '{1}'", UserAccountsItems.Email, encodedEmail);
            string hashedPass = (string) DecryptRequestList(tableName, UserAccountsItems.Password, requirements);
            if(hashedPass == null)
            {
                throw new IssueWithCredentialException();
            }
            return hashedPass;
        }

        /// <summary>
        /// Gets the user id based on the provided email.
        /// </summary>
        /// <param name="email">THe provided email.</param>
        /// <returns>The user ID.</returns>
        public static string GetUIDFromEmail(string email)
        {
            string encodedEmail = Sys_Security.Encoder(email);
            string requirements = string.Format("{0} = '{1}'", UserAccountsItems.Email, encodedEmail);
            string userID = (string) DecryptRequestList(tableName, UserAccountsItems.User_ID, requirements);
            return userID;
        }

        /// <summary>
        /// Gets entire account information with a user ID.
        /// </summary>
        /// <param name="uid">The user ID.</param>
        /// <returns>The associated user account.</returns>
        public static UserAccount GetAccount(string uid)
        {
            string requirements = string.Format("{0} = '{1}'", UserAccountsItems.User_ID, uid);
            UserAccount account = (UserAccount) DecryptRequestList(tableName, null, requirements);
            return account;
        }

        /// <summary>
        /// Checks if email is being used in the database.
        /// </summary>
        /// <param name="email">The email to look for.</param>
        /// <returns>True if email is in the database.</returns>
        public static bool EmailInUse(string email)
        {
            string encodedEmail = Sys_Security.Encoder(email);
            string requirements = string.Format("{0} = '{1}'", UserAccountsItems.Email, encodedEmail);
            object associatedAccounts = DecryptRequestList(tableName, UserAccountsItems.Email, requirements);
            return associatedAccounts != null;
        }

        /// <summary>
        /// Checks if UID is being used in the database.
        /// </summary>
        /// <param name="uid">The uid to look for.</param>
        /// <returns>True if uid is in the database.</returns>
        public static bool UIDInUse(string uid)
        {
            string requirements = string.Format("{0} = '{1}'", UserAccountsItems.User_ID, uid);
            object associatedAccounts = DecryptRequestList(tableName, UserAccountsItems.User_ID, requirements);
            return associatedAccounts != null;
        }

        /// <summary>
        /// Deletes a user account.
        /// </summary>
        /// <param name="uid">UserID associated with the user account to delete.</param>
        public static void DeleteUser(string uid)
        {
            string requirements = string.Format("{0} = '{1}'", UserAccountsItems.User_ID, uid);
            DatabaseAccess.DeleteEntryFromTable(tableName, requirements);
        }

        /// <summary>
        /// Updates account information with provided values.
        /// </summary>
        /// <param name="updatedAccount">The updated accoutn values.</param>
        public static void UpdateUserAccount(UserAccount updatedAccount)
        {
            string requirements = string.Format("{0} = '{1}'", UserAccountsItems.User_ID, updatedAccount.UserID);
            UserAccount oldInfo = GetAccount(updatedAccount.UserID);
            oldInfo.UpdateInfo(updatedAccount);
            DatabaseAccess.UpdateEntryInTable(tableName, oldInfo.ToSqlString(true), requirements);
        }

        /// <summary>
        /// Adds a new account to the database.
        /// </summary>
        /// <param name="newAccount">The new account.</param>
        public static void AddNewAccount(UserAccount newAccount)
        {
            DatabaseAccess.AddEntryToTable(tableName, newAccount.ToSqlString(false));
        }

        /// <summary>
        /// Decrypts values from List of List of strings and converts to appropriate type.
        /// </summary>
        /// <param name="tableName">The name of the table to select from.</param>
        /// <param name="itemsToSelect">The item(s) to select.</param>
        /// <param name="requirements">The required entries to select from.</param>
        /// <returns></returns>
        private static object DecryptRequestList(string tableName, string itemsToSelect = null, string requirements = null)
        {
            object desiredValues = null;

            DataTable allAccountsFound = DatabaseAccess.Select(tableName,itemsToSelect, requirements);
            if(allAccountsFound != null)
            {
                foreach(DataRow accountInfo in allAccountsFound.Rows)
                {
                    if(accountInfo.ItemArray.Length == 6)
                        desiredValues = ResultToAccount(accountInfo);
                    else
                    {
                        desiredValues = accountInfo[0];
                        desiredValues = Sys_Security.Decoder((string) desiredValues);
                    }
                }
            }
            return desiredValues;
        }

        /// <summary>
        /// Converts a list of strings into a user account.
        /// </summary>
        /// <param name="results">The list of strings to convert.</param>
        /// <returns>A new account variable.</returns>
        private static UserAccount ResultToAccount(DataRow results) 
        {
            UserAccount account = new UserAccount()
            {
                UserID = (string) results[0],
                Username = Sys_Security.Decoder((string)results[1]),
                Password = Sys_Security.Decoder((string)results[2]),
                Email = Sys_Security.Decoder((string)results[3]),
                DarkMode = (bool) results[4],
                Background = Sys_Security.Decoder((string)results[5])
            };
            return account;
        }
    }
}