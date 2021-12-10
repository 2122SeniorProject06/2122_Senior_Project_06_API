using System.Collections.Generic;
namespace _2122_Senior_Project_06
{
    /// <summary>
    /// Implementation for account database interface that allows for testing.
    /// </summary>
    /// <remarks> Made by Hugo Mazariego. Last update 12/09/2021. </remarks>
    public class TestAccountDatabase : IAccountDatabase
    {
        private List<UserAccount> tempDatabase;

        public TestAccountDatabase()
        {
            tempDatabase = new List<UserAccount>();

            UserAccount tempAccount1 = new UserAccount();
            tempAccount1.Email = "Test1@email.com";
            tempAccount1.Password = Sys_Security.SHA256_Hash("Test1");
            tempAccount1.UserID = "1";
            tempAccount1.UserName = "test1";
            tempDatabase.Add(tempAccount1);

            UserAccount tempAccount2 = new UserAccount();
            tempAccount2.Email = "Test2@email.com";
            tempAccount2.Password = Sys_Security.SHA256_Hash("Test2");
            tempAccount2.UserID = "2";
            tempAccount2.UserName = "test2";
            tempDatabase.Add(tempAccount2);

            UserAccount tempAccount3 = new UserAccount();
            tempAccount3.Email = "Test3@email.com";
            tempAccount3.Password = Sys_Security.SHA256_Hash("Test3");
            tempAccount3.UserID = "3";
            tempAccount3.UserName = "test3";
            tempDatabase.Add(tempAccount3);

            UserAccount tempAccount4 = new UserAccount();
            tempAccount4.Email = "Test4@email.com";
            tempAccount4.Password = Sys_Security.SHA256_Hash("Test4");
            tempAccount4.UserID = "4";
            tempAccount4.UserName = "test4";
            tempDatabase.Add(tempAccount4);

            UserAccount tempAccount5 = new UserAccount();
            tempAccount5.Email = "Test5@email.com";
            tempAccount5.Password = Sys_Security.SHA256_Hash("Test5");
            tempAccount5.UserID = "5";
            tempAccount5.UserName = "test5";
            tempDatabase.Add(tempAccount5);
        }

        public UserAccount GetByUsername (string username)
        {
            UserAccount userAccount = null;

            for(int i = 0; i < tempDatabase.Count; i++)
            {
                if(tempDatabase[i].UserName == username)
                {
                    userAccount = tempDatabase[i];
                    break;
                }
            }

            return userAccount;
        }
    }
}