using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.Models.requests {
	public enum RequestType {
		Default = 0,
		CreateRoom = 1,
		PostInRoom = 2
	}
}