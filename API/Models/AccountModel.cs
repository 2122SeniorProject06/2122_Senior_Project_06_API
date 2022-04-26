using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace  _2122_Senior_Project_06.Models
{
    //model could be used for user's account page
    public class AccountModel
    {
        public string userID { get; set; }
        public string new_Username { get; set; }
        public string new_Password { get; set; }
        public string new_Email { get; set; }
        public string confirmedPassword { get; set; }
        public bool IsSetting {get; set;}
        public bool DarkMode {get; set;}
        public string Background {get; set;}

        /// <summary>
        /// Gets and sets the results of the verification.
        /// </summary>
        /// <remarks>
        /// <para>It is a 4 element array of whether the user input is valid.</para>
        /// <para>0: Email is valid.</para>
        /// <para>1: Password inputs match.</para>
        /// <para>2: Username is walid.</para>
        /// <para>3: Password is valid.</para>
        /// </remarks>
        public bool[] VerificationResults {get; set;}

        /// <summary>
        /// Gets and sets the validation error messages.
        /// </summary>
        /// <remarks>Respective to VerificationResults</remarks>
        public string[] VerificationErrors {get; set;}
    }
}