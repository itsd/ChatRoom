using Nostromo.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR.ChatServer.Queues.QueueItems {
	public class PostItem : IMongoQueueItem {
		public object ID { get; set; }
		public string Comment { get; set; }
		public string RoomToken { get; set; }
	}
}