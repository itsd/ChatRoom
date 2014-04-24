using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ChatRoom.Server.SignalR.Infrastructure {
	public class SignalRContext : IPrincipal {

		public IIdentity Identity {
			get { return _identity ?? (_identity = new SignalRIdentity(this)); }
		}private IIdentity _identity;

		public bool IsInRole(string role) {
			if(role == "") {
				return true;
			}
			return false;
		}

		public static SignalRContext Current {
			get { return _current ?? (_current = new SignalRContext()); }
		}private static SignalRContext _current;

		private SignalRContext() { }
	}

	public class SignalRIdentity : IIdentity {

		private SignalRContext _context;

		public SignalRIdentity(SignalRContext context) {
			_context = context;
		}

		public string AuthenticationType {
			get { return "SignalR Authentication"; }
		}

		public bool IsAuthenticated {
			get { throw new NotImplementedException(); }
		}

		public string Name {
			get { return "User XXXX"; }
		}
	}
}