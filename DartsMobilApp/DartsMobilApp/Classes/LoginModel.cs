using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.Classes
{
    public class LoginModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
