using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_DT102G.Data
{
	public static class Seeder
	{
		static StreamWriter file = new("logger.txt");
		public static void SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			file.WriteLine("In SeedData");
			file.Flush();
			SeedRoles(roleManager);
			SeedUsers(userManager);

			file.Close();
		}


		private static void SeedUsers(UserManager<IdentityUser> userManager)
		{
			if (userManager.FindByNameAsync("admin@telia.com").Result == null)
			{
				file.WriteLine("In SeedUsers add admin@telia.com");
				IdentityUser user = new IdentityUser();
				user.UserName = "admin@telia.com";
				user.NormalizedUserName = "admin@telia.com";
				user.Email = "admin@telia.com";
				user.NormalizedEmail = "admin@telia.com";
				user.EmailConfirmed = true;

				IdentityResult result = userManager.CreateAsync(user, "Pissen30060!").Result;

				if (result.Succeeded)
				{
					userManager.AddToRoleAsync(user, "Admin").Wait();
				}
			}
			else
			{
				file.WriteLine("In SeedUsers admin@telia.com exist already");
			}
		}

		private static void SeedRoles(RoleManager<IdentityRole> roleManager)
		{
			if (!roleManager.RoleExistsAsync("Admin").Result)
			{
				file.WriteLine("In SeedRoles add admin");
				IdentityRole role = new IdentityRole();
				role.Name = "Admin";
				IdentityResult roleResult = roleManager.
				CreateAsync(role).Result;
			}
			else
			{
				file.WriteLine("In SeedRoles admin exist");
			}
		}
	}
}
