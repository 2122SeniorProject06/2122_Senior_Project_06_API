using System;

namespace _2122_Senior_Project_06.Exceptions
{
    internal class IssueWithCredentialException : Exception
    {
        public IssueWithCredentialException(string message) : base(message)
        {
        }

        public IssueWithCredentialException()
        {
        }
    }   
}