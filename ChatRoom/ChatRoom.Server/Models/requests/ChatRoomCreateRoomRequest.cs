using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.Models.requests {
	public class ChatRoomCreateRoomRequest : ChatRoomRequestBase {
		public IEnumerable<int> UserIds { get; set; }
	}
}