using System;

namespace _2122_Senior_Project_06
{
    class Program
    {
        static void Main(string[] args)
        {
            // bool verify = Sys_Security.VerifySQL("SELECT * FROM articles WHERE articleid = 1 AND 1=1");
            bool verify = Sys_Security.CreateNewAcc("HugosAss1");
            if(verify)
            {
                Console.WriteLine("True");
            }
            else
            {
                Console.WriteLine("False");
            }
        }
    }
}
