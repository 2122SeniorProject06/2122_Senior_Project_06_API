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
        public int CreateNewUser([FromBody] NewAccountModel potentialAccount)
        {
            int val = 0;
            if(Sys_Security.VerifyEmail(potentialAccount.Email) && 
                !UserAccountsDataTable.EmailInUse(potentialAccount.Email))//if email is an email and if email is not already in use
            {
                val += 0;
            }
            else
            {
                val += 1;
            }
            if(potentialAccount.Username != null)//checks if user name is empty
            {
                val += 0;
            }
            else
            {
                val += 3;
            }
            if(Sys_Security.VerifyNewPass(potentialAccount.Password))//checks if password meets requirements
            {
                val += 0;
            }
            else
            {
                val += 9;
                /* 
                    Return error message "Password does not meet password requirements."
                    Include password policy:
                            - Minimum of 8 character
                            - One upper case letter
                            - One lower case letter
                            - One number
                */
            }
            if(val == 0) //If everything is ok then we create account and return val(which will be 0)
            {
                UserAccount newAccount = new UserAccount(potentialAccount.Username, potentialAccount.Email,
                                                        Sys_Security.SHA256_Hash(potentialAccount.Password));
                UserAccountsDataTable.AddNewAccount(newAccount);
                return val;
            }
            else//if an error occured then we return val. Val could be: 1,3,9,4,10,12
            {
                return val;
            }
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