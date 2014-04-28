using ChatRoom.Domain;
using ChatRoom.Domain.Interfaces.Services;
using ChatRoom.Entity;
using ChatRoom.Mongo;
using ChatRoom.Server.SignalR.Infrastructure;
using ChatRoom.Server.SignalR.Models.entities;
using ChatRoom.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.SignalR.ChatServer {
	public class Messanger {
		private static readonly Dictionary<string, IEnumerable<string>> _chatRooms = new Dictionary<string, IEnumerable<string>> { };
		private readonly static IDictionary<int, string> _connections = new Dictionary<int, string>();
		private readonly static IDictionary<string, SocketUser> _tokens = new Dictionary<string, SocketUser>();

		public int OnlineUsers { get; set; }

		private ISessionService _sessionService;
		private IUserService _userService;

		public static Messanger Instance {
			get {
				return _instance ?? (_instance = new Messanger());
			}
		}private static Messanger _instance;

		private Messanger() {
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
			_userService = Resolver.Resolve<IUserService>();
		}

		//public Messanger() {

		//}

		public string CreateRoom(IEnumerable<string> userConnections) {
			var roomToken = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
			_chatRooms.Add(roomToken, new List<string> { });
			foreach(var item in userConnections) {
				((List<string>)_chatRooms[roomToken]).Add(item);
			}
			return roomToken;
		}

		public SocketUser GetUserByToken(string token) {
			SocketUser User = _tokens.FirstOrDefault(x => x.Key == token).Value;
			if(User == null) {
				User = _sessionService.FetchByToken(token);
			}

			return User;
		}

		public void SaveUser(SocketUser user, string connection) {
			_tokens.Add(user.Token, user);
			_connections.Add(user.ID, connection);
		}

		public void RemoveUser(SocketUser user, string connection) {
			_tokens.Remove(user.Token);
			_connections.Remove(user.ID);
		}

		public string GetConnection(int userId) {
			return _connections[userId];
		}

		public IEnumerable<string> GetConnectionIdsByID(IEnumerable<int> ids) {
			//return from x in _users
			//	   where ids.Contains(x.Value)
			//	   select x.Key;
			return null;
		}

		public IEnumerable<SocketUser> GetOnlineUsers(string token) {
			return _tokens.Where(x => x.Key != token).Select(x => x.Value);
		}

		public IEnumerable<SocketUser> GetAllUsers(string token) {
			var user = GetUserByToken(token);
			return _userService.GetAll().Where(x => x.ID != user.ID).Select(y => (SocketUser)y);
		}
	}
}