using Microsoft.EntityFrameworkCore;
using StackExchangeApi.Models;

namespace StackExchangeApi.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<TagModel> Tags { get; set; }
	}
}
