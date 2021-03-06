﻿using ChatRoom.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ChatRoom.Server.SignalR.Models.entities {
	public class SocketUser {
		public int ID { get; set; }
		public string Username { get; set; }
		public string Connection { get; set; }
		public string Token { get; set; }

		public static implicit operator SocketUser(Session session) {
			return new SocketUser {
				ID = session.UserID,
				Username = session.Username,
				Token = session.Token
			};
		}

		public static implicit operator SocketUser(User user) {
			return new SocketUser {
				ID = user.ID,
				Username = user.Username
			};
		}
	}
}