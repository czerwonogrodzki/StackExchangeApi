using StackExchangeApi.Data;
using StackExchangeApi.Models;

namespace StackExchangeApi.Services
{
	public interface ITagsDataService
	{
		Task InitializeDatabaseAsync(ApplicationDbContext dbContext, ILogger logger, IConfiguration configuration);
		Task<int> PopulateDatabaseAsync();
		Task<List<TagModel>> GetTagsAsync();
	}
}
