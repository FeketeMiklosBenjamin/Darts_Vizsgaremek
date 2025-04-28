using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.Classes
{
    public class AccessTokenResponse
    {
        public string? accessToken { get; set; }
    }

    public class RefreshTokenModel
    {
        public string? refreshToken { get; set; }
    }
}
