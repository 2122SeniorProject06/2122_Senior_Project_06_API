using System;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Collections.Generic;
using _2122_Senior_Project_06.SqlDatabase;
using _2122_Senior_Project_06.Exceptions;
using System.Text.RegularExpressions;

namespace _2122_Senior_Project_06
{
    /// <summary>
    /// Class in charge of system and cyber security.
    /// </summary>
    /// <remarks> Created by Andrew and Hugo. </remarks>
    internal class Sys_Security
    {
        /// <summary>
        /// Verify input is not a SQL injection attack.
        /// </summary>
        /// <param name="args">The value to check.</param>
        /// <returns>This solely used to check a string for certain words or series of characters.
        /// Name was never changed, SQL inejection mitigated by URLencoding</returns>
        private static bool SQLCheck(string args) 
        {
            
            bool isSQLInjection = false;
            string[] sqlCheckList = { "--", ";--", ";", "/*" ,
                    "*/" ,"@@" ,"char" ,"nchar" ,"varchar" ,"nvarchar" ,
                    "alter" ,"begin", "cast", "create", "cursor", "declare", 
                    "delete", "drop", "end", "exec", "execute", "fetch", "insert", 
                    "select", "sys", "sysobjects", "syscolumns", "table", 
                    "update"}; // array of know SQL commands
            string CheckString = args.Replace("'", "''");

            for (int i = 0; i <= sqlCheckList.Length - 1; i++)
            {

                if ((CheckString.IndexOf(sqlCheckList[i], StringComparison.OrdinalIgnoreCase) >= 0))
                {    

                    isSQLInjection = true;

                }

            }

            return isSQLInjection;
            // This is a temporary implementation, certain characters and strings that SHOULD be viable
            // in a valid password conflict with they sqlChecklist array
        }

        /// <summary>
        /// Encodes and Decodes text with Http text
        /// </summary>
        /// <param name="args">The value to code.</param>
        /// <returns>cipherText or plainText</returns>
        private static string Encode(string args)
        {
            return HttpUtility.UrlEncode(args);
        }
        private static string Decode(string args)
        {
            return HttpUtility.UrlDecode(args);
        }

        /// <summary>
        /// Verify if the provided password matches the stored password.
        /// </summary>
        /// <param name="Curr_pass">The password provided by the user.</param>
        /// <param name="Stored_pass">The password retrieved by the database.</param>
        /// <returns>Whether the passwords match or not.</returns>
        private static bool PassCheck(string curr, string email)
        {
            /*if(64_bit(Curr) == "64_bit of Admin name"
               {
                    TERMINATE
               }*/

            string Curr_hash = SHA256_Hash(curr);
            string Stored_hash = UserAccountsDataTable.GetPassFromEmail(email);
            //int entry_attempt = 0;
            if (Curr_hash == Stored_hash)
            {
                return true;
            }
            else
            {
                //if(entry_attempt == 10)
                //{
                    //lock account? send warning to email on file + lock account?
                //}
                return false;
            }
        }

        /// <summary>
        /// Checks username and password against registration policies.
        /// </summary>
        /// <param name="args">Password to check.</param>
        /// <returns>If the password matches the registration policies.</returns>
        private static bool NPassCheck(string args)
        {
            bool[] isValid = new bool[4];
            /*
                [0]:between character requirements
                [1]:has lowercase letter
                [2]:has capital letter
                [3]:has number
            */

            if(args.Length >= 8 && args.Length <= 64)
            {
                isValid[0] = true;
            }
            foreach (char characters in args)
            {
                if((int)characters >= 97 && (int)characters <= 122)
                {
                    isValid[1] = true;
                }
                if ((int)characters >= 65 && (int)characters <= 90)
                {
                    isValid[2] = true;
                }
                if ((int)characters >= 48 && (int)characters <= 57)
                {
                    isValid[3] = true;
                }
            }
            if(isValid[0] && isValid[1] && isValid[2] && isValid[3])
            {
                return true;
            }
            else
            {
                string[] errorTypes = {"be at least 8 characters",
                                       "contain a lowercase letter",
                                       "contain a capital letter",
                                       "contain a number" };
                string message = "The password must: ";
                for(int i = 0; i < isValid.Length; i++)
                {
                    if(!isValid[i])
                    {
                        message += errorTypes[i] + ", ";
                    }
                }
                message = Regex.Replace(message, ", $", ".");
                throw new IssueWithCredentialException(message);
            }
        }

        /*
         *      Public Functions
         */

        /// <summary>
        /// Takes in any string and returns if it contains possible SQL functions
        /// </summary>
        /// <param name="args">The value to check.</param>
        /// <returns>If the value is clean or not.</returns>
        /// <remarks>Public Function for SQL verification.</remarks>
        public static bool VerifySQL(string args)
        {
            return(!SQLCheck(args));
        }

        //Verifies current password matches that stored in database
        //Curr_password is sent as plain text(gets hashed while in PassCheck)
        //Both Curr_password and Curr_username are encoded(using the Encoder() function) before entering function(See LoginController.cs)
        public static bool VerifyPass(string curr_password, string email)
        {
            bool passCheckResult = false;
            try
            {
                if(VerifyEmail(email) && UserAccountsDataTable.EmailInUse(email))
                {
                        passCheckResult = PassCheck(curr_password,email);
                }
            }
            catch(IssueWithCredentialException) {}
            
            return passCheckResult;
        }

        //Verifies Newpassword satifies password requirements
        //Current implementation: NewPassword is sent as plaintext(Not encoded), returns a boolean if password satifies requirements
        public static bool VerifyNewPass(string args)
        {
            return(NPassCheck(args));
        }
        
        public static string Encoder(string args)
        {
            return Encode(args);
        }
        public static string Decoder(string args)
        {
            return Decode(args);
        }

        /// <summary>
        /// Converts passed value into hashed SHA256 form.
        /// </summary>
        /// <param name="args">Value to hash.</param>
        /// <returns>The hashed value.</returns>
        public static string SHA256_Hash(string args) //Can turn to private
        {
            if(!String.IsNullOrEmpty(args))
            {
                SHA256Managed _sha256 = new SHA256Managed();
                byte[] _cipherText = _sha256.ComputeHash(Encoding.Default.GetBytes(args));
                return Convert.ToBase64String(_cipherText);
            }
            else return null;
        }
    
        public static string GenID(string ID, bool type)
        {
            bool in_Use = true;
            Random rnd = new Random();
            while(in_Use)
            {
                ID = null;
                for(int i = 0; i<=8;i++)
                {
                    int a = rnd.Next(0,36);
                    char val;
                    if(a < 26)
                    {
                        val = (char)(a + 65);
                    }
                    else
                    {
                        val = (char)(a + 22);
                    }
                    ID += val;
                }
                if(type)
                {
                    in_Use = UserAccountsDataTable.UIDInUse(ID);
                }
                else
                {
                    in_Use = JournalsDataTable.JournalIDInUse(ID);
                }
            }
            return ID;   
        }

        public static bool VerifyEmail(string potentialEmail)
        {
            bool[] isValid = new bool[2];
            foreach(char c in potentialEmail)
            {
                if((int)c == 64) //Check for @
                {
                    isValid[0] = true;
                    
                }
                if ((int)c == 46) //Check for .
                {
                    isValid[1] = true;
                }
            }
            if(isValid[0] && isValid[1])
            {
                return true;
            }
            else
            {
                throw new IssueWithCredentialException("Not a valid email.");
            }
        }
    }
}
