﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Infrastructure.Models
{
    public class ProfileViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ProfileViewModel()
        {

        }

        public ProfileViewModel(ApplicationUser user)
        {
            Id = user.Id;
            Email = user.Email;
        }

        public static IEnumerable<ProfileViewModel> GetUserProfiles(IEnumerable<ApplicationUser> users)
        {
            var profiles = new List<ProfileViewModel> { };
            foreach (ApplicationUser user in users)
            {
                profiles.Add(new ProfileViewModel(user));
            }

            return profiles;
        }
    }
}
