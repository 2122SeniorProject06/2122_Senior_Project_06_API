using Microsoft.AspNetCore.Http;
using _2122_Senior_Project_06.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using System.Data;
using _2122_Senior_Project_06.Types;
using _2122_Senior_Project_06.SqlDatabase;
using _2122_Senior_Project_06.Exceptions;

namespace _2122_Senior_Project_06.Controllers
{
     /*
     * The following controller pulls information from the database for a users account metrics
     *  @ GetOccurances
     *  @ GetMostEffective
     */

    /// <summary>
    /// The API's metric controller.
    /// </summary>
    ///  <remarks> Allows for user metrics to be pulled and processed. Functions produce arrays containing 
    ///     most effective activities and occurances for each activity(as long as it was effective)  
    ///  </remarks>
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("[controller]")]
    public class MetricController : ControllerBase
    {
        /// <summary>
        /// Gets all the activity occurances by user and calculates which activites were helpful and returns int[] containing said values.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>int[] containing occurances of effective activites</returns>
        [HttpGet("GetOccurances")]
        public int[] GetOccurances(string userID)
        {
            List<Metrics> userMetricData = null;
            int[] userOccurances = new int[6];

            if(Sys_Security.VerifySQL(userID))
            {
                if(UserAccountsDataTable.UIDInUse(userID))
                {
                    /*
                        Occurance Key:
                        [0]: Focus 
                        [1]: Ground
                        [2]: Relax
                        [3]: Breathe
                        [4]: Encorage
                        [5]: Check In
                    */
                    userMetricData = JournalsDataTable.GetMetricsWithUserId(userID);
                    for(int i = 0; i < userMetricData.Count; i++)
                    {
                        if(userMetricData[i].WasEffective)
                        {
                            if(userMetricData[i].Activity == ActivityItems.Focus)
                            {
                                userOccurances[0] += 1;
                            }
                            else if(userMetricData[i].Activity == ActivityItems.Ground)
                            {
                                userOccurances[1] += 1;
                            }
                            else if(userMetricData[i].Activity == ActivityItems.Relax)
                            {
                                userOccurances[2] += 1;
                            }
                            else if(userMetricData[i].Activity == ActivityItems.Breathe)
                            {
                                userOccurances[3] += 1;
                            }
                            else if(userMetricData[i].Activity == ActivityItems.Encourage)
                            {
                                userOccurances[4] += 1;
                            }
                            else if(userMetricData[i].Activity == ActivityItems.CheckIn)
                            {
                                userOccurances[5] += 1;
                            }
                        }
                    }   
                }
            }
            return userOccurances;
        }

        [HttpGet("GetTotalOccurances")]
        public int[] TotalOccurances(string userID)
        {
            List<Metrics> userMetricData = null;
            int[] userOccurances = new int[6];

            if(Sys_Security.VerifySQL(userID))
            {
                if(UserAccountsDataTable.UIDInUse(userID))
                {
                    /*
                        Occurance Key:
                        [0]: Focus 
                        [1]: Ground
                        [2]: Relax
                        [3]: Breathe
                        [4]: Encorage
                        [5]: Check In
                    */
                    userMetricData = JournalsDataTable.GetMetricsWithUserId(userID);
                    for(int i = 0; i < userMetricData.Count; i++)
                    {
                        if(userMetricData[i].Activity == ActivityItems.Focus)
                        {
                            userOccurances[0] += 1;
                        }
                        else if(userMetricData[i].Activity == ActivityItems.Ground)
                        {
                            userOccurances[1] += 1;
                        }
                        else if(userMetricData[i].Activity == ActivityItems.Relax)
                        {
                            userOccurances[2] += 1;
                        }
                        else if(userMetricData[i].Activity == ActivityItems.Breathe)
                        {
                            userOccurances[3] += 1;
                        }
                        else if(userMetricData[i].Activity == ActivityItems.Encourage)
                        {
                            userOccurances[4] += 1;
                        }
                        else if(userMetricData[i].Activity == ActivityItems.CheckIn)
                        {
                            userOccurances[5] += 1;
                        }
                    }   
                }
            }
            return userOccurances;
        }
        


        /// <summary>
        /// Gets top three most effective activities
        /// </summary>
        /// <param name="userID","userOccurances"></param>
        /// <returns>string[] contianing top three most effective activites</returns>

        [HttpGet("MostHelpful")]
        public string[] GetMostEffective(string userID, int[] userOccurances)
        {
            string[] mostEffective = new string[3];
            if(Sys_Security.VerifySQL(userID))
            {
                if(UserAccountsDataTable.UIDInUse(userID))
                {
                    int[] sortedArray = userOccurances;
                    
                    Array.Sort(sortedArray);
                    Array.Reverse(sortedArray);
                    int[] topThree = sortedArray.Take(3).ToArray();

                    for(int x = 0; x < (userOccurances).Length; x++)
                    {
                        if(userOccurances[x] == topThree[0] && string.IsNullOrEmpty(mostEffective[0]))
                        {
                            mostEffective[0] = ActivityItems.MatchToActivity(x);
                        }
                        else if(userOccurances[x] == topThree[1] && string.IsNullOrEmpty(mostEffective[1]))
                        {
                        mostEffective[1] = ActivityItems.MatchToActivity(x);
                        }
                        else if(userOccurances[x] == topThree[2] && string.IsNullOrEmpty(mostEffective[2]))
                        {
                            mostEffective[2] = ActivityItems.MatchToActivity(x);
                        }
                    }
                }
            }
            return mostEffective;
        }
    }
}