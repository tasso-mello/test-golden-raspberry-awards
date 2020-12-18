namespace api.raspberry.awards.Core.Exceptions
{
    using common.raspberry.awards.Utilities;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

	/// <summary>
	/// 
	/// </summary>
    public class HttpStatusCodeExceptionMiddleware
	{
		private readonly ILogger<HttpStatusCodeExceptionMiddleware> _logger;
		private readonly RequestDelegate _next;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="next"></param>
		/// <param name="loggerFactory"></param>
		public HttpStatusCodeExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
		{
			_next = next ?? throw new ArgumentNullException(nameof(next));
			_logger = loggerFactory?.CreateLogger<HttpStatusCodeExceptionMiddleware>() ??
					  throw new ArgumentNullException(nameof(loggerFactory));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (InvalidOperationException ex)
			{
				if (context.Response.HasStarted)
				{
					_logger.LogWarning(
						"The response has already started, the http status code middleware will not be executed.");
					throw;
				}

				context.Response.Clear();
				context.Response.StatusCode = 500;
				context.Response.ContentType = "application/json";

				await context.Response.WriteAsync(Messages.GenerateGenericErrorMessage(ex.Message).ToString());
			}
			catch (HttpStatusCodeException ex)
			{
				if (context.Response.HasStarted)
				{
					_logger.LogWarning(
						"The response has already started, the http status code middleware will not be executed.");
					throw;
				}

				context.Response.Clear();
				context.Response.StatusCode = ex.StatusCode;
				context.Response.ContentType = ex.ContentType;

				await context.Response.WriteAsync(Messages.GenerateGenericErrorMessage(ex.Message).ToString());
			}
		}
	}
}
