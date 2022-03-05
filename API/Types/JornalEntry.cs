using System;
using System.Data;
namespace  _2122_Senior_Project_06.Types
{
    /// <summary>
    /// The class that manages the login page.
    /// </summary>
    /// <remarks> Made by Hugo Mazariego. Last update 12/09/2021. </remarks>
    public class JournalEntry 
    {
        public string JournalID {get; private set;}
        public string Title {get; set;}
        public string Body {get; set;}
        public string UserID {get; private set;}
        public DateTime LastUpdated {get; set;}

        public JournalEntry ()
        {
            
        }


        /// <summary>
        /// Parameterized constructor for JournalEntry that was just created.
        /// </summary>
        /// <param name="title">The title of the entry.</param>
        /// <param name="body">The body of the entry.</param>
        /// <param name="userID">The userID of the entry creator.</param>
        /// <param name="lastUpdated">The time the journal was last update.
        /// Reccomended to send DateTime.Now.</param>
        public JournalEntry(string title, string body, string userID, DateTime lastUpdated)
        {
            JournalID = Sys_Security.GenID(JournalID, false);
            Title = title;
            Body = body;
            UserID = userID;
            LastUpdated = lastUpdated;
        }

        /// <summary>
        /// Parameterized constructor for JournalEntry being read from the database.
        /// </summary>
        /// <param name="result">The data from the database.</param>
        public JournalEntry(DataRow result)
        {
            JournalID = (string)result[0];
            Title = Sys_Security.Decoder((string)result[1]);
            Body = Sys_Security.Decoder((string)result[2]);
            UserID = (string)result[3];
            LastUpdated = (DateTime) result[4];
        }

        /// <summary>
        /// Returns values in a SQL value format.
        /// </summary>
        /// <returns>All values formatted into SQL.</returns>
        public string ToSqlString()
        {
            string formattedLastUpdated = LastUpdated.ToString("yyyy-MM-dd HH:MM:ss");
            return string.Format("'{0}', '{1}', '{2}', '{3}', '{4}'",
                                 JournalID,
                                 Sys_Security.Encoder(Title),
                                 Sys_Security.Encoder(Body),
                                 UserID,
                                 formattedLastUpdated);
        }

        /// <summary>
        /// Updates the data of the journal entry.
        /// </summary>
        /// <param name="newInfo">The updated information.</param>
        public void UpdateInfo(JournalEntry newInfo)
        {
            Title = newInfo.Title ?? Title;
            Body = newInfo.Body ?? Body;
            LastUpdated = newInfo.LastUpdated;
        }
    }
}