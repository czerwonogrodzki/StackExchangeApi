
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace StackExchangeApi.Middleware
{
	public class GlobalExceptionHandlerMiddleware : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				ProblemDetails message = new ProblemDetails()
				{
					Status = 500,
					Type = "Internal Server Error",
					Detail = ex.Message
				};

				string jsonResponse = JsonSerializer.Serialize(message);

				context.Response.StatusCode = 500;
				context.Response.ContentType = "application/json";

				await context.Response.WriteAsync(jsonResponse);
			}
		}
	}
}
