using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DartsMobilApp.Database
{
    public class UserLoginData
    { 

            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            [NotNull, Unique]
            public string Email { get; set; }

            [NotNull] 
            public string Password { get;set; }

        public UserLoginData(string email, string password) 
        {
           Email = email;
           Password = password;
        }
        public UserLoginData()
        {
            
        }
    }
}
