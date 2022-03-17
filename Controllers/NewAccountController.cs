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
        [HttpPost("SuperSecretBaseValueGeneration")]
        public string GenerateUsers(string password)
        {
            if(password == "TheKeyToSecrecyIsLength")
            {
                UserAccount sarah = new UserAccount("Sarah", "email1@gmail.com", Sys_Security.SHA256_Hash("G00lsby"));
                UserAccount hugo = new UserAccount ("Hugo", "email2@gmail.com", Sys_Security.SHA256_Hash("M@zariego"));
                UserAccount andrew = new UserAccount ("Andrew", "email3@gmail.com", Sys_Security.SHA256_Hash("Bev!lacqua"));
                UserAccount ulysses = new UserAccount ("Ulysses", "email4@gmail.com", Sys_Security.SHA256_Hash("Riv&ra"));
                UserAccount dani = new UserAccount ("Dani", "email5@gmail.com", Sys_Security.SHA256_Hash("Mar+inez"));
                UserAccountsDataTable.AddNewAccount(sarah);
                UserAccountsDataTable.AddNewAccount(hugo);
                UserAccountsDataTable.AddNewAccount(andrew);
                UserAccountsDataTable.AddNewAccount(ulysses);
                UserAccountsDataTable.AddNewAccount(dani);
                return "MissionComplete";
            }

            else if(password == "SuperSecretBaseValueGenerationPassword")
                return "You are so stupid. Did you really think that we would just give you the password?";

            else return "Incorrect Password. The correct password is \"SuperSecretBaseValueGenerationPassword\".";
        }
        /*
         * The following controller processes a new account being created
         *  @ CreateNewUser
         */
        [HttpPost("Create")]
        public bool CreateNewUser([FromBody] NewAccountModel potentialAccount)
        {
            if(Sys_Security.VerifyEmail(potentialAccount.Email))
            {
                if(!UserAccountsDataTable.EmailInUse(potentialAccount.Email))
                {
                    if(Sys_Security.VerifyNewPass(potentialAccount.Password))
                    {

                    UserAccount newAccount = new UserAccount(potentialAccount.Username, potentialAccount.Email,
                                                            Sys_Security.SHA256_Hash(potentialAccount.Password));
                    UserAccountsDataTable.AddNewAccount(newAccount);
                    return true;
                    }
                    else
                    {
                        return false;
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
                    return false;
                    /* 
                    Return error message "Email is already in use."
                    */
                }
            }
            else
            {
                return false;
                /*
                Return error message "Email is not valid"
                */
            }
        }
    }
}