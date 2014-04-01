using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.Models.requests {
	public class ChatRoomTalkingRequest : ChatRoomRequestBase {
		public string Comment { get; set; }
		public string RoomToken { get; set; }
	}
}