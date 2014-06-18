using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nostromo.Common.Interfaces;

namespace MongoObserver {

	public class ChatItem : IMongoQueueItem {

		public object ID { get; set; }
		public string Message { get; set; }
		public int UserID { get; set; }
		public DateTime Created { get; set; }
		public string RoomName { get; set; }

		public override string ToString() {
			return "User " + UserID + " said '" + Message + "'";
		}
	}
}
