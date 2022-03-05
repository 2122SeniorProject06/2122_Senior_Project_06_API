using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace  _2122_Senior_Project_06.Models
{
    public class Journal
    {
        public string JournalID {get; private set;}
        public string Title {get; set;}
        public string Body {get; set;}
        public string UserID {get; private set;}
        public DateTime LastUpdated {get; set;}
    }
    // may need to transfer User and New Account functions to new model
}