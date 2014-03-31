using ChatRoom.Domain;
using ChatRoom.Server.Models.context;
using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.Models {
	public class ChatRoomWebSocket : WebSocketHandler {

		public Session Session;

		public ChatRoomWebSocket(string token) {
			Session = SocketContext.Current.GetUser(token);
		}

		public override void OnOpen() {
			SocketContext.Current.AddToPrivateRoom(this);
			SocketContext.Current.SendOnlineFriendsToUser(this);
			SocketContext.Current.SendComeToOnlineNotificationToUsers(this);
		}

		public override void OnMessage(string message) {
			base.OnMessage(message);
		}

		public override void OnClose() {
			SocketContext.Current.WentOffline(this);
			SocketContext.Current.SendWentOfflineNotificationToUsers(this);
		}
	}
}