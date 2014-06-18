using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Nostromo.Mongo;
using SignalR.ChatServer.Queues.QueueConfigurations;
using SignalR.ChatServer.Queues.QueueItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR.ChatServer.Queues {
	public class PostQueue : MongoQueue<PostItem> {
		protected override IMongoQuery Criteria {
			get {
				return Query.Exists("RoomToken");
			}
		}
		public PostQueue(PostQueueMongoConfiguration config) : base(config) { }
	}
}