using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using _2122_Senior_Project_06.Types;
namespace _2122_Senior_Project_06.SqlDatabase
{
    /// <summary>
    /// Runs database commands.
    /// </summary>
    /// <remarks> Paired programmed by Hugo and Andrew. </remarks>
    internal class DatabaseAccess
    {
        #region DatabaseConnection Class
        /// <summary>
        /// Class directly interacting with the database.
        /// </summary>
        /// <remarks>
        /// Internal does not hide the class from other namespaces.
        /// It hides the class from other assemblies i.e. other .csproj files.
        /// The only way to hide the class is to make it a nested private class.
        /// </remarks>
        /// <remarks> Paired programmed by Hugo and Andrew. </remarks>
        private class DatabaseConnection
        {
            private static string _connectionString;

            /// <summary>
            /// Sends a sql request to the database.
            /// </summary>
            /// <param name="request">The request to send.</param>
            /// <returns>The result of the request, if applicable.</returns>
            /// <remarks>Returns a DataTable with the results. </remarks>
            public static DataTable SendRequest(string request)
            {
                //Return value
                DataTable results = new DataTable();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand(request, connection);
                    command.Connection.Open();
                    using(var reader = command.ExecuteReader())
                    {
                        results.Load(reader);
                    }
                    command.Connection.Close();
                }
                return results;
            }

            /// <summary>
            /// Configures the connection string to use in the class.
            /// </summary>
            /// <param name="connectionString"></param>
            public static void ConfigureConnectionString(string connectionString)
            {
                //Decrypts encoded connection string into bytes.
                var decryptedBytes = System.Convert.FromBase64String(connectionString);
                //Converts bytes into decoded connection  string.
                _connectionString = System.Text.Encoding.UTF8.GetString(decryptedBytes);
            }
        }
        #endregion

        /// <summary>
        /// Sets up the database for connection.
        /// </summary>
        /// <param name="configuration">The configuration file.</param>
        public static void SetupDatabase(IConfiguration configuration)
        {
            var encodedConnectionString = configuration.GetConnectionString("ShardDB");
            DatabaseConnection.ConfigureConnectionString(encodedConnectionString);
            SetupAndCreateTables();
        }

        /// <summary>
        /// Ensures all necessary tables are set up in which ever database.
        /// </summary>
        private static void SetupAndCreateTables()
        {
            string userAccountValues = string.Format("{0} varchar(9), {1} varchar(256), {2} varchar(65), " +
                                                     "{3} varchar(256), {4} bit, {5} varchar(256)",
                                                     UserAccountsItems.User_ID, UserAccountsItems.Username,
                                                     UserAccountsItems.Password, UserAccountsItems.Email,
                                                     UserAccountsItems.DarkMode, UserAccountsItems.Background);
            string journalValues = string.Format("{0} varchar(9), {1} varchar(256), {2} varchar(256), {3} varchar(9), "+
                                                 "{4} DateTime, {5} bit, {6} varchar(20), {7} bit",
                                                 JournalsItems.JournalID, JournalsItems.Title, JournalsItems.Body,
                                                 JournalsItems.UserID, JournalsItems.LastUpdated, MetricsItems.HadAttack,
                                                 MetricsItems.Activity, MetricsItems.WasEffective);
            string[] tableNames = {"UserAccounts", "Journals"};
            string[] tableValues = {userAccountValues, journalValues};
            int index = 0;
            foreach(var tableName in tableNames)
            {
                CreateTable(tableName, tableValues[index]);
                index++;
            }
        }

        /// <summary>
        /// Creates a table in the database.
        /// </summary>
        /// <param name="tableName">The name of the table to create.</param>
        /// <param name="values">The values of the new tables.</param>
        public static void CreateTable(string tableName, string values)
        {
            string request = string.Format("CREATE TABLE {0} ({1});", tableName, values);
            try
            {
                DatabaseConnection.SendRequest(request);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                if(e.Message != string.Format("There is already an object named '{0}' in the database.", tableName))
                {
                    Console.WriteLine(e.GetType());
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        /// <summary>
        /// Adds an entry to a table. 
        /// </summary>
        /// <param name="tableName">The table to add to the entry.</param>
        /// <param name="values">The values of the new entry.</param>
        public static void AddEntryToTable(string tableName, string values)
        {
            string request = string.Format("INSERT INTO {0} VALUES ({1});", tableName, values);
            DatabaseConnection.SendRequest(request);
            
        }

        /// <summary>
        /// Updates an entry in a data table.
        /// </summary>
        /// <param name="tableName">The table to update.</param>
        /// <param name="values">The value(s) to update.</param>
        /// <param name="requirements">The required item(s) to update.</param>
        public static void UpdateEntryInTable(string tableName, string values, string requirements)
        {
            if(requirements == null)
            {
                throw new Exception("WHERE CLAUSE MUST BE SPECIFIED. LACK OF WHERE CLAUSE WILL UPDATE ALL ENTRIES.");
            }
            string request = string.Format("UPDATE {0} SET {1} WHERE {2};", tableName, values, requirements);
            DatabaseConnection.SendRequest(request);
        }

        /// <summary>
        /// Removes an entry from a data table.
        /// </summary>
        /// <param name="tableName">The data table to remove from.</param>
        /// <param name="requirements">The required item(s) to remove.</param>
        public static void DeleteEntryFromTable(string tableName, string requirements)
        {
            if(requirements == null)    
            {
                throw new Exception("WHERE CLAUSE MUST BE SPECIFIED. LACK OF WHERE CLAUSE WILL DELETE ALL ENTRIES.");
            }
            string request = string.Format("DELETE FROM {0} WHERE {1};", tableName, requirements);
            DatabaseConnection.SendRequest(request);
        }

        /// <summary>
        /// Selects items from a table.
        /// </summary>
        /// <param name="tableName">The name of the table to select from.</param>
        /// <param name="itemsToSelect">The specific items to select.</param>
        /// <param name="requirements">The requirements for which entries to select from.</param>
        /// <returns>A list of selected items.</returns>
        public static DataTable Select(string tableName, string itemsToSelect = null, string requirements = null)
        {
            string requestTemplate = "SELECT {0} FROM {1}{2};";
            string items = itemsToSelect ?? "*";
            string condition = string.Empty;
            string request = string.Empty;
            if (requirements != null)
            {
                condition = string.Format(" WHERE {0}", requirements);
            }
            request = string.Format(requestTemplate, items, tableName, condition);
            return DatabaseConnection.SendRequest(request);
        }

    }
}