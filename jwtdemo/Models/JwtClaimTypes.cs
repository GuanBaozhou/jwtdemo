using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace jwtdemo.Models
{
    public class JwtClaimTypes
    {
        public static string Name { get; set; } = ClaimTypes.Name;
        public static string Role { get; set; } = ClaimTypes.Role;
        public static string Company { get; set; } = ClaimTypes.GroupSid;
        public static string UserName { get; set; } = ClaimTypes.Actor;
        public static string AccountName { get; set; } = ClaimTypes.Surname;
        public static string UserId { get; set; } = ClaimTypes.NameIdentifier;
    }
}
