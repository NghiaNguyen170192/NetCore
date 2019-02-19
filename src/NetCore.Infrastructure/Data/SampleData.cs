using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Infrastructure.Migrations.ApplicationDb;
using NetCore.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Data
{
    public static class SampleData
    {
        private static async Task AddUserRole(IApplicationBuilder app, string userName, string roleName)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string defaultPassword = "Qwert@123";
                ApplicationUser user = new ApplicationUser
                {
                    UserName = userName,
                    NormalizedUserName = userName,
                    Email = userName,
                    NormalizedEmail = userName,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityRole identityRole = new IdentityRole { Name = roleName, NormalizedName = roleName };
                if (roleManager.FindByNameAsync(roleName).Result == null)
                {
                    await roleManager.CreateAsync(identityRole);
                }

                if (userManager.FindByNameAsync(userName).Result == null)
                {
                    IdentityResult result = userManager.CreateAsync(user, defaultPassword).Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, identityRole.Name).Wait();
                    }
                }
            }
        }

        public static async void SeedUsersAndRoles(IApplicationBuilder app)
        {
            await AddUserRole(app,"admin@nnqt.com", "admin");
            await AddUserRole(app, "manager@nnqt.com", "manager");
            await AddUserRole(app, "user@nnqt.com", "user");
        }

        public static void Migration(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            using (var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>())
            using (var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
            using (var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                persistedGrantDbContext.Database.Migrate();
                configurationDbContext.Database.Migrate();
                applicationDbContext.Database.Migrate();
            }
        }

        //public static async void SeedSampleData(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        //using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
        //{
        //    if (context.TitleBasics != null && !context.TitleBasics.Any())
        //    {
        //        var filePath = $@"{env.ContentRootPath}\AppData\NetCoreTrainingSeedingData.sql";
        //        var lines = File.ReadLines(filePath);
        //        var commands = ParseCommand(lines);

        //        foreach (var command in commands)
        //        {
        //            await context.Database.ExecuteSqlCommandAsync(command);
        //        }
        //    }
        //}
        //}

        public class Users
        {
            public static List<TestUser> Get()
            {
                return new List<TestUser> {
                    new TestUser {
                        SubjectId = Guid.NewGuid().ToString(),
                        Username = "manager",
                        Password ="Password123!",
                        Claims = new List<Claim> {
                            new Claim(JwtClaimTypes.Role, "manager"),
                        }
                    },

                    new TestUser {
                        SubjectId = Guid.NewGuid().ToString(),
                        Username = "admin",
                        Password = "Password123!",
                        Claims = new List<Claim> {
                            new Claim(JwtClaimTypes.Role, "admin"),
                        }
                    },

                    new TestUser {
                        SubjectId = Guid.NewGuid().ToString(),
                        Username = "user",
                        Password = "Password123!",
                        Claims = new List<Claim> {
                            new Claim(JwtClaimTypes.Role, "user"),
                        }
                    }
                };
            }
        }

        public static async void InitializeDbData(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            using (var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>())
            using (var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>())
            {
                if (userManager.Users.Any() && roleManager.Roles.Any())
                {
                    return;
                }

                foreach (var testUser in Users.Get())
                {
                    var identityUser = new ApplicationUser(testUser.Username)
                    {
                        Id = testUser.SubjectId,
                    };

                    var result = await userManager.CreateAsync(identityUser, testUser.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddClaimsAsync(identityUser, testUser.Claims.ToList());
                        var claimRole = testUser.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Role);
                        if (claimRole != null)
                        {
                            var identityRole = new IdentityRole(claimRole.Value);
                            await roleManager.CreateAsync(identityRole);
                            await userManager.AddToRoleAsync(identityUser, identityRole.Name);
                        }
                    }
                }
            }
        }

        private static IEnumerable<string> ParseCommand(IEnumerable<string> lines)
        {
            var sb = new StringBuilder();
            var commands = new List<string>();
            foreach (var line in lines)
            {
                if (string.Equals(line, "GO", StringComparison.OrdinalIgnoreCase))
                {
                    if (sb.Length > 0)
                    {
                        commands.Add(sb.ToString());
                        sb = new StringBuilder();
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        sb.Append(line);
                    }
                }
            }

            return commands;
        }
    }
}
