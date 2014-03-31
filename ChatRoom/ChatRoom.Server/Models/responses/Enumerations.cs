using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.Models.responses {
	public enum ResponseType {
		Default = 0,
		OnlineUsers = 1,
		ComesOnline = 2,
		WentOffline = 3
	}
}