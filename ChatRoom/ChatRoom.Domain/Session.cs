using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace ChatRoom.Domain {
	public class Session : IPrincipal {
		[Key]
		public string Token { get; set; }
		public int UserID { get; set; }
		public string Username { get; set; }
		public DateTime LastAccess { get; set; }

		public static Session Current {
			get { return Thread.CurrentPrincipal as Session; }
		}

		public IIdentity Identity {
			get { throw new NotImplementedException(); }
		}

		public bool IsInRole(string role) {
			throw new NotImplementedException();
		}

		public static explicit operator Session(User user) {
			return new Session {
				Token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"),
				LastAccess = DateTime.UtcNow,
				UserID = user.ID,
				Username = user.Username,
			};
		}
	}
}
