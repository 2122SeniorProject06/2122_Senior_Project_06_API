using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2122_Senior_Project_06.Models;
using _2122_Senior_Project_06.SqlDatabase;

namespace _2122_Senior_Project_06.Controllers
{
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("[controller]")]
    public class NewAccountController : ControllerBase
    {
        
        /*
         * The following controller processes a new account being created
         *  @ CreateNewUser
         */
        [HttpPost("Create")]
        public IActionResult CreateNewUser([FromBody] NewAccountModel potentialAccount)
        {
            if(UserAccountsDataTable.EmailInUse(potentialAccount.Email))
            {
                if(Sys_Security.VerifyNewPass(potentialAccount.Password))
                {

                UserAccount newAccount = new UserAccount(potentialAccount.Username, potentialAccount.Email,
                                                         Sys_Security.SHA256_Hash(potentialAccount.Password));
                UserAccountsDataTable.AddNewAccount(newAccount);
                return Ok();
                }
                else
                {
                    return Forbid();
                    /* 
                        Return error message "Password does not meet password requirements."
                        Include password policy:
                                - Minimum of 8 character
                                - One upper case letter
                                - One lower case letter
                                - One number
                    */
                }
            }
            else
            {
                return Forbid();
                /* 
                   Return error message "Email is already in use."
                */
            }
        }
    }
}