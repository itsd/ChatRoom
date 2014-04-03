using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.SignalR.ChatServer {
	public class Messanger {
		private static readonly Lazy<Messanger> _instance = new Lazy<Messanger>(() => new Messanger());

		private static readonly Dictionary<string, IEnumerable<int>> _chatRooms = new Dictionary<string, IEnumerable<int>> { };
		private static readonly Dictionary<string, int> _users = new Dictionary<string, int> { };

		private Messanger() { }

		private static Messanger Instance { get { return _instance.Value; } }

		public string CreateRoom(IEnumerable<int> userIds) {
			var roomToken = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

			//((List<int>)userIds).Add(session.UserID);
			_chatRooms.Add(roomToken, userIds);

			return roomToken;
		}

		public void ComeOnline(string connectionId, int userId) {
			_users.Add(connectionId, userId);
		}

		public void WentOffline(string connectionId) {
			_users.Remove(connectionId);
		}

		public IEnumerable<string> GetConnectionIdsByID(IEnumerable<int> ids) {
			return from x in _users
				   where ids.Contains(x.Value)
				   select x.Key;
		}
	}
}