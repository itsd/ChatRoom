using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Domain.Interfaces.Repositories {
	public interface ISessionRepository {
		void Save(Session session);
		void Delete(string token);
		Session Fetch(string token);
		Session Fetch(int userId);
		void UpdateLastAccess(string token);
	}
}
