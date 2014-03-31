using ChatRoom.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.WebSockets;

namespace ChatRoom.Server {
	/// <summary>
	/// Summary description for Server
	/// </summary>
	public class Server : IHttpHandler {

		//public void ProcessRequest(HttpContext context) {
		//	context.Response.ContentType = "text/plain";
		//	context.Response.Write("Hello World");
		//}

		public void ProcessRequest(HttpContext context) {
			if(context.IsWebSocketRequest) {
				string token = context.Request.QueryString["token"];
				context.AcceptWebSocketRequest(new ChatRoomWebSocket(token) { });
			}
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}