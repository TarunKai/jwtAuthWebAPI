using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace jwtAuthWebAPI
{
	public class CustomAuthFilter : AuthorizeAttribute, IAuthenticationFilter
	{

		public bool AllowMultiple { get { return false; } }
		public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
		{
			string authParameter = string.Empty;
			HttpRequestMessage message = context.Request;
			AuthenticationHeaderValue authorization = message.Headers.Authorization;
			if (authorization==null)
			{
				context.ErrorResult = new AuthenticationFailurResult("Missing Authorization Header", message);
				return;
			}

			if (authorization.Scheme!="Bearer")
			{
				context.ErrorResult = new AuthenticationFailurResult("Invalid Authorization schema", message);
				return;
			}

			if (string.IsNullOrEmpty(authorization.Parameter))
			{
				context.ErrorResult = new AuthenticationFailurResult("Missing Token", message);
				return;
			}

			context.Principal = TokenManager.GetClaimsPrincipal(authorization.Parameter);
		}

		public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
		{
			var result = await context.Result.ExecuteAsync(cancellationToken);
			if (result.StatusCode==HttpStatusCode.Unauthorized)
			{
				result.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic", "reals=localhost"));
				context.Result = new ResponseMessageResult(result);
			}
		}
	}

	public class AuthenticationFailurResult : IHttpActionResult
	{
		public string ReasonPhrase;
		public HttpRequestMessage request { get; set; }

		public AuthenticationFailurResult(string reasonPhrase,HttpRequestMessage requestMessage )
		{
			ReasonPhrase = reasonPhrase;
			request = requestMessage;
		}
		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult(Execute());
		}

		public HttpResponseMessage Execute()
		{
			HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
			responseMessage.RequestMessage = request;
			responseMessage.ReasonPhrase = ReasonPhrase;

			return responseMessage;

		}
	}
}