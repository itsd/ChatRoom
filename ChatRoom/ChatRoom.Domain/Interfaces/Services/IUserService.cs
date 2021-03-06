﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Domain.Interfaces.Services {
	public interface IUserService {
		User Fetch(int id);
		User Register(string username, string password);
		User SetUserProfile(int userId, string name, string username, string email, string password);
		IEnumerable<User> GetAll();
		IEnumerable<User> GetFriends(int userId);
	}
}
