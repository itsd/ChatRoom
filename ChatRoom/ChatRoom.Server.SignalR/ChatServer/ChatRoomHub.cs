using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using ChatRoom.Domain;
using Microsoft.AspNet.SignalR.Hubs;
using ChatRoom.Server.SignalR.Models.entities;
using ChatRoom.Server.SignalR.Infrastructure;

namespace ChatRoom.Server.SignalR.ChatServer {

	[HubName("chatRoom")]
	public class ChatRoomHub : Hub {

		private Messanger _messenger;

		public ChatRoomHub() {
			_messenger = Messanger.Instance;
		}

		#region BaseMethods

		public override Task OnConnected() {
			//Connection Properties
			string roomToken = Context.QueryString["groupToken"];
			string token = Context.QueryString["token"];
			string connection = Context.ConnectionId;

			//Store user
			var user = _messenger.GetUserByToken(token);
			_messenger.SaveUser(user, connection);

			//Get Online users
			var users = _messenger.GetOnlineUsers(token);

			//Get this user to others
			Clients.AllExcept(_messenger.GetConnectionsForUser(token).ToArray()).getWhoCameOnline(user);

			//Get online users to this user
			Clients.Caller.getChatUsers(users);

			return base.OnConnected();
		}

		public override Task OnDisconnected() {
			//Connection Properties
			string roomToken = Context.QueryString["groupToken"];
			string token = Context.QueryString["token"];
			string connection = Context.ConnectionId;

			//Remove user
			var user = _messenger.GetUserByToken(token);
			bool notifyOthers = _messenger.RemoveUser(user, connection);

			//If there is not any connection for this user notify to others
			if(notifyOthers) {
				Clients.Others.wentOffline(user);
			}

			return base.OnDisconnected();
		}

		public override Task OnReconnected() {
			return base.OnReconnected();
		}

		#endregion

	}
}