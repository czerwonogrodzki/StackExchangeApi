using Microsoft.EntityFrameworkCore;
using StackExchangeApi.Data;
using StackExchangeApi.Extensions;
using StackExchangeApi.Loggers;
using StackExchangeApi.Mapping;
using StackExchangeApi.Middleware;
using StackExchangeApi.Repositories;
using StackExchangeApi.Services;
using System.Net;
using System.Text.Json.Serialization;

namespace StackExchangeApi
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Logging.ClearProviders();
			builder.Logging.AddConsole();

			builder.Services.AddLogging();
			builder.Services.AddControllers().AddJsonOptions(config =>
			{
				config.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			});

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddAutoMapper(typeof(TagMappings));

			builder.Services.AddHttpClient("StackExchange")
				.RemoveAllLoggers()
				.AddLogger<HttpClientLogger>()
				.ConfigurePrimaryHttpMessageHandler(config => new HttpClientHandler()
				{
					AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
				});

			builder.Services.AddRouting(options =>
			{
				options.LowercaseUrls = true;
			});

			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddScoped<ITagsDataService, TagsDataService>();
			builder.Services.AddScoped<ITagsRepository, TagsRepository>();

			builder.Services.AddSingleton<GlobalExceptionHandlerMiddleware>();
			builder.Services.AddSingleton<HttpClientLogger>();

			builder.Services.AddHttpLogging(options =>
			{
				options.CombineLogs = true;
			});

			var app = builder.Build();

			await app.SeedDataAsync();

			app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseHttpLogging();

			app.UseAuthorization();

			app.MapControllers();


			app.Run();
		}
	}
}
