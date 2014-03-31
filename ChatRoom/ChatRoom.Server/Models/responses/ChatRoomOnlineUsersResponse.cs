using ChatRoom.Server.Models.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.Models.responses {
	public class ChatRoomOnlineUsersResponse : ChatRoomResponseBase {

		public override ResponseType Type { get { return ResponseType.OnlineUsers; } }

		public IEnumerable<SocketUser> Users { get; set; }
	}
}