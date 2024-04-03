
using StackExchangeApi.Data;
using StackExchangeApi.Services;

namespace StackExchangeApi.Extensions
{
	public static class DataExtensions
	{
		public static async Task SeedDataAsync(this WebApplication app)
		{
			using (var services = app.Services.CreateScope())
			{
				var dataService = services.ServiceProvider.GetRequiredService<ITagsDataService>();

				var dbContext = services.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				var logger = services.ServiceProvider.GetRequiredService<ILogger<ITagsDataService>>();
				var configuration = services.ServiceProvider.GetRequiredService<IConfiguration>();

				await dataService.InitializeDatabaseAsync(dbContext, logger, configuration);
			}
		}
	}
}
