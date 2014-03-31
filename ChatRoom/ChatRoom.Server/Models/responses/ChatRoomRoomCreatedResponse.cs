using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.Models.responses {
	public class ChatRoomRoomCreatedResponse : ChatRoomResponseBase {
		public override ResponseType Type { get { return ResponseType.RoomCreated; } }

		public string RoomID { get; set; }
	}
}