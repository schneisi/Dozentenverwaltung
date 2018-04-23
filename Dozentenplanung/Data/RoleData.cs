using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dozentenplanung
{
    public static class RolesData
    {
        private static readonly string[] Roles = new string[] {
            "Administrator",
            "UserManager"
        };



        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //var theDatabaseContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                var theRoleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                foreach (var eachRole in Roles)
                {
                    if (!await theRoleManager.RoleExistsAsync(eachRole))
                    {
                        await theRoleManager.CreateAsync(new IdentityRole(eachRole));
                    }
                }

            }
        }
    }
}
