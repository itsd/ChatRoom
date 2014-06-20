using ChatRoom.Domain;
using ChatRoom.Domain.Interfaces.Repositories;
using ChatRoom.Entity.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Entity {
	public class UserRepository : EntityRepositoryBase, IUserRepository {
		public UserRepository(ChatRoomEntityContext context) : base(context) { }

		public User Fetch(string username, string password) {
			return _context.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
		}

		public bool UsernameExists(int id, string username) {
			return _context.Users.Any(u => u.ID != id && u.Username == username);
		}

		public bool EmailExists(int id, string email) {
			return _context.Users.Any(u => u.ID != id && u.Email == email);
		}

		public void Save(User user) {
			_context.Users.Add(user);
			_context.SaveChanges();
		}

		public User Fetch(int id) {
			return _context.Users.FirstOrDefault(x => x.ID == id);
		}

		public IEnumerable<User> GetAll() {
			return _context.Users;
		}

		public IEnumerable<User> GetFriends(int userId) {
			return _context.Users.Where(u => u.ID != userId);
		}
	}
}
