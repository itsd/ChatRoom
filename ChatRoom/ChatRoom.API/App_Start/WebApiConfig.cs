using ChatRoom.API.Infrastructure;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ChatRoom.API {
	public static class WebApiConfig {
		public static void Register(HttpConfiguration config) {
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();
			config.EnsureInitialized();

			var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
			jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			//Enable cross domain calls in web api from everywhere for all methods
			config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

			// Register handlers
			config.MessageHandlers.Add(new CorsMessageHandler(config));
			config.MessageHandlers.Add(new SessionHandler());
		}
	}
}
