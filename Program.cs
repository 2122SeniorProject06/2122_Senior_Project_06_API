using System;

namespace _2122_Senior_Project_06
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("Verify no SQL injection;True = invalid input, False = valid input");
            bool verifySQL = Sys_Security.VerifySQL("SELECT * FROM articles WHERE articleid = 1 AND 1=1");
            Console.WriteLine("User inputted password: SELECT * FROM articles WHERE articleid = 1 AND 1=1");
            Sys_Security.printBool(verifySQL);
            Console.WriteLine();

            Console.WriteLine("Verifies password for new account satisfies password policy; True = valid, False = invalid");
            bool verify_newAcc = Sys_Security.CreateNewAcc("SuitablePassword0@7");
            Console.WriteLine("User inputted password: SuitablePassword0@7");
            Sys_Security.printBool(verify_newAcc);
            Console.WriteLine();

            Console.WriteLine("Verifies password with stored password; True = vaild, False = invalid");
            bool verify_pass = Sys_Security.Verify_Pass("SecurePasscode@1", Sys_Security.SHA256_Hash("SecurePasscode@1"));
            Console.WriteLine("User inputted password: SecurePasscode@1");
            Sys_Security.printBool(verify_pass);
            Console.WriteLine();
        }
    }
}
