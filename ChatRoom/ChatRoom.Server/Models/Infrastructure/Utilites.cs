using ChatRoom.Domain;
using ChatRoom.Server.Models.responses;
using Microsoft.Web.WebSockets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.Models.Infrastructure {
	public static class Utilites {

		private static string _privateRoomKey {
			get { return "_private_room_{0}"; }
		}

		public static string UserPrivateRoomKey {
			get { return _privateRoomKey; }
		}

		public static string GetSessionPrivateKey(this Session session) {
			return string.Format(_privateRoomKey, session.Token);
		}

		public static void SendOnlineUsers(this WebSocketCollection socketCollection, ChatRoomOnlineUsersResponse message) {
			socketCollection.Broadcast(JsonConvert.SerializeObject(message, Formatting.Indented));
		}

		public static void SendComeOnline(this WebSocketCollection socketCollection, ChatRoomCameOnlineResponse message) {
			socketCollection.Broadcast(JsonConvert.SerializeObject(message, Formatting.Indented));
		}

		public static void SendWentOffline(this WebSocketCollection socketCollection, ChatRoomWentOfflineResponse message) {
			socketCollection.Broadcast(JsonConvert.SerializeObject(message, Formatting.Indented));
		}

		public static void SendRoomCreated(this WebSocketCollection socketCollection, ChatRoomRoomCreatedResponse message) {
			socketCollection.Broadcast(JsonConvert.SerializeObject(message, Formatting.Indented));
		}
	}
}