using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ChatRoom.Shared;
using ChatRoom.Domain.Interfaces.Services;
using ChatRoom.API.Models.Account;

namespace ChatRoom.API.Controllers {
	[RoutePrefix("user")]
	public class UserController : ApiController {

		private IUserService _userService;

		public UserController(IUserService userService) {
			_userService = userService.ScreamIfNull("userService");
		}

		[Route("{id:int}"), HttpGet]
		public SessionModel GetUser(int id) {
			var user = _userService.Fetch(id);
			return new SessionModel {
				UserID = user.ID,
				Username = user.Username
			};
		}
	}
}
