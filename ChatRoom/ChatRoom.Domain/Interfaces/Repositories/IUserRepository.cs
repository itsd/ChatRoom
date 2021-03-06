﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Domain.Interfaces.Repositories {
	public interface IUserRepository {

		User Fetch(int id);

		User Fetch(string username, string password);

		void Save(User user);

		bool UsernameExists(int id, string username);

		bool EmailExists(int id, string email);

		IEnumerable<User> GetAll();

		IEnumerable<User> GetFriends(int userId);
	}
}
