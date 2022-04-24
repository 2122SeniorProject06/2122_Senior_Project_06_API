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
     * The following controller pulls information from the database for a users account
     *  @ AuthenticateUser
     *  @ RetrieveID(maybe on database end?)
     */

    /// <summary>
    /// The API's login controller.
    /// </summary>
    ///  <remarks> Ok so i have very little idea how this is going to work, needs to be discussed with hugo and sarah.
    ///            Most likely we will need to make a new model as well + new SQL functions  </remarks>
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("[controller]")]
    public class MetricController : ControllerBase
    {
        [HttpGet("GetMetrics")]
        public MerticsModel GetUserMetrics(string userID)
        {
            List<Metrics> userMetricData = null;
            MerticsModel userMetrics = null;

            if(Sys_Security.VerifySQL(userID))
            {
                if(UserAccountsDataTable.UIDInUse(userID))
                {
                    userMetrics = new MerticsModel();
                    /*
                        Occurance Key:
                        [0]: Focus 
                        [1]: Ground
                        [2]: Relax
                        [3]: Breathe
                        [4]: Encorage
                        [5]: Check In
                    */
                    userMetrics.ActivityOccurances = new int[5];
                    userMetrics.MostEffective = new string[3];
                    userMetricData = JournalsDataTable.GetMetricsWithUserId(userID);
                    for(int i = 0; i < userMetricData.Count; i++)
                    {
                        if(userMetricData[i].WasEffective)
                        {
                            if(userMetricData[i].Activity == ActivityItems.Focus)
                            {
                                userMetrics.ActivityOccurances[0] += 1;
                            }
                            else if(userMetricData[i].Activity == ActivityItems.Ground)
                            {
                                userMetrics.ActivityOccurances[1] += 1;
                            }
                            else if(userMetricData[i].Activity == ActivityItems.Relax)
                            {
                                userMetrics.ActivityOccurances[2] += 1;
                            }
                            else if(userMetricData[i].Activity == ActivityItems.Breathe)
                            {
                                userMetrics.ActivityOccurances[3] += 1;
                            }
                            else if(userMetricData[i].Activity == ActivityItems.Encourage)
                            {
                                userMetrics.ActivityOccurances[4] += 1;
                            }
                            else if(userMetricData[i].Activity == ActivityItems.CheckIn)
                            {
                                userMetrics.ActivityOccurances[5] += 1;
                            }
                        }
                    }
                    int[] sortedArray = userMetrics.ActivityOccurances;
                    Array.Sort(sortedArray);
                    Array.Reverse(sortedArray);
                    int[] topThree = sortedArray.Take(3).ToArray();

                    for(int x = 0; x < (userMetrics.ActivityOccurances).Length; x++)
                    {
                        if(userMetrics.ActivityOccurances[x] == topThree[0] && string.IsNullOrEmpty(userMetrics.MostEffective[0]))
                        {
                            userMetrics.MostEffective[0] = ActivityItems.MatchToActivity(x);
                        }
                        else if(userMetrics.ActivityOccurances[x] == topThree[1] && string.IsNullOrEmpty(userMetrics.MostEffective[1]))
                        {
                            userMetrics.MostEffective[1] = ActivityItems.MatchToActivity(x);
                        }
                        else if(userMetrics.ActivityOccurances[x] == topThree[2] && string.IsNullOrEmpty(userMetrics.MostEffective[2]))
                        {
                            userMetrics.MostEffective[2] = ActivityItems.MatchToActivity(x);
                        }
                    }   
                }
            }
            return userMetrics;
        }
    }
}