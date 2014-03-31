using ChatRoom.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.Models.entities {
	public class SocketUser {
		public int ID { get; set; }
		public string Username { get; set; }

		public static implicit operator SocketUser(Session session) {
			return new SocketUser {
				ID = session.UserID,
				Username = session.Username
			};
		}
	}
}