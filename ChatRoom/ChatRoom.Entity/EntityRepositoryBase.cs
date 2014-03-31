using ChatRoom.Entity.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Entity {
	public class EntityRepositoryBase {

		protected ChatRoomEntityContext _context;

		public EntityRepositoryBase(ChatRoomEntityContext context) {
			if(context == null) throw new ArgumentNullException("context");
			_context = context;
		}
	}
}
