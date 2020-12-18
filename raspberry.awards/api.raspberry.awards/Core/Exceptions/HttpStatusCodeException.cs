namespace api.raspberry.awards.Core.Exceptions
{
	using Newtonsoft.Json.Linq;
	using System;
	using System.Runtime.Serialization;
	using System.Security.Permissions;

	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class HttpStatusCodeException : Exception
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="statusCode"></param>
		public HttpStatusCodeException(int statusCode)
		{
			StatusCode = statusCode;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="message"></param>
		public HttpStatusCodeException(int statusCode, string message) : base(message)
		{
			StatusCode = statusCode;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="inner"></param>
		public HttpStatusCodeException(int statusCode, Exception inner) : this(statusCode, inner.ToString())
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="errorObject"></param>
		public HttpStatusCodeException(int statusCode, JObject errorObject) : this(statusCode, errorObject.ToString())
		{
			ContentType = @"application/json";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected HttpStatusCodeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			ResourceReferenceProperty = info.GetString("ResourceReferenceProperty");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException("info");

			info.AddValue("ResourceReferenceProperty", ResourceReferenceProperty);

			base.GetObjectData(info, context);
		}

		/// <summary>
		/// 
		/// </summary>
		public string ResourceReferenceProperty { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int StatusCode { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string ContentType { get; set; } = @"text/plain";
	}
}
