using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.Models.responses {
	public class ChatRoomWentOfflineResponse {
		public override ResponseType Type { get { return ResponseType.WentOffline; } }

		public int ID { get; set; }
		public string Username { get; set; }
	}
}