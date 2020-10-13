using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwtdemo.Models
{
    public class AccessToken
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Company { get; set; }
        public string UserName { get; set; }
        public string AccountName { get; set; }
        public string UserId { get; set; }


    }
}
