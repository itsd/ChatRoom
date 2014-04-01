using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.Models.responses {
	public class ChatRoomTalkingResponse : ChatRoomResponseBase {
		public override ResponseType Type { get { return ResponseType.Talking; } }

		public int CreatedByID { get; set; }
		public string Comment { get; set; }
		public string RoomToken { get; set; }
	}
}