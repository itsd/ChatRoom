using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using SignalR.Models;
using System.Threading.Tasks;

namespace SignalR {

	[HubName("chat")]
	public class ChatHub : Hub {

		private string _dnsName;

		public ChatHub() {
			_dnsName = InstanceDetailsProvider.GetDnsName();
		}

		public override Task OnConnected() {
			Utils.Instance.IncreaseInstanceLoad(_dnsName);

			return base.OnConnected();
		}

		public override Task OnDisconnected() {
			Utils.Instance.DecreaseInstanceLoad(_dnsName);

			return base.OnDisconnected();
		}

		public override Task OnReconnected() {
			return base.OnReconnected();
		}

		public void writeSomething(string text) { 
			
		}
	}
}