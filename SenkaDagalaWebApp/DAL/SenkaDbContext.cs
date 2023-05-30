using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SenkaDagalaWebApp.Models;

namespace SenkaDagalaWebApp.DAL;

public class SenkaDbContext:IdentityDbContext<AppUser>
{
	public SenkaDbContext(DbContextOptions<SenkaDbContext> options):base(options)
	{

	}
	public DbSet<Job> Jobs { get; set; }
	public DbSet<Service> Services { get; set; }
	public DbSet<Setting> Settings { get; set; }
}
