using ChatRoom.Domain;
using ChatRoom.Server.Models.context;
using ChatRoom.Server.Models.requests;
using Microsoft.Web.WebSockets;
using Newtonsoft.Json;
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
			ChatRoomRequestBase request = JsonConvert.DeserializeObject<ChatRoomRequestBase>(message);

			switch(request.Type) {
				case RequestType.Default: break;
				case RequestType.CreateRoom:

					request = JsonConvert.DeserializeObject<ChatRoomCreateRoomRequest>(message);

					string roomId = SocketContext.Current.CreateChatRoomWithUsers(this, ((ChatRoomCreateRoomRequest)request).UserIds);
					SocketContext.Current.SendRoomCreatedNotification(this, roomId);
					break;
				case RequestType.PostInRoom:
					break;
				default:
					break;
			}


			//base.OnMessage(message);
		}

		public override void OnClose() {
			SocketContext.Current.WentOffline(this);
			SocketContext.Current.SendWentOfflineNotificationToUsers(this);
		}
	}
}