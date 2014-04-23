using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using ChatRoom.Domain;
using Microsoft.AspNet.SignalR.Hubs;
using ChatRoom.Server.SignalR.Models.entities;

namespace ChatRoom.Server.SignalR.ChatServer {

	[HubName("chatRoom")]
	public class ChatRoomHub : Hub {

		private Messanger _messenger;

		public ChatRoomHub() {
			_messenger = new Messanger();
		}

		#region BaseMethods

		public override Task OnConnected() {
			this.SendOnlineUsers(Context.ConnectionId);
			this.CameOnline();

			return base.OnConnected();
		}

		public override Task OnDisconnected() {
			_messenger.WentOffline(Context.ConnectionId);
			this.WentOffline(Context.ConnectionId);

			return base.OnDisconnected();
		}

		public override Task OnReconnected() {
			return base.OnReconnected();
		}

		#endregion

		public void CameOnline() {
			string token = Context.QueryString["token"];
			var user = _messenger.GetUserByToken(token, Context.ConnectionId);

			Clients.AllExcept(Context.ConnectionId).getWhoCameOnline((SocketUser)user);
		}

		public void SendOnlineUsers(string connectionId) {
			string token = Context.QueryString["token"];
			var onlineUsers = _messenger.GetOnlineUsers(token);
			Clients.Caller.getOnlineUsers(onlineUsers);
		}

		public void WentOffline(string connectionId) {
			string token = Context.QueryString["token"];
			var user = _messenger.GetUserByConnection(Context.ConnectionId);
			_messenger.RemoveFromUsers(Context.ConnectionId);
			Clients.AllExcept(connectionId).wentOffline(user);
		}

		public string SendMessage(string message, IEnumerable<int> userIds) {
			string roomToken = Context.QueryString["groupToken"];
			var createdBy = _messenger.GetUserByID(userIds.First());

			if(string.IsNullOrEmpty(roomToken)) {
				var user1 = _messenger.GetUserByID(userIds.First());
				var user2 = _messenger.GetUserByID(userIds.Skip(1).First());
				roomToken = _messenger.CreateRoom(new List<string> { user1.Connection, user2.Connection });
				Groups.Add(user1.Connection, roomToken);
				Groups.Add(user2.Connection, roomToken);
			}

			Clients.OthersInGroup(roomToken).getMessage(roomToken, message, createdBy.Username);
			//Clients.Others.getMessage(roomToken, message, createdBy.Username, userIds);

			return roomToken;
		}
	}
}