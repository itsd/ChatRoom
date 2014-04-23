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

		private static readonly Dictionary<string, IEnumerable<string>> _chatRooms = new Dictionary<string, IEnumerable<string>> { };
		private static readonly Dictionary<string, SocketUser> _users = new Dictionary<string, SocketUser> { };

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

		public string CreateRoom(IEnumerable<string> userConnections) {
			var roomToken = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
			_chatRooms.Add(roomToken, new List<string> { });
			foreach(var item in userConnections) {
				((List<string>)_chatRooms[roomToken]).Add(item);
			}
			return roomToken;
		}

		public SocketUser GetUserByID(int id) {
			return _users.FirstOrDefault(x => x.Value.ID == id).Value;
		}

		public SocketUser GetUserByToken(string token, string connection) {
			if(!_users.ContainsKey(token)) {
				var session = _sessionService.FetchByToken(token);
				SocketUser user = session;
				user.Connection = connection;
				_users.Add(token, user);
			}
			return _users[token];
		}

		public SocketUser GetUserByConnection(string connection) {
			return _users.FirstOrDefault(x => x.Value.Connection == connection).Value;
		}

		public void RemoveFromUsers(string connection) {
			var user = _users.FirstOrDefault(x => x.Value.Connection == connection);
			_users.Remove(user.Key);
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