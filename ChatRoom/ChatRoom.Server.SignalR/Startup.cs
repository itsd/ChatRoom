using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(ChatRoom.Server.SignalR.Startup))]

namespace ChatRoom.Server.SignalR {
	public class Startup {
		public void Configuration(IAppBuilder app) {
			// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

			var config = new HubConfiguration { EnableJSONP = true };
			app.MapSignalR(config);
		}
	}
}
