using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Domain.Interfaces.Services {
	public interface IUserService {
		User Fetch(int id);
		User Register(string username, string password);
	}
}
