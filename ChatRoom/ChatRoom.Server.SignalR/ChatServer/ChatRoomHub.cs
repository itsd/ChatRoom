using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ChatRoom.Server.SignalR.ChatServer {
	public class ChatRoomHub : Hub {

		private readonly Messanger _messenger;

		#region BaseMethods

		public override System.Threading.Tasks.Task OnConnected() {
			//Get user id from qs
			int userId = 7889;
			_messenger.ComeOnline(Context.ConnectionId, userId);

			return base.OnConnected();
		}

		public override System.Threading.Tasks.Task OnDisconnected() {
			return base.OnDisconnected();
		}

		public override System.Threading.Tasks.Task OnReconnected() {
			_messenger.WentOffline(Context.ConnectionId);

			return base.OnReconnected();
		}

		#endregion

		public void JoinRoom(IEnumerable<int> userIds) {
			var room = _messenger.CreateRoom(userIds);
			var userConnections = _messenger.GetConnectionIdsByID(userIds);
			Groups.Add(Context.ConnectionId, room);
			foreach(var item in userConnections) {
				Groups.Add(item, room);
			}
		}

		public void PostInRoom(string roomName, string comment) {
			Clients.Group(roomName, Context.ConnectionId).sendRoomMessage(roomName, comment);
		}
	}
}