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
		private ISessionRepository _sessionRepository;

		public UserService(IUserRepository userRepository, ISessionRepository sessionRepository) {
			_userRepository = userRepository.ScreamIfNull("userRepository");
			_sessionRepository = sessionRepository.ScreamIfNull("sessionRepository");
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

		public IEnumerable<User> GetAll() {
			return _userRepository.GetAll();
		}

		public User SetUserProfile(int userId, string name, string username, string email, string password) {
			User user = Fetch(userId);
			if(user == null) throw new UserNotFoundException(userId);

			if(!string.IsNullOrEmpty(name) && name.Trim().ToLower() != user.Name.ToLower()) {
				//change name
				user.Name = name;
			}

			if(!string.IsNullOrEmpty(username) && username.Trim().ToLower() != user.Username.ToLower()) {
				if(_userRepository.UsernameExists(userId, username)) throw new UsernameExistsException();

				//change username
				user.Username = username;
			}

			if(!string.IsNullOrEmpty(email) && email.Trim().ToLower() != user.Email.ToLower()) {
				if(!email.IsVaildEmail()) throw new InvalidEmailException();
				if(_userRepository.EmailExists(userId, email)) throw new EmailExistsException();

				//change email
				user.Email = email;
			}

			if(!string.IsNullOrEmpty(password)) {

				//check if password is valid ...

				//change password
				user.Password = password.ToMD5Hash();
			}

			_userRepository.Save(user);
			_sessionRepository.UpdateSessionUser(user);

			return user;
		}
	}
}
