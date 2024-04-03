﻿using Microsoft.Extensions.Http.Logging;

namespace StackExchangeApi.Loggers
{
	public class HttpClientLogger : IHttpClientLogger
	{
		private readonly ILogger<HttpClientLogger> _logger;

		public HttpClientLogger(ILogger<HttpClientLogger> logger)
		{
			_logger = logger; ;
		}

		public object? LogRequestStart(HttpRequestMessage request)
		{
			_logger.LogInformation(
				"Sending '{request.Method}' to '{Request.Host}{Request.Path}'",
				request.Method,
				request.RequestUri?.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped),
				request.RequestUri!.PathAndQuery);

			return null;
		}

		public void LogRequestStop(object? context, HttpRequestMessage request, HttpResponseMessage response, TimeSpan elapsed)
		{

			_logger.LogInformation(
				"Received '{Response.StatusCodeInt} {Response.StatusCodeString}' after {Response.ElapsedMilliseconds}ms",
				(int)response.StatusCode,
				response.StatusCode,
				elapsed.TotalMilliseconds.ToString("F1"));
		}

		public void LogRequestFailed(object? context, HttpRequestMessage request, HttpResponseMessage? response, Exception exception, TimeSpan elapsed)
		{
			_logger.LogError(
				exception,
				"Request towards '{Request.Host}{Request.Path}' failed after {Response.ElapsedMilliseconds}ms",
				request.RequestUri?.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped),
				request.RequestUri!.PathAndQuery,
				elapsed.TotalMilliseconds.ToString("F1"));
		}
	}
}

