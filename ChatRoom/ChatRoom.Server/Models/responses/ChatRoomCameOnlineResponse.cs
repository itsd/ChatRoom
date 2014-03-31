using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.Models.responses {
	public class ChatRoomCameOnlineResponse : ChatRoomResponseBase {
		public override ResponseType Type { get { return ResponseType.ComesOnline; } }
		public int ID { get; set; }
		public string Username { get; set; }
	}
}