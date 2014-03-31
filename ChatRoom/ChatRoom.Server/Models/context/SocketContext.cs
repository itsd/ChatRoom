using ChatRoom.Domain;
using ChatRoom.Server.Models.collection;
using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatRoom.Server.Models.Infrastructure;
using ChatRoom.Domain.Interfaces.Services;
using ChatRoom.Services;
using Microsoft.Practices.Unity;
using ChatRoom.Entity;
using ChatRoom.Mongo;
using ChatRoom.Mongo.Context;
using ChatRoom.Entity.Context;
using ChatRoom.Server.Models.responses;
using ChatRoom.Server.Models.entities;

namespace ChatRoom.Server.Models.context {
	public class SocketContext {
		private static Dictionary<string, Session> _users = new Dictionary<string, Session> { };
		private static Dictionary<string, ChatRoomSocketCollection> _socketRooms = new Dictionary<string, ChatRoomSocketCollection> { };
		private static Dictionary<string, IEnumerable<int>> _chatRooms = new Dictionary<string, IEnumerable<int>> { };

		private ISessionService _sessionService;

		public static SocketContext Current {
			get {
				if(_context == null) _context = new SocketContext();
				return _context;
			}
		} private static SocketContext _context;

		private SocketContext() {

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

			//Resolver.RegisterType(
			//	typeof(ChatRoomEntityContext),
			//	new PerRequestLifetimeManager()
			//);

			//Resolver.RegisterType(
			//	typeof(ChatRoomMongoContext),
			//	new PerRequestLifetimeManager()
			//);


			_sessionService = Resolver.Resolve<ISessionService>();
		}

		private Session GetSession(WebSocketHandler socket) {
			return (socket as ChatRoomWebSocket).Session;
		}

		public Session GetUser(string token) {
			string key = string.Format(Utilites.UserPrivateRoomKey, token);

			if(!_users.ContainsKey(key)) {
				var session = _sessionService.FetchByToken(token);
				_users.Add(key, session);
			}
			return _users[key];
		}

		public void AddToRoom(WebSocketHandler socket, string roomKey) {
			if(!_socketRooms.ContainsKey(roomKey)) {
				_socketRooms.Add(roomKey, new ChatRoomSocketCollection { });
			}
			_socketRooms[roomKey].Add(socket);
		}

		public void AddToPrivateRoom(WebSocketHandler socket) {
			var session = GetSession(socket);
			AddToRoom(socket, session.GetSessionPrivateKey());
		}

		public void WentOffline(WebSocketHandler socket) {
			var session = GetSession(socket);
			_socketRooms.Remove(session.GetSessionPrivateKey());
			_users.Remove(session.GetSessionPrivateKey());
		}

		public void SendOnlineFriendsToUser(WebSocketHandler socket) {
			var session = GetSession(socket);
			if(_users.Count > 0) {
				_socketRooms[session.GetSessionPrivateKey()].SendOnlineUsers(new ChatRoomOnlineUsersResponse {
					Users = _users.Where(x => x.Key != session.GetSessionPrivateKey()).Select(x => (SocketUser)x.Value)
				});
			}
		}

		public void SendComeToOnlineNotificationToUsers(WebSocketHandler socket) {
			var session = GetSession(socket);
			foreach(var item in _socketRooms.Where(x => x.Key != session.GetSessionPrivateKey())) {
				item.Value.SendComeOnline(new ChatRoomCameOnlineResponse {
					ID = session.UserID,
					Username = session.Username
				});
			}
		}

		public void SendWentOfflineNotificationToUsers(WebSocketHandler socket) {
			var session = GetSession(socket);
			foreach(var item in _socketRooms.Where(x => x.Key != session.GetSessionPrivateKey())) {
				item.Value.SendWentOffline(new ChatRoomWentOfflineResponse {
					ID = session.UserID,
					Username = session.Username
				});
			}
		}

		public void SendRoomCreatedNotification(WebSocketHandler socket, string roomId) {
			var session = GetSession(socket);
			foreach(var item in _socketRooms.Where(x => x.Key == session.GetSessionPrivateKey())) {
				item.Value.SendRoomCreated(new ChatRoomRoomCreatedResponse {
					RoomID = roomId
				});
			}
		}

		public string CreateChatRoomWithUsers(WebSocketHandler socket, IEnumerable<int> userIds) {
			var session = GetSession(socket);
			var roomId = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
			((List<int>)userIds).Add(session.UserID);
			_chatRooms.Add(roomId, userIds);
			return roomId;
		}
	}
}