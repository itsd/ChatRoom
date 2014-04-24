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
			_messenger = new Messanger();
		}

		#region BaseMethods

		public override Task OnConnected() {


			//string token = Context.QueryString["token"];
			//string connection = Context.ConnectionId;
			//var user = _messenger.GetUserByToken(token);
			//_messenger.SaveUser(user, connection);

			//var onlineUsers = _messenger.GetOnlineUsers(token);

			//Clients.AllExcept(connection).getWhoCameOnline(user);
			//Clients.Caller.getOnlineUsers(onlineUsers);
			return base.OnConnected();
		}

		public override Task OnDisconnected() {
			//string token = Context.QueryString["token"];
			//var user = _messenger.GetUserByToken(token);
			//string connection = Context.ConnectionId;
			//_messenger.RemoveUser(user, connection);

			//Clients.AllExcept(connection).wentOffline(user);
			return base.OnDisconnected();
		}

		public override Task OnReconnected() {
			return base.OnReconnected();
		}

		#endregion

		public string SendMessage(string message, IEnumerable<int> userIds) {
			string roomToken = Context.QueryString["groupToken"];
			string token = Context.QueryString["token"];

			var createdBy = _messenger.GetUserByToken(token);




			//if(string.IsNullOrEmpty(roomToken)) {
			var user1Token = _messenger.GetConnection(userIds.First());
			var user2Token = _messenger.GetConnection(userIds.Skip(1).First());

			roomToken = //_messenger.CreateRoom(new List<string> { user1Token, user2Token });

			string.Format("{0}_{1}", user1Token, user2Token);

			Groups.Add(user1Token, roomToken);
			Groups.Add(user1Token, roomToken);
			//}


			//var user1Token = _messenger.GetConnection(userIds.First());
			//var user2Token = _messenger.GetConnection(userIds.Skip(1).First());

			//string roomKey = string.Format("{0}_{1}", user1Token, user2Token);

			//Groups.Add(user1Token, roomKey);
			//Groups.Add(user2Token, roomKey);

			Clients.OthersInGroup(roomToken).getMessage(roomToken, message, createdBy.Username);

			//Clients.OthersInGroup(roomToken).getMessage(roomToken, message, createdBy.Username);
			//Clients.Others.getMessage(roomToken, message, createdBy.Username, userIds);



			//Clients.All.getMessage(roomToken, message, createdBy.Username, userIds);


			return roomToken;
		}
	}
}