using SignalR.ChatServer.Context;
using SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SignalR.Controllers {

	[RoutePrefix("posts")]
	public class PostController : ApiController {

		[Route("live"), HttpGet]
		public HttpResponseMessage Live() {

			return new HttpResponseMessage() {
				StatusCode = HttpStatusCode.OK
			};
		}

		[Route("liveInstance"), HttpGet]
		public InstanceModel LiveInstance() {

			return new InstanceModel {
				Url = InstanceDetailsProvider.GetDnsName()
			};

			//return new HttpResponseMessage() {
			//	StatusCode = HttpStatusCode.OK
			//};
		}

		[Route("createpost"), HttpPost]
		public HttpResponseMessage PostSomething(CreatePostModel model) {
			PostContext.Instance.PostComment(new ChatServer.Queues.QueueItems.PostItem {
				Comment = model.Comment,
				RoomToken = Guid.NewGuid().ToString("N")
			});

			return new HttpResponseMessage(HttpStatusCode.OK);
		}
	}
}
