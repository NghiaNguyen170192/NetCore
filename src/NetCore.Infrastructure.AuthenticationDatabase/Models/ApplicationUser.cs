﻿using Microsoft.AspNetCore.Identity;

namespace NetCore.Infrastructure.AuthenticationDatabase.Models
{
    public class ApplicationUser: IdentityUser
    {
        public ApplicationUser() : base() { }
        public ApplicationUser(string userName) : base(userName) { }      
    }
}
