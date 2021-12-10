using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace  _2122_Senior_Project_06
{
    /// <summary>
    /// The class that manages the login page.
    /// </summary>
    /// <remarks> Made by Hugo Mazariego. Last update 12/09/2021. </remarks>
    public class LoginPage : INotifyPropertyChanged
    {
        #region Private Members

        private string _username;
        private string _password;

        #endregion

        #region Events
        /// <summary>
        /// Updates elements using public properties.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets and sets the user's username choice.
        /// </summary>
        public string Username
        {
            get => _username;
             set
             {
                 _username = value;
                 NotifyPropertyChanged("Username");
             }
        }

        /// <summary>
        /// Gets and sets the user's password choice.
        /// </summary>
        public string Password
        {
            get => _password;
             set
             {
                 _password = value;
                 NotifyPropertyChanged("Password");
             }
        }

        /// <summary>
        /// Gets the Account Database Interface.
        /// </summary>
        public IAccountDatabase AccountDatabase {get; private set;}

        /// <summary>
        /// Gets the error message for if the login was unsucessful.
        /// </summary>
        public static string ErrorMessage {get => "Username or Password is incorrect!";}

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="accountDatabase">The account database interface to implement.</param>
        public LoginPage(IAccountDatabase accountDatabase)
        {
            AccountDatabase = accountDatabase;
        }

        /// <summary>
        /// Checks if provide password matches with provided username.
        /// </summary>
        /// <returns>If the passwords match.</returns>
        public bool ValidatePassword()
        {
            bool passwordValid = false;
            
            if(Sys_Security.VerifySQL(Password) && Sys_Security.VerifySQL(Username))
            {
                string hashedValue = Sys_Security.SHA256_Hash(Password);
                UserAccount userAccount = AccountDatabase.GetByUsername(Username);
                if(userAccount.Password == hashedValue)
                {
                    passwordValid = true;
                }
            }

            return passwordValid;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates public properties when updated.
        /// </summary>
        /// <param name="propertyName">The name of the property being changed.</param>
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #endregion
    }
}