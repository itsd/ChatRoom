using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Domain.Exceptions {
	public class InvalidSessionTokenException : Exception { }
	public class LoginFailedException : Exception { }
	public class UsernameExistsException : Exception { }
	public class EmailExistsException : Exception { }
	public class InvalidEmailException : Exception { }
	public class UserNotFoundException : Exception {
		public UserNotFoundException() : base("User not found") { }
		public UserNotFoundException(int userId) : base("User not found with ID: " + userId) { }
	}
}
