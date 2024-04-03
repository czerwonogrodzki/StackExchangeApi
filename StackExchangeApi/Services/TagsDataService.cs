using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using StackExchangeApi.Data;
using StackExchangeApi.Models;
using StackExchangeApi.Repositories;
using System.Text.Json;

namespace StackExchangeApi.Services
{
	public class TagsDataService : ITagsDataService
	{
		private readonly ITagsRepository _tagsRepository;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;

		public TagsDataService(IHttpClientFactory httpClientFactory, ITagsRepository tagsRepository, IMapper mapper, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_tagsRepository = tagsRepository;
			_mapper = mapper;
			_configuration = configuration;
		}

		public async Task InitializeDatabaseAsync(ApplicationDbContext dbContext, ILogger logger, IConfiguration configuration)
		{
			try
			{
				if (!dbContext.Database.EnsureCreated())
				{
					return;
				}

				logger.LogWarning("Database not created; initializating and seeding with data");

				int tagsCount = await PopulateDatabaseAsync();

				logger.LogInformation($"Database successfully created and seeded with {tagsCount} tags");
			}
			catch (Exception ex)
			{
				logger.LogCritical($"Error, exiting application. \nException message: {ex.Message}");

				Environment.Exit(1);
			}
		}

		public async Task<int> PopulateDatabaseAsync()
		{
			var tags = await GetTagsAsync();

			_tagsRepository.AddRange(tags);

			return tags.Count();
		}

		public async Task<List<TagModel>> GetTagsAsync()
		{
			var httpClient = _httpClientFactory.CreateClient("StackExchange");

			int numberOfTags = _configuration.GetValue<int>("NumberOfTagsInDatabase");
			int stackExchangePageSize = _configuration.GetValue<int>("StackExchange:PageSize");
			string stackExchangeApiUrl = _configuration.GetValue<string>("StackExchange:ApiUrl");

			List<TagModel> tags = new List<TagModel>();

			for (int page = 1; page <= numberOfTags / stackExchangePageSize; page++)
			{
				Dictionary<string, string> queryParams = new Dictionary<string, string>()
				{
					{ "site", "stackoverflow" },
					{ "page", page.ToString() },
					{ "pagesize", stackExchangePageSize.ToString() }
				};

				HttpRequestMessage request = new HttpRequestMessage()
				{
					Method = HttpMethod.Get,
					RequestUri = new Uri(QueryHelpers.AddQueryString(stackExchangeApiUrl, queryParams))
				};

				var response = await httpClient.SendAsync(request);

				response.EnsureSuccessStatusCode();

				var content = await response.Content.ReadAsStringAsync();

				var results = JsonSerializer.Deserialize<StackExchangeResponseModel>(content);

				var mappedResults = _mapper.Map<IEnumerable<Tag>, IEnumerable<TagModel>>(results.Tags);

				tags.AddRange(mappedResults);
			}

			tags = CalculatePercentage(tags);

			return tags;
		}

		private List<TagModel> CalculatePercentage(List<TagModel> tags)
		{
			var sum = tags.Sum(x => x.Count);

			foreach (TagModel tag in tags)
			{
				tag.Percentage = Math.Round((decimal)tag.Count / sum * 100.0m, 5);
			}

			return tags;
		}
	}
}
