namespace  _2122_Senior_Project_06.Types
{
    /// <summary>
    /// Class for User Account data type.
    /// </summary>
    /// <remarks> Paired programmed by Hugo, Andrew, and Sarah. </remarks>
    public class UserAccount
    {
        public string UserID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public UserAccount()
        {

        }

        /// <summary>
        /// Parameterized constructor for new account.
        /// </summary>
        /// <param name="username">The chosen username.</param>
        /// <param name="email">The chosen email.</param>
        /// <param name="password">The chosen password. Hash before sending.</param>
        public UserAccount(string username, string email, string password)
        {
            Username = username;
            Password = password;
            Email = email;
            UserID = Sys_Security.GenID(UserID,true);
        }

        /// <summary>
        /// Returns values in a SQL value format.
        /// </summary>
        /// <returns>All values formatted into SQL.</returns>
        public string ToSqlString(bool isUpdate)
        {
            string[] values = { UserID,
                                Sys_Security.Encoder(Username),
                                Sys_Security.Encoder(Password), 
                                Sys_Security.Encoder(Email)};

            if(isUpdate)
            {
                values[0] =  string.Format("{0} = '{1}'", UserAccountsItems.User_ID, values[0]);
                values[1] =  string.Format("{0} = '{1}'", UserAccountsItems.Username, values[1]);
                values[2] =  string.Format("{0} = '{1}'", UserAccountsItems.Password, values[2]);
                values[3] =  string.Format("{0} = '{1}'", UserAccountsItems.Email, values[3]);
            }
            else
            {
                for(int i = 0; i < values.Length; i++)
                {
                    values[i] =  string.Format("'{0}'", values[i]);
                }
            }
            return string.Format("{0}, {1}, {2}, {3}",
                                 values[0],
                                 values[1],
                                 values[2],
                                 values[3]);
        }

        /// <summary>
        /// Updates the data of the user account.
        /// </summary>
        /// <param name="newInfo">The updated information.</param>
        public void UpdateInfo(UserAccount newInfo)
        {
            Username = newInfo.Username ?? Username;
            Password = newInfo.Password ?? Password;
            Email = newInfo.Email ?? Email;
        }
    }    
}