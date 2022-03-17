using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace  _2122_Senior_Project_06.Models
{
    //model could be used for user's account page
    public class UserModel
    {
        public string userID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Badges { get; set; }
    }
}