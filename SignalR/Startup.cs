using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using SignalR.ChatServer.Context;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization;
using SignalR.ChatServer.Queues.QueueItems;
using SignalR.Models;

[assembly: OwinStartup(typeof(SignalR.Startup))]

namespace SignalR {
	public class Startup {
		public void Configuration(IAppBuilder app) {

			var config = new HubConfiguration { EnableJSONP = true };
			app.MapSignalR(config);

			BsonClassMap.RegisterClassMap<PostItem>(cm => {
				cm.AutoMap();
				cm.SetIdMember(cm.MapProperty(o => o.ID));
				cm.IdMemberMap.SetIdGenerator(new ObjectIdGenerator());
			});

			BsonClassMap.RegisterClassMap<Instance>(cm => {
				cm.AutoMap();
				cm.SetIdMember(cm.MapProperty(o => o.ID));
				cm.IdMemberMap.SetIdGenerator(new ObjectIdGenerator());
			});

			PostContext.Instance.StartListening();
		}
	}
}
