using Xunit;
using System;
using _2122_Senior_Project_06;

namespace _2122_Senior_Project_06
{
    public class UnitTest1
    {
        [Fact]
        public void Test1() // Test Password Policy
        {
            string testPassword = "GoodTestPassword01";
            string testPassword2 = "badpass01";
            Sys_Security.VerifyNewPass(testPassword);
            Sys_Security.VerifyNewPass(testPassword2);
        }

        [Fact]
        public void Test2() // Test Login
        {
            
        }

        [Fact]
        public void Test3() // Test new Journal entry
        {
            
        }

        [Fact]
        public void Test4() // Test Get list of Journals
        {
            
        }

        [Fact]
        public void Test5() // Test Select journal
        {
            
        }

        [Fact]
        public void Test6() // Test Delete journal
        {
            
        }
    }
}

