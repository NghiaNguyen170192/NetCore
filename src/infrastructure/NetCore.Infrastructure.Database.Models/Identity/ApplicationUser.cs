using Microsoft.AspNetCore.Identity;
using System;

namespace NetCore.Infrastructure.Models.Identity
{
    public class ApplicationUser: IdentityUser
    {
        public ApplicationUser() : base() { }
        public ApplicationUser(string userName) : base(userName) { }      
    }
}
