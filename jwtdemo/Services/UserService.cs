using jwtdemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwtdemo.Services
{
    public interface IUserService
    {
        bool IsValid(LoginDto req);
    }





    public class UserService : IUserService
    {
        public bool IsValid(LoginDto req)
        {
            return true;
        }
    }
}
