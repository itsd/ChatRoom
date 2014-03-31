using ChatRoom.Mongo.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Mongo {
	public class MongoRepositoryBase {

		protected ChatRoomMongoContext _context;

		public MongoRepositoryBase(ChatRoomMongoContext context) {
			if(context == null) throw new ArgumentNullException("context");
			_context = context;
		}
	}
}
