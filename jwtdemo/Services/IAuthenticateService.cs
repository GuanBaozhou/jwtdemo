using jwtdemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwtdemo.Services
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(LoginDto request, out string token);

        AccessToken DecodeToken(string accesstoken);


    }


}
