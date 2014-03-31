using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.Models.responses {
	public class ChatRoomResponseBase {
		public virtual ResponseType Type { get { return ResponseType.OnlineUsers; } }
	}
}