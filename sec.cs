using System;
using System.Text;
using System.Security.Cryptography;

namespace _2122_Senior_Project_06
{
    /// <summary>
    /// Class in charge of system and cyber security.
    /// </summary>
    /// <remarks>  Created by Andrew Bevilacqua. Last updated on 12/09/21.  </remarks>
    public class Sys_Security
    {
        /// <summary>
        /// Converts passed value into hashed SHA256 form.
        /// </summary>
        /// <param name="args">Value to hash.</param>
        /// <returns>The hashed value.</returns>
        public static string SHA256_Hash(string args)
        {
            SHA256Managed _sha256 = new SHA256Managed();
            byte[] _cipherText = _sha256.ComputeHash(Encoding.Default.GetBytes(args));
            return Convert.ToBase64String(_cipherText);
        }

        /// <summary>
        /// Verify input is not a SQL injection attack.
        /// </summary>
        /// <param name="args">The value to check.</param>
        /// <returns>If the value is a clean input or not.</returns>
        public static bool VerifySQL(string args) 
        {
            
            bool isSQLInjection = false;
            string[] sqlCheckList = { "--", ";--", ";", "/*" ,
                    "*/" ,"@@" ,"@" ,"char" ,"nchar" ,"varchar" ,"nvarchar" ,
                    "alter" ,"begin", "cast", "create", "cursor", "declare", 
                    "delete", "drop", "end", "exec", "execute", "fetch", "insert", 
                    "kill", "select", "sys", "sysobjects", "syscolumns", "table", 
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
        /// Prints the boolean value to the console.
        /// </summary>
        /// <param name="verify">The boolean value to print.</param>
        public static void printBool(bool verify)
        {
            if(verify)
            {
                Console.WriteLine("True");
            }
            else
            {
                Console.WriteLine("False");
            }
        }

        /// <summary>
        /// Verify if the provided password matches the stored password.
        /// </summary>
        /// <param name="Curr_pass">The password provided by the user.</param>
        /// <param name="Stored_pass">The password retrieved by the database.</param>
        /// <returns>Whether the passwords match or not.</returns>
        public static bool Verify_Pass(string Curr_pass, string Stored_pass) // verifies inputted passwords matches stored
        {

            // **check if Admin/ has admin permission**

            string Curr_hash = SHA256_Hash(Curr_pass);  // hashes inputted password with SHA256
            bool verify = false;
            int entry_attempt = 0;
            if (Curr_hash == Stored_pass) //if the passwords match
            {
                verify = true; //cant i just say return true? take away the verify all together
                // allow user's access to application(under their account)
                //      Should return verify = true, prefrom extra operation?
            }
            else // if passwords do not match
            {
                Console.WriteLine("The username or password do not match.");
                //if(entry_attempt == 10)
                //{
                    //lock account? send warning to email on file + lock account?
                //}
                entry_attempt++;
                verify = false;
            }
            Console.WriteLine("The SHA256 hash of the password, "+ Curr_pass+ ", is: " + Curr_hash);
            return verify;
        }

        /// <summary>
        /// Checks username and password against registration policies.
        /// </summary>
        /// <param name="args">Password to check.</param>
        /// <returns>If the password matches the registration policies.</returns>
        public static bool CreateNewAcc(string args)
        {
            bool verify_pass = false; // boolean that signifies if the password meets requirements

            bool verify_length = false; // boolean that signifies if the password meets length requirements
            bool no_SQL = false; // boolean that signifies if the password does not contain SQL commands
            bool verify_Upper_char = false; // boolean that signifies if the password meets upper case character requirements
            bool verify_num = false; // boolean that signifies if the password meets number character requirements

            if(args.Length >= 8 && args.Length <= 64) // if password length is between 8 and 64 characters
            {
                verify_length = true;
                if(VerifySQL(args)) // if the password does not contain SQL commands
                {
                    no_SQL = true;
                    foreach (char characters in args)
                    {
                        if(characters.ToString() == char.ToUpper(characters).ToString()) // checks if the inputted string contains an upper case letter
                        {
                            verify_Upper_char = true;
                        }
                    }
                    foreach (char characters in args)
                    {
                        if(char.IsDigit(characters)) // checks if the inputted string contains a number
                        {
                            verify_num = true;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Password invalid.");
                }
            }
            else
            {
                Console.WriteLine("Password length invaild.");
            }
            if(verify_length && no_SQL && verify_Upper_char && verify_num) // double checks if the password requirements are met
            {
                verify_pass = true; // if the requirements are met then the password is approved
            }

            // current implementatin is subject to change. This is a rough implementations and does not account for special characters
            // certain passwords conflict with VerifySQL() function
            // **save hashed password to database**

            return verify_pass;;
        } 
        
    }
}
