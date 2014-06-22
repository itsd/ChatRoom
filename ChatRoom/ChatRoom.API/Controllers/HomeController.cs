using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ChatRoom.Shared;
using ChatRoom.Domain.Interfaces.Services;
using ChatRoom.API.Models.Account;
using ChatRoom.API.Models.User;
using ChatRoom.Domain;

namespace ChatRoom.API.Controllers {
	[RoutePrefix("")]
	public class HomeController : ApiController {
		//public ActionResult Index() {
		//	ViewBag.Title = "Home Page";
		//	return View();
		//}

		[Route(""), HttpGet]
		public HttpResponseMessage Index() {
			return new HttpResponseMessage {
				Content = new StringContent(string.Format("ChatRoom API is running ... {0}{0}WebPing Status{0}Machine Name: {1}{0}Processors: {2}{0}Working Memory: {3}", Environment.NewLine, Environment.MachineName, Environment.ProcessorCount, Environment.WorkingSet))
			};
		}
	}
}
