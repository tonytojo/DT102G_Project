using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projekt_DT102G.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_DT102G.Data
{
	public class ApplicationDbContext : IdentityDbContext<IdentityUser,IdentityRole,string>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		//Modeller som blir tabeller i databasen
		public DbSet<Book> Books { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderLine> OrderLines { get; set; }
		public DbSet<Genre> Genre { get; set; }
	}
}
