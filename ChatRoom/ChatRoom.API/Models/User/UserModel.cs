﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.API.Models.User {
	public class UserModel {
		public int ID { get; set; }
		public string Name { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }

		public static implicit operator UserModel(Domain.User user) {
			return new UserModel {
				ID = user.ID,
				Name = user.Name,
				Email = user.Email,
				Username = user.Username
			};
		}
	}
}