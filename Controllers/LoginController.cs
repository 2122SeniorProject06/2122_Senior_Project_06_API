using Microsoft.AspNetCore.Http;
using _2122_Senior_Project_06.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Data;
using _2122_Senior_Project_06.SqlDatabase;

namespace _2122_Senior_Project_06.Controllers
{
     /*
     * The following controller processes a user logging in
     *  @ AuthenticateUser
     *  @ RetrieveID(maybe on database end?)
     */

    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// Authenticates the provided user login information.
        /// </summary>
        /// <param name="loginModel">The provided login credentials.</param>
        /// <returns>The account userID, or -1 if login failed.</returns>
        [HttpPost("Authenticate")]
        public int AuthenticateUser([FromBody]LoginPage loginModel){
            int userID = -1;
            if(Sys_Security.VerifyPass(loginModel.Password, loginModel.Email))
            {
                userID = UserAccountsDataTable.GetUIDFromEmail(loginModel.Email);
            }  
            return userID;
        }
    }
}