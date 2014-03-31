using ChatRoom.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ChatRoom.Shared;
using ChatRoom.API.Models.Account;
using ChatRoom.Domain;
using ChatRoom.Domain.Exceptions;

namespace ChatRoom.API.Controllers {
	[RoutePrefix("account")]
	public class AccountController : ApiController {

		private IUserService _userService;
		private ISessionService _sessionService;

		public AccountController(IUserService userService, ISessionService sessionService) {
			_userService = userService.ScreamIfNull("userService");
			_sessionService = sessionService.ScreamIfNull("sessionService");
		}

		[Route("login"), HttpPost]
		public SessionModel Login(LoginModel model) {
			try {
				return _sessionService.Login(model.Username, model.Password);
			} catch(LoginFailedException) { throw new HttpResponseException(HttpStatusCode.Forbidden); }
		}

		[Route("logout"), HttpPost]
		public HttpResponseMessage LogOut() {
			_sessionService.Logout(Session.Current.Token);
			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[Route("signup"), HttpPost]
		public SessionModel SignUp(SignupModel model) {
			try {
				var user = _userService.Register(model.Username, model.Password);
				return _sessionService.Login(model.Username, model.Password);
			} catch(UsernameExistsException) { throw new HttpResponseException(HttpStatusCode.Conflict); }
		}
	}
}
