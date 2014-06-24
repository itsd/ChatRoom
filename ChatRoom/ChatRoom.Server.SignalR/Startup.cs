using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using ChatRoom.Server.SignalR.Infrastructure;

[assembly: OwinStartup(typeof(ChatRoom.Server.SignalR.Startup))]

namespace ChatRoom.Server.SignalR {
	public class Startup {
		public void Configuration(IAppBuilder app) {
			// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

			var config = new HubConfiguration { EnableJSONP = true };
			app.MapSignalR(config);

			//Camel Case Property Names Contract Resolver
			var settings = new JsonSerializerSettings();
			settings.ContractResolver = new SignalRContractResolver();
			var serializer = JsonSerializer.Create(settings);
			GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

			//Authentication
			//GlobalHost.HubPipeline.RequireAuthentication(); 
		}
	}
}
