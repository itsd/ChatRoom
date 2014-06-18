using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nostromo.Mongo;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace MongoObserver {
	public class ChatQueue : MongoQueue<ChatItem> {
		protected override MongoDB.Driver.IMongoQuery Criteria {
			get {
				return Query.Exists("UserID");
			}
		}
		public ChatQueue(ChatQueueMongoConfiguration config) : base(config) {

		}

	}
}
