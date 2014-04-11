using ChatRoom.Domain;
using ChatRoom.Domain.Exceptions;
using ChatRoom.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ChatRoom.API.Infrastructure {
	public class SessionHandler : DelegatingHandler {

		public const string AUTHENTICATION_TOKEN_HEADER = "Api-Auth-Token";

		protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken) {

			IEnumerable<string> headerValues;

			//try to get token from request header
			if(request.Headers.TryGetValues(AUTHENTICATION_TOKEN_HEADER, out headerValues)) {
				var token = headerValues.FirstOrDefault();

				if(!string.IsNullOrEmpty(token)) {
					//resolve security service dependency
					var service = DependencyResolver.Current.GetService<ISessionService>();

					//tolerate InvalidSessionTokenException. in this case
					//current thread will be treated as unauthenticated.
					try {
						service.ValidateToken(token);
						HttpContext.Current.User = Session.Current;
					} catch(InvalidSessionTokenException) { }
				}
			}

			return base.SendAsync(request, cancellationToken);
		}
	}
}