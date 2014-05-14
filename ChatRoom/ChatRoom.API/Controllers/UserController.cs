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
	[RoutePrefix("user")]
	public class UserController : ApiController {

		private IUserService _userService;

		public UserController(IUserService userService) {
			_userService = userService.ScreamIfNull("userService");
		}

		[Route(""), HttpGet]
		public UserModel CurrentUser() {
			return _userService.Fetch(Session.Current.UserID);
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
