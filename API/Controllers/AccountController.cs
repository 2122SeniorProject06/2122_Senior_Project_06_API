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
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// Gets all the metrics and returns to the account page.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>A list of Journal Entries with only the metrics</returns>
        /// <remarks> This function can most likely be scrapped since all user metrics is handled with the metricsController
        ///             This function may cause errors when used in frontend.
        ///</remarks>
        [HttpGet("GetMetrics")]
        public List<Metrics> GetMetrics(string userID)
        {
            return JournalsDataTable.GetMetricsWithUserId(userID);
        }

        /// <summary>
        /// Returns a user's email, username, current badges given a userID
        /// </summary>
        /// <param name="userID">UserID obtained through login</param>
        /// <returns>User's info from database</returns>
        [HttpGet("GetAllInfo")]
        public UserAccount GetUserInfo(string userID)
        {
            if(Sys_Security.VerifySQL(userID))
            {
                if(UserAccountsDataTable.UIDInUse(userID))
                {
                    UserAccount returnValue = UserAccountsDataTable.GetAccount(userID);
                    returnValue.Password = null;
                    return returnValue;
                }
                else
                {
                    return null; //User does not exist
                }
            }
            else
            {
                return null; //SQL injection
            }
        }

        /// <summary>
        /// Updates users info no matter type of data
        /// User can update Email, Username or Password
        /// </summary>
        /// <param name="UserAccount">UserAccount is obtained dependent on what the user would like to update</param>
        /// <returns>IActionResult, Ok() if successful, Forbid() if SQLinjection/user DNE. 
        ///     If successful but user inputted invalid information then error is displayed to user</returns>
        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser([FromBody]AccountModel potentialUpdate) //might need to send UID seperate
        {
            if(Sys_Security.VerifySQL(potentialUpdate.userID))
            {
                /*
                    Bool Key:
                    [0]: password
                    [1]: confirmPassword
                    [2]: email
                    [3]: username
                */
                potentialUpdate.VerificationErrors = new string[3];
                potentialUpdate.VerificationResults = new bool[3];
                if(UserAccountsDataTable.UIDInUse(potentialUpdate.userID))
                {
                    UserAccount currInfo = UserAccountsDataTable.GetAccount(potentialUpdate.userID);
                    if(potentialUpdate.new_Password != null)
                    {
                        try
                        {
                            if(potentialUpdate.new_Password == potentialUpdate.confirmedPassword)
                            {
                                try
                                {
                                    potentialUpdate.VerificationResults[0] = Sys_Security.VerifyNewPass(potentialUpdate.new_Password);
                                    potentialUpdate.VerificationResults[1] = true;
                                }
                                catch(IssueWithCredentialException e)
                                {
                                    potentialUpdate.VerificationResults[0] = false;
                                    potentialUpdate.VerificationErrors[0] = e.Message;
                                }
                            }
                            else throw new IssueWithCredentialException("Passowrds do not match.");
                        }
                        catch(IssueWithCredentialException e)
                        {
                            potentialUpdate.VerificationResults[1] = false;
                            potentialUpdate.VerificationErrors[1] = e.Message;
                        }
                        
                    }
                    else 
                    {
                        potentialUpdate.VerificationResults[0] = true;
                        potentialUpdate.VerificationResults[1] = true;
                    }

                    if(potentialUpdate.new_Email != null)
                    {
                        try
                        {
                            if(Sys_Security.VerifyEmail(potentialUpdate.new_Email))//if email is an email and if email is not already in use
                            {
                                if(!UserAccountsDataTable.EmailInUse(potentialUpdate.new_Email))
                                    potentialUpdate.VerificationResults[2] = true;

                                else throw new IssueWithCredentialException("Email already in use.");
                            }
                            else throw new IssueWithCredentialException("Email is not valid.");
                        }
                        catch(IssueWithCredentialException e)
                        {
                            potentialUpdate.VerificationResults[2] = false;
                            potentialUpdate.VerificationErrors[2] = e.Message;
                        }
                    }
                    else potentialUpdate.VerificationResults[2] = true;

                    if(potentialUpdate.new_Username != null)
                    {
                        potentialUpdate.VerificationResults[3] = true;
                    }
                    else potentialUpdate.VerificationResults[3] = true;

                    if(!potentialUpdate.VerificationResults.Contains(false))
                    {
                        UserAccount updatedUser = new UserAccount(potentialUpdate.new_Username, potentialUpdate.new_Email,
                                                        Sys_Security.SHA256_Hash(potentialUpdate.new_Password), potentialUpdate.DarkMode, potentialUpdate.Background);
                        currInfo.UpdateInfo(updatedUser);
                        UserAccountsDataTable.UpdateUserAccount(currInfo);
                    } 

                    potentialUpdate.confirmedPassword = null;
                    potentialUpdate.new_Email = null;
                    potentialUpdate.new_Password = null;
                    potentialUpdate.userID = null;
                    potentialUpdate.new_Username = null;
                    return Ok(potentialUpdate);
                }
                else
                {
                    return Forbid();
                }
            }
            else
            {
                return Forbid();
            }
            /*
                Should we verify password first?
                        Password stored in database is hashed so we cant tell what it is from our end
                UI sends in curr_pass and user's email, verify password matches continue to change/update password
                    Make sure new password satifies password policy
                        Change password in database
            */
        }

        /// <summary>
        /// Changes a users username while they are in their account settings
        /// As far as verification UI may need to send userID to both verify, pull current username, and change username
        /// </summary>
        /// <param name="userID">UserID obtained through login</param>
        /// <returns>OK() if change as successful, or error code if not?</returns>
        [HttpPut("UpdateUsername")]
        public IActionResult ChangeUsername()
        {
            /*
                change username in database

            */
            return Ok();
        }

        /// <summary>
        /// Changes a users email while they are in their account settings
        /// As far as verification UI may need to send userID to both verify, pull current email, and change email
        /// </summary>
        /// <param name=""></param>
        /// <returns>OK() if change as successful, or error code if not?</returns>
        [HttpPut("UpdateEmail")]
        public IActionResult ChangeEmail()
        {
            /*
                Verify that the email is in use
                    if email is not in use then something reaaaal bad has happened
                    if email is in use then continue with change/update
                        verfiy email is indeed an email
                            change email in database

            */
            return Ok();
        }
        /*
            get badges function?
        */

    }
}