using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Domain.Interfaces.Services {
	public interface ISessionService {
		Session Login(string username, string password);
		Session ValidateToken(string token);
		Session FetchByToken(string token);
		void Logout(string token);
	}
}
