namespace api.raspberry.awards.Core.Exceptions
{
	using Microsoft.AspNetCore.Builder;

	/// <summary>
	/// 
	/// </summary>
	public static class HttpStatusCodeExceptionMiddlewareExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static IApplicationBuilder UseHttpStatusCodeExceptionMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<HttpStatusCodeExceptionMiddleware>();
		}
	}
}
