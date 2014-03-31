using ChatRoom.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.API.Models.Account {
	public class SessionModel {
		public int UserID { get; set; }
		public string Username { get; set; }
		public string Token { get; set; }

		public static implicit operator SessionModel(Session session) {
			return new SessionModel {
				UserID = session.UserID,
				Username = session.Username,
				Token = session.Token
			};
		}
	}
}