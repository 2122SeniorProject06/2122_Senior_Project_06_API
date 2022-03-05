namespace  _2122_Senior_Project_06
{
    /// <summary>
    /// Class for User Account data type.
    /// </summary>
    /// <remarks> Made by Hugo Mazariego. Last update 12/09/2021. </remarks>
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
        public string ToSqlString()
        {
            return string.Format("'{0}', '{1}', '{2}', '{3}'",
                                 UserID,
                                 Sys_Security.Encoder(Username),
                                 Sys_Security.Encoder(Password), 
                                 Sys_Security.Encoder(Email));
        }
    }    
}