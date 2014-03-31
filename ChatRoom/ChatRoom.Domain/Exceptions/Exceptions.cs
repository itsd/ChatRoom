using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Domain.Exceptions {
	public class InvalidSessionTokenException : Exception { }
	public class LoginFailedException : Exception { }
	public class UsernameExistsException : Exception { }
}
