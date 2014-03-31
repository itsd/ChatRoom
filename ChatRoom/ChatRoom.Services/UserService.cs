using ChatRoom.Domain;
using ChatRoom.Domain.Interfaces.Repositories;
using ChatRoom.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoom.Shared;
using ChatRoom.Domain.Exceptions;

namespace ChatRoom.Services {
	public class UserService : IUserService {

		private IUserRepository _userRepository;

		public UserService(IUserRepository userRepository) {
			_userRepository = userRepository.ScreamIfNull("userRepository");
		}

		public User Register(string username, string password) {
			if(_userRepository.UsernameExists(0, username)) { throw new UsernameExistsException(); }

			User user = new User {
				Username = username,
				Password = password.ToMD5Hash()
			};

			_userRepository.Save(user);

			return user;
		}

		public User Fetch(int id) {
			return _userRepository.Fetch(id);
		}
	}
}
