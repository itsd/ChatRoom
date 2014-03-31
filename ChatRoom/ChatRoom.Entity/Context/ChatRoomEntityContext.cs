using ChatRoom.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Entity.Context {
	public class ChatRoomEntityContext : DbContext {
		public DbSet<User> Users { get; set; }
	}
}
