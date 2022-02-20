using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace  _2122_Senior_Project_06.Models
{
    
    public class JournalModel
    {
        public string _title {get; set;}

        public string _body {get; set;}
        
        public int _userId {get;}

        public int _id {get; private set;}

    }
}