using System;
using System.Text;
using System.Security.Cryptography;

namespace _2122_Senior_Project_06
{
    public class Sys_Security
    {
        private static string SHA256_Hash(string args) // returns the SHA256 hash of args
        {
            SHA256Managed _sha256 = new SHA256Managed();
            byte[] _cipherText = _sha256.ComputeHash(Encoding.Default.GetBytes(args));
            return Convert.ToBase64String(_cipherText);
        }
        public static bool VerifySQL(string args) // verifies args does no correlate with SQL commands; true==valid input
        {
            
            bool isSQLInjection = false;
            string[] sqlCheckList = { "--", ";--", ";", "/*" ,"*/" ,"@@" ,"@" ,"char" ,"nchar" ,"varchar" ,"nvarchar" ,"alter" 
            ,"begin", "cast", "create", "cursor", "declare", "delete", "drop", "end", "exec", "execute", "fetch", "insert", "kill"
            ,"select", "sys", "sysobjects", "syscolumns", "table", "update"}; // array of know SQL commands
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
        public static bool Verify_Pass(string Curr_pass, string Stored_pass, bool New_acc) // verifies inputted passwords matches stored
        {

            // **check if Admin/ has admin permission**

            string Curr_hash = SHA256_Hash(Curr_pass);  // hashes inputted password with SHA256
            bool verify;
            int entry_attempt = 0;
            if (Curr_pass == Stored_pass) //if the passwords match
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

            return verify;
        }
        
    }
}