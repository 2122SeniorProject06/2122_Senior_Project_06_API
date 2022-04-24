using System;
using System.Data;
using System.Collections.Generic;
namespace  _2122_Senior_Project_06.Types
{
    /// <summary>
    /// The class that manages the login page.
    /// </summary>
    /// <remarks> Paired programmed by Hugo, Andrew, and Sarah. </remarks>
    public class JournalEntry 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
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
        public JournalEntry(string title, string body, string userID, DateTime lastUpdated,
                            bool hadAttack, string activity, bool wasEffective)
        {
            JournalID = Sys_Security.GenID(JournalID, false);
            Title = title;
            Body = body;
            UserID = userID;
            LastUpdated = lastUpdated;
            ActivityMetric = new Metrics(hadAttack, activity, wasEffective);
        }

        /// <summary>
        /// Parameterized constructor for JournalEntry that was just created.
        /// </summary>
        /// <param name="title">The title of the entry.</param>
        /// <param name="body">The body of the entry.</param>
        /// <param name="userID">The userID of the entry creator.</param>
        /// <param name="lastUpdated">The time the journal was last update.
        /// Reccomended to send DateTime.Now.</param>
        /// <param name="activityMetrics">The user's metrics.</param>
        public JournalEntry(string title, string body, string userID, DateTime lastUpdated, Metrics activityMetrics)
        {
            JournalID = Sys_Security.GenID(JournalID, false);
            Title = title;
            Body = body;
            UserID = userID;
            LastUpdated = lastUpdated;
            ActivityMetric = activityMetrics;
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
            ActivityMetric = new Metrics((bool)result[5], (string)result[6], (bool)result[7]);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets and sets the Journal ID.
        /// </summary>
        public string JournalID {get; set;}

        /// <summary>
        /// Gets and sets the journal's title.
        /// </summary>
        public string Title {get; set;}

        /// <summary>
        /// Gets and sets the journal's body.
        /// </summary>
        public string Body {get; set;}

        /// <summary>
        /// Gets and sets the journal creator's ID.
        /// </summary>
        public string UserID {get; set;}

        /// <summary>
        /// Gets and sets the last time the journal was updated.
        /// </summary>
        public DateTime LastUpdated {get; set;}

        /// <summary>
        /// Gets and sets the journal's activity metrics.
        /// </summary>
        /// <value></value>
        public Metrics ActivityMetric {get; set;}
        #endregion

        #region Methods
        /// <summary>
        /// Returns values in a SQL value format.
        /// </summary>
        /// <param name="isUpdate">Whether the command is being called from an Update function.</param>
        /// <returns>All values formatted into SQL.</returns>
        public string ToSqlString(bool isUpdate)
        {
            string formattedLastUpdated = LastUpdated.ToString("yyyy-MM-dd HH:MM:ss");
            string[] values = { JournalID,
                                Sys_Security.Encoder(Title),
                                Sys_Security.Encoder(Body),
                                UserID,
                                formattedLastUpdated };

            if(isUpdate)
            {
                values[0] =  string.Format("{0} = '{1}'",JournalsItems.JournalID, values[0]);
                values[1] =  string.Format("{0} = '{1}'",JournalsItems.Title, values[1]);
                values[2] =  string.Format("{0} = '{1}'",JournalsItems.Body, values[2]);
                values[3] =  string.Format("{0} = '{1}'",JournalsItems.UserID, values[3]);
                values[4] =  string.Format("{0} = '{1}'",JournalsItems.LastUpdated, values[4]);
            }
            else
            {
                for(int i = 0; i < values.Length; i++)
                {
                    values[i] = string.Format("'{0}'", values[i]);
                }
            }
            
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5}",
                                 values[0],
                                 values[1],
                                 values[2],
                                 values[3],
                                 values[4],
                                 ActivityMetric.ToSqlString(isUpdate));
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
        #endregion
    }
}