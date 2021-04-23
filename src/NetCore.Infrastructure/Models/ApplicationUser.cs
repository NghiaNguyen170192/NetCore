using Microsoft.AspNetCore.Identity;
using System;

namespace NetCore.Infrastructure.Models
{
    public class ApplicationUser: IdentityUser
    {
        public ApplicationUser() : base() { }
        public ApplicationUser(string userName) : base(userName) { }      
    }
}
