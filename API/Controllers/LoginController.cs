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
using _2122_Senior_Project_06.SqlDatabase;
using _2122_Senior_Project_06.Types;

namespace _2122_Senior_Project_06.Controllers
{
     /*
     * The following controller processes a user logging in
     *  @ AuthenticateUser
     */

    /// <summary>
    /// The API's login controller.
    /// </summary>
    ///  <remarks> Paired programmed by Andrew and Sarah. </remarks>
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// Authenticates the provided user login information.
        /// </summary>
        /// <param name="loginModel">The provided login credentials.</param>
        /// <returns>The account userID, or -1 if login failed.</returns>
        [HttpPost("Authenticate")]
        public UserAccount AuthenticateUser([FromBody]LoginPage loginModel){
            string userID = null;
            UserAccount curr_User = null;
            loginModel.UserPrefrences = new List<string>();
            try{
            if(Sys_Security.VerifyPass(loginModel.Password, loginModel.Email))
            {
                userID = UserAccountsDataTable.GetUIDFromEmail(loginModel.Email);
                if(!string.IsNullOrEmpty(userID))
                {
                    curr_User = UserAccountsDataTable.GetAccount(userID);        
                    curr_User.Password = null;
                    curr_User.Email = null;
                    curr_User.Username = null;
                }
            }}
            catch(Exception e){
                curr_User.UserID = e.Message;
            }
            return curr_User;
        }
    }
}