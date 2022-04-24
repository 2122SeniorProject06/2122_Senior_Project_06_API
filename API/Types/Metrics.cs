using System.Collections.Generic;
using System.Data;
namespace _2122_Senior_Project_06.Types
{
    public class Metrics
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Metrics(){ }
        
        /// <summary>
        /// The parameterized constructor.
        /// </summary>
        /// <param name="hadAttack">If the user had an attack.</param>
        /// <param name="activity">The activity used.</param>
        /// <param name="wasEffective">If the activity was effective.</param>
        public Metrics(bool hadAttack, string activity, bool wasEffective)
        {
            HadAttack = hadAttack;
            Activity = activity;
            WasEffective = wasEffective;
        }

        public Metrics(DataRow result)
        {
            HadAttack = (bool)result[0];
            Activity = (string)result[1];
            WasEffective = (bool)result[2];
        }

        #region Properties
        /// <summary>
        /// Gets and sets if the user had an attack.
        /// </summary>
        public bool HadAttack { get; set; }

        /// <summary>
        /// Gets and sets the activity used.
        /// </summary>
        /// <remarks>Any assignment will be compared to the list of current activites.</remarks>
        public string Activity
        { 
            get => _activity;
             set
             {
                _activity = VerifyActivity(value);
             } 
        }
        private string _activity;

        /// <summary>
        /// Gets and sets if the activity was effective.
        /// </summary>
        public bool WasEffective { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns values in a SQL value format.
        /// </summary>
        /// <param name="isUpdate">Whether the command is being called from an Update function.</param>
        /// <returns>All values formatted into SQL.</returns>
        public string ToSqlString(bool isUpdate)
        {
            string[] values = { (HadAttack ? 1 : 0).ToString(), Activity, (WasEffective ? 1 : 0).ToString() };

            if(isUpdate)
            {
                values[0] =  string.Format("{0} = {1}", MetricsItems.HadAttack, values[5]);
                values[1] =  string.Format("{0} = '{1}'", MetricsItems.Activity, values[6]);
                values[2] =  string.Format("{0} = {1}", MetricsItems.WasEffective, values[7]);
            }
            else
            {
                for(int i = 0; i < values.Length; i++)
                {
                    values[i] = i == 1 ? string.Format("'{0}'", values[i]) : string.Format("{0}", values[i]);
                }
            }

            return string.Format("{0}, {1}, {2}", values[0], values[1], values[2]);
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
        #endregion
    }
}