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

		private static readonly ConnectionMapping<string> _tokens = new ConnectionMapping<string> { };
		private static readonly UserMapping<string, SocketUser> _users = new UserMapping<string, SocketUser> { };
		private static readonly RoomMapping<string> _rooms = new RoomMapping<string> { };

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

		public SocketUser GetUserByToken(string token) {
			SocketUser user = _users.Get(token);
			if(user == default(SocketUser)) { user = _sessionService.FetchByToken(token); }
			return user;
		}

		public void SaveUser(SocketUser user, string connection) {
			_tokens.Add(user.Token, connection);
			_users.Add(user.Token, user);
		}

		public bool RemoveUser(SocketUser user, string connection) {
			_tokens.Remove(user.Token, connection);
			if(_tokens.CountForKey(user.Token) == 0) {
				_users.RemoveByKey(user.Token);
				return true;
			}
			return false;
		}

		public IEnumerable<string> GetConnectionsForUser(string token) {
			return _tokens.GetAllByKey(token);
		}

		public IEnumerable<SocketUser> GetOnlineUsers(string token) {
			var onlineTokens = _tokens.GetAllKeys();

			return from u in _users.ValuesList
				   where u.Token != token && onlineTokens.Contains(u.Token)
				   select u;
		}

		public string CreateRoom(IEnumerable<int> userIds) {
			string newRoomId = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
			foreach(var userId in userIds) { _rooms.Add(newRoomId, userId); }
			return newRoomId;
		}

		public string GetRoomByUserIds(IEnumerable<int> userIds) {
			return _rooms.GetByValues(userIds);
		}

		public IEnumerable<string> GetConnectionsForRoom(string roomId) { //Want to get all users' all connections in this room

			var userIds = _rooms.GetAllByKey(roomId);

			var userTokens = from x in _users.ValuesList
							 where userIds.Contains(x.ID)
							 select x.Token;

			return _tokens.GetAllByKeys(userTokens);
		}

		public IEnumerable<string> GetUserRoomIds(int userId) {
			return _rooms.GetAllByContainingValue(userId);
		}

		public IEnumerable<string> GetUsersAllConnections(string token) {
			return _tokens.GetAllByKey(token);
		}
	}
}