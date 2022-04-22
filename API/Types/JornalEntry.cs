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
        public string JournalID {get; set;}
        public string Title {get; set;}
        public string Body {get; set;}
        public string UserID {get; set;}
        public DateTime LastUpdated {get; set;}
        public bool HadAttack { get; set; }
        public string Activity { get; set; }
        public bool WasEffective { get; set; }

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
            HadAttack = hadAttack;
            Activity = VerifyActivity(activity);
            WasEffective = wasEffective;
        }

        /// <summary>
        /// Parameterized constructor for JournalEntry being read from the database.
        /// </summary>
        /// <param name="result">The data from the database.</param>
        public JournalEntry(DataRow result)
        {
            if(result.ItemArray.Length == 8)
            {
                JournalID = (string)result[0];
                Title = Sys_Security.Decoder((string)result[1]);
                Body = Sys_Security.Decoder((string)result[2]);
                UserID = (string)result[3];
                LastUpdated = (DateTime) result[4];
                HadAttack = (bool)result[5];
                Activity = VerifyActivity((string)result[6]);
                WasEffective = (bool)result[7];
            }
            else if(result.ItemArray.Length == 3)
            {
                HadAttack = (bool)result[0];
                Activity = VerifyActivity((string)result[1]);
                WasEffective = (bool)result[2];
            }
        }

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
                                 formattedLastUpdated,
                                 (HadAttack ? 1 : 0).ToString(),
                                 Activity,
                                 (WasEffective ? 1 : 0).ToString() };

            if(isUpdate)
            {
                values[0] =  string.Format("{0} = '{1}'",JournalsItems.JournalID, values[0]);
                values[1] =  string.Format("{0} = '{1}'",JournalsItems.Title, values[1]);
                values[2] =  string.Format("{0} = '{1}'",JournalsItems.Body, values[2]);
                values[3] =  string.Format("{0} = '{1}'",JournalsItems.UserID, values[3]);
                values[4] =  string.Format("{0} = '{1}'",JournalsItems.LastUpdated, values[4]);
                values[5] =  string.Format("{0} = {1}",JournalsItems.HadAttack, values[5]);
                values[6] =  string.Format("{0} = '{1}'",JournalsItems.Activity, values[6]);
                values[7] =  string.Format("{0} = {1}",JournalsItems.WasEffective, values[7]);
            }
            else
            {
                for(int i = 0; i < values.Length; i++)
                {
                    values[i] = i != 5 && i != 7 ? string.Format("'{0}'", values[i]) : string.Format("{0}", values[i]);
                }
            }
            
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}",
                                 values[0],
                                 values[1],
                                 values[2],
                                 values[3],
                                 values[4],
                                 values[5],
                                 values[6],
                                 values[7]);
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

        /// <summary>
        /// Checks if passed activity is on the approved list.
        /// </summary>
        /// <param name="activity">The activity name passed.</param>
        /// <returns>The activity that was used. Returns Other if activity doesn't belong on the list.</returns>
        private string VerifyActivity(string activity)
        {
            List<string> ActivityList = new List<string>()
            {
                ActivityItems.Breathe,
                ActivityItems.CheckIn,
                ActivityItems.Encourage,
                ActivityItems.Focus,
                ActivityItems.Ground,
                ActivityItems.Other,
                ActivityItems.Relax
            };
            return ActivityList.Contains(activity) ? activity : ActivityItems.Other;
        }
    }
}