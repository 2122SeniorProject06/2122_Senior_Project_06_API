using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using _2122_Senior_Project_06;

namespace  _2122_Senior_Project_06.Models
{
    public class NewAccountModel
    {
        /// <summary>
        /// Gets and sets the new account username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets and sets the new account password.
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// Gets and sets the new account password verification. 
        /// </summary>
        public string confirmedPassword { get; set; }

        /// <summary>
        /// Gets and sets the new account email.
        /// </summary>
        /// <value></value>
        public string Email { get; set; }

        /// <summary>
        /// Gets and sets the results of the verification.
        /// </summary>
        /// <remarks>
        /// <para>It is a 4 element array of whether the user input is valid.</para>
        /// <para>0: Email is valid.</para>
        /// <para>1: Password inputs match.</para>
        /// <para>2: Username is valid.</para>
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