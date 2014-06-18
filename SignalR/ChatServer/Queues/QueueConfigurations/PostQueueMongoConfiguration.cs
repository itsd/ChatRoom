using Nostromo.Mongo;
using SignalR.ChatServer.Queues.QueueItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR.ChatServer.Queues.QueueConfigurations {
	public class PostQueueMongoConfiguration : MongoQueueConfiguration<PostItem> {

	}
}