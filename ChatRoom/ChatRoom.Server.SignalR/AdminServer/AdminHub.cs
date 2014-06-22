using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using ChatRoom.Server.SignalR.ChatServer;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatRoom.Server.SignalR.AdminServer {

	[HubName("admin")]
	public class AdminHub : Hub {

		private Messanger _messenger;

		public AdminHub() {
			_messenger = Messanger.Instance;
		}

		public override System.Threading.Tasks.Task OnConnected() {
			return base.OnConnected();
		}

		public override System.Threading.Tasks.Task OnDisconnected() {
			return base.OnDisconnected();
		}

		public override System.Threading.Tasks.Task OnReconnected() {
			return base.OnReconnected();
		}

		public void Hello() {
			//var context = GlobalHost.ConnectionManager.GetHubContext<ChatRoomHub>();
			//context.Clients.All.Send("Admin", "stop the chat");
			//Clients.All.hello();
		}
	}
}