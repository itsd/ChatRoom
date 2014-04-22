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
			var user = _messenger.GetUser(token);

			Clients.AllExcept(Context.ConnectionId).getWhoCameOnline((SocketUser)user);
		}

		public void SendOnlineUsers(string connectionId) {
			string token = Context.QueryString["token"];
			var onlineUsers = _messenger.GetOnlineUsers(token);
			Clients.Caller.getOnlineUsers(onlineUsers);
		}

		public void WentOffline(string connectionId) {
			string token = Context.QueryString["token"];
			var user = _messenger.GetUser(token);

			Clients.AllExcept(connectionId).wentOffline((SocketUser)user);
		}
	}
}