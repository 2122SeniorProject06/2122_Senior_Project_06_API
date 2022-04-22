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
using _2122_Senior_Project_06.Types;
using _2122_Senior_Project_06.Exceptions;

namespace _2122_Senior_Project_06.Controllers
{
    /// <summary>
    /// The API's account creation controller.
    /// </summary>
    ///  <remarks> Paired programmed by Andrew and Sarah. </remarks>
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
        public NewAccountModel CreateNewUser([FromBody] NewAccountModel potentialAccount)
        {
            potentialAccount.VerificationErrors = new string[4];
            potentialAccount.VerificationResults = new bool[4];
            string[] errorTypes = {"Email is invalid.",
                                    "Username is invalid.",
                                    "Password is invalid.",
                                    "Passwords do not match."};
            
            // {"The password must be at least more than 8 lengths.",
            //  "The password must contain at least one lowercase character.",
            //  "The password must contain at least one capital character.",
            //  "The password must contain at least one number." };
            try
            {
                if(Sys_Security.VerifyEmail(potentialAccount.Email))//if email is an email and if email is not already in use
                {
                    if(!UserAccountsDataTable.EmailInUse(potentialAccount.Email))
                        potentialAccount.VerificationResults[0] = true;

                    else throw new IssueWithCredentialException("Email already in use.");
                }
            }
            catch(IssueWithCredentialException e)
            {
                potentialAccount.VerificationResults[0] = false;
                potentialAccount.VerificationErrors[0] = e.Message;
            }

            if(potentialAccount.Password == potentialAccount.confirmedPassword)
            {
                potentialAccount.VerificationResults[1] = true;
            }
            else
            {
                potentialAccount.VerificationResults[1] = false;
                potentialAccount.VerificationErrors[1] = "Passwords do not match.";
            }

            if(!string.IsNullOrEmpty(potentialAccount.Username))//checks if user name is empty
            {
                potentialAccount.VerificationResults[2] = true;
            }
            else
            {
                potentialAccount.VerificationResults[2] = false;
                potentialAccount.VerificationErrors[2] = "Username is invalid.";
            }

            try
            {
                potentialAccount.VerificationResults[3] = Sys_Security.VerifyNewPass(potentialAccount.Password);
            }
            catch(IssueWithCredentialException e)
            {
                potentialAccount.VerificationResults[3] = false;
                potentialAccount.VerificationErrors[3] = e.Message;
            }
            
            /* 
                Return error message "Password does not meet password requirements."
                Include password policy:
                        - Minimum of 8 character
                        - One upper case letter
                        - One lower case letter
                        - One number
            */
            if(!potentialAccount.VerificationResults.Contains(false)) //If everything is ok then we create account
            {
                UserAccount newAccount = new UserAccount(potentialAccount.Username, potentialAccount.Email,
                                                        Sys_Security.SHA256_Hash(potentialAccount.Password));
                UserAccountsDataTable.AddNewAccount(newAccount);
            }

            potentialAccount.confirmedPassword = null;
            potentialAccount.Email = null;
            potentialAccount.Password = null;
            potentialAccount.Username = null;
            return potentialAccount;
        }

        // [HttpPost("ErrorMess")]
        // public int checkInput([FromBody] NewAccountModel potentialAccount)
        // {
        //     bool check0 = false;
        //     bool check1 = false;
        //     bool check2 = false;
        //     int value = 0;
        //     if(potentialAccount.Username == null)
        //     {
                
        //     }
        //     if(Sys_Security.VerifyEmail(potentialAccount.Email))
        //     {
        //         value += 1;
        //     }
        // }
        [HttpPost("EmailCheck")]
        public bool CheckEmail([FromBody] string curr_email)
        {
            if(Sys_Security.VerifyEmail(curr_email))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        [HttpPost("PassCheck")]
        public bool CheckPass([FromBody] string curr_pass)
        {
            if(Sys_Security.VerifyNewPass(curr_pass))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}