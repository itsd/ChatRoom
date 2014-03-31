using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoom.Domain;
using System.Configuration;
using MongoDB.Driver.Builders;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ChatRoom.Mongo.Context {
	public class ChatRoomMongoContext {

		static ChatRoomMongoContext() {
			BsonClassMap.RegisterClassMap<Session>(cm => {
				cm.AutoMap();
				cm.SetIgnoreExtraElements(true);
				cm.SetIgnoreExtraElementsIsInherited(true);

				cm.SetIdMember(cm.MapProperty(x => x.Token));
				cm.IdMemberMap.SetIdGenerator(new NullIdChecker());
			});
		}

		public ChatRoomMongoContext() {
			Sessions.EnsureIndex(IndexKeys.Ascending("Token"));
		}

		private MongoDatabase _mongoDatabase(string connectionString) {
			MongoUrl url = MongoUrl.Create(connectionString);
			MongoClient client = new MongoClient(url);
			MongoServer server = client.GetServer();
			return server.GetDatabase(url.DatabaseName);
		}

		private MongoDatabase _mongoDB {
			get {
				return _mongoDatabase(ConfigurationManager.ConnectionStrings["ChatRoomMongoContext"].ConnectionString);
			}
		}

		public MongoCollection GetCollection(string collectionName, string connectionName = null) {
			return _mongoDB.GetCollection(collectionName);
		}

		public MongoCollection<T> GetCollection<T>(string collectionName, string connectionName = null) {
			return _mongoDB.GetCollection<T>(collectionName);
		}

		public MongoCollection<Session> Sessions {
			get { return GetCollection<Session>("Sessions"); }
		}
	}
}
