using ChatRoom.Domain;
using ChatRoom.Domain.Exceptions;
using ChatRoom.Domain.Interfaces.Repositories;
using ChatRoom.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatRoom.Shared;

namespace ChatRoom.Services {
	public class SessionService : ISessionService {

		private ISessionRepository _sessionRepository;
		private IUserRepository _userRepository;

		public SessionService(ISessionRepository sessionRepository, IUserRepository userRepository) {
			_sessionRepository = sessionRepository.ScreamIfNull("sessionRepository");
			_userRepository = userRepository.ScreamIfNull("sessionRepository");
		}

		public Session Login(string username, string password) {
			password = password.ToMD5Hash();
			var user = _userRepository.Fetch(username.ToLower(), password);
			if(user == null) throw new LoginFailedException();

			Session session = (Session)user;
			_sessionRepository.Save(session);
			Thread.CurrentPrincipal = session;
			return session;
		}

		public Session ValidateToken(string token) {
			var session = _sessionRepository.Fetch(token);
			if(session == null) throw new InvalidSessionTokenException();

			_sessionRepository.UpdateLastAccess(token);
			Thread.CurrentPrincipal = session;
			return session;
		}

		public Session FetchByToken(string token) {
			var session = _sessionRepository.Fetch(token);
			if(session == null) throw new InvalidSessionTokenException();

			_sessionRepository.UpdateLastAccess(token);
			return session;
		}

		public void Logout(string token) {
			_sessionRepository.Delete(token);
		}
	}
}