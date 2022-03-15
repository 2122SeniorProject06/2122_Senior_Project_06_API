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
        /// Adds a new account to the database.
        /// </summary>
        /// <param name="newAccount">The new account.</param>
        public static void AddNewAccount(UserAccount newAccount)
        {
            DatabaseAccess.AddEntryToTable(tableName, newAccount.ToSqlString());
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
                    if(accountInfo.ItemArray.Length == 4)
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
                Username = (string) results[1],
                Password = (string) results[2],
                Email = (string) results[3]
            };
            return account;
        }
    }
}