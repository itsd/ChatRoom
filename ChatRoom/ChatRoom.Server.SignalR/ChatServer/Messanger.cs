using ChatRoom.Domain;
using ChatRoom.Domain.Interfaces.Services;
using ChatRoom.Entity;
using ChatRoom.Mongo;
using ChatRoom.Server.SignalR.Models.entities;
using ChatRoom.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.SignalR.ChatServer {
	public class Messanger {
		//private static readonly Lazy<Messanger> _instance = new Lazy<Messanger>(() => new Messanger());

		private static readonly Dictionary<string, IEnumerable<int>> _chatRooms = new Dictionary<string, IEnumerable<int>> { };
		private static readonly Dictionary<string, Session> _users = new Dictionary<string, Session> { };

		private ISessionService _sessionService;

		public Messanger() {
			IUnityContainer Resolver = new UnityContainer();

			Resolver.RegisterTypes(
				 AllClasses.FromAssemblies(
					 typeof(ISessionService).Assembly,
					 typeof(SessionService).Assembly,
					 typeof(UserRepository).Assembly,
					 typeof(SessionRepository).Assembly
				 ),
				 WithMappings.FromMatchingInterface,
				 WithName.Default,
				 x => new TransientLifetimeManager()
			 );

			_sessionService = Resolver.Resolve<ISessionService>();
		}

		//public static Messanger Instance {
		//	get {
		//		if(_instance == null) {
		//			_instance = new Messanger();
		//		}

		//		return _instance;

		//	}
		//}public static Messanger _instance;

		public string CreateRoom(IEnumerable<int> userIds) {
			var roomToken = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

			//((List<int>)userIds).Add(session.UserID);
			_chatRooms.Add(roomToken, userIds);

			return roomToken;
		}

		public Session GetUser(string token) {
			if(!_users.ContainsKey(token)) {
				var session = _sessionService.FetchByToken(token);
				_users.Add(token, session);
			}
			return _users[token];
		}

		public void ComeOnline(string connectionId, Session user) {
			if(!_users.ContainsKey(connectionId)) {
				_users.Add(connectionId, user);
			}
		}

		public void WentOffline(string connectionId) {
			_users.Remove(connectionId);
		}

		public IEnumerable<string> GetConnectionIdsByID(IEnumerable<int> ids) {
			//return from x in _users
			//	   where ids.Contains(x.Value)
			//	   select x.Key;
			return null;
		}

		public IEnumerable<SocketUser> GetOnlineUsers(string token) {
			return _users.Where(x => x.Key != token).Select(x => (SocketUser)x.Value);
		}
	}
}