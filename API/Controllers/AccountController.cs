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
                    
                    return UserAccountsDataTable.GetAccount(userID);
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
        /// </summary>
        /// <param name="UserAccount">UserAccount is obtained dependent on what the user would like to update</param>
        /// <returns>IActionResult, Ok() if successful, Forbid() if invalid/user DNE</returns>
        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser([FromBody]UserAccount updatedUser) //might need to send UID seperate
        {
            if(Sys_Security.VerifySQL(updatedUser.UserID))
            {
                if(UserAccountsDataTable.UIDInUse(updatedUser.UserID))
                {
                    UserAccountsDataTable.UpdateUserAccount(updatedUser);
                    return Ok();
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