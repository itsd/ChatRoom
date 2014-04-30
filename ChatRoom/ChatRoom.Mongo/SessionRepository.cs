using ChatRoom.Domain;
using ChatRoom.Domain.Interfaces.Repositories;
using ChatRoom.Mongo.Context;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Mongo {
	public class SessionRepository : MongoRepositoryBase, ISessionRepository {
		public SessionRepository(ChatRoomMongoContext context) : base(context) { }

		public void Save(Session session) {
			_context.Sessions.Save(session, WriteConcern.Unacknowledged);
		}

		public void Delete(string token) {
			_context.Sessions.Remove(
				Query.EQ("_id", token),
				WriteConcern.Unacknowledged
			);
		}

		public Session Fetch(string token) {
			return _context.Sessions.FindOne(Query.EQ("_id", token));
		}

		public Session Fetch(int userId) {
			return _context.Sessions.FindOne(Query.EQ("UserID", userId));
		}

		public void UpdateLastAccess(string token) {
			_context.Sessions.Update(
				Query.EQ("_id", token),
				Update.Set("LastAccess", DateTime.UtcNow),
				WriteConcern.Unacknowledged
			);
		}

	}
}
