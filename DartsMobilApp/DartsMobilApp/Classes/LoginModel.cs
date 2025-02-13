using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.Classes
{
    public class LoginModel
    {
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }
    }

    public class LoginResponse
    {
        public string? id { get; set; }
        
        public string? accessToken { get; set; }

        public string? refreshToken { get; set; }
        public string? message { get; set; }

        public string? username { get; set; }

        public string? emailAddress { get; set; }
    }
}
