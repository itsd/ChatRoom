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
			Log("Pass constructor");
		}

		#region BaseMethods

		public override Task OnConnected() {
			Log("Start onconnected");
			//Connection Properties
			string roomToken = Context.QueryString["groupToken"];
			string token = Context.QueryString["token"];
			string connection = Context.ConnectionId;

			//Store user
			var user = _messenger.GetUserByToken(token);
			_messenger.SaveUser(user, connection);

			//Add this user in all rooms
			var userRooms = _messenger.GetUserRoomIds(user.ID);
			if(userRooms != null) { foreach(var room in userRooms) { Groups.Add(connection, room); } }

			//Get Online users
			var users = _messenger.GetOnlineUsers(token);

			//Get this user to others
			Clients.AllExcept(_messenger.GetConnectionsForUser(token).ToArray()).getWhoCameOnline(user);

			if(users.Count() > 0) {
				//Get online users to this user
				Clients.Caller.getChatUsers(users);
			}

			Log("pass onconnected");

			return base.OnConnected();
		}

		public override Task OnDisconnected() {
			Log("start ondisconnected");

			//Connection Properties
			string roomToken = Context.QueryString["groupToken"];
			string token = Context.QueryString["token"];
			string connection = Context.ConnectionId;

			//Remove user
			var user = _messenger.GetUserByToken(token);
			bool notifyOthers = _messenger.RemoveUser(user, connection);

			//Remove this user from all rooms
			var userRooms = _messenger.GetUserRoomIds(user.ID);
			if(userRooms != null) { foreach(var room in userRooms) { Groups.Remove(connection, room); } }

			//If there is not any connection for this user notify to others
			if(notifyOthers) {
				Clients.Others.wentOffline(user);
			}

			Log("passed ondisconnected");
			return base.OnDisconnected();
		}

		public override Task OnReconnected() {
			return base.OnReconnected();
		}

		#endregion

		public void Log(string message) {
			string path = @"E:\Log\Log.txt";
			System.IO.File.AppendAllText(path, message);
		}

		public string SendMessage(string message, IEnumerable<int> userIds) {
			//Connection Properties
			string roomToken = Context.QueryString["groupToken"];
			string token = Context.QueryString["token"];
			string connection = Context.ConnectionId;

			//Determine post creator
			var user = _messenger.GetUserByToken(token);

			//If this is a first post in room
			if(string.IsNullOrEmpty(roomToken)) {

				//Find if room exists with this user Ids
				roomToken = _messenger.GetRoomByUserIds(userIds);

				if(string.IsNullOrEmpty(roomToken)) {
					//create room with user ids
					roomToken = _messenger.CreateRoom(userIds);

					var cons = _messenger.GetConnectionsForRoom(roomToken);
					//Add users in room
					foreach(var item in cons) {
						Groups.Add(item, roomToken);
						System.Diagnostics.Debug.WriteLine("Added connection {0}, in room {1}", item, roomToken);
					}
				}
			}

			//Get all connections for this user and exclute them
			var usersAllConnections = _messenger.GetUsersAllConnections(user.Token).ToArray();

			//Send message to all users exluding post creator
			Clients.Group(roomToken, usersAllConnections).getMessage(new { roomToken, message, user.Username, userIds });

			////Send message to creator users
			//Clients.Clients(usersAllConnections).getYourMessage(new { roomToken, message, user.Username, userIds });


			var userExpThis = (from x in usersAllConnections
							   where x != Context.ConnectionId
							   select x).ToArray();

			Clients.Clients(userExpThis).getYourMessage(new { roomToken, message, user.Username, userIds });

			//Clients.Others.getMessage(new { roomToken, message, user.Username, userIds });

			//Return room token to post creator
			return roomToken;
		}
	}
}