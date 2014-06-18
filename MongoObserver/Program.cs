using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Nostromo.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoObserver {
	class Program {
		public static Application app;

		static void Main(string[] args) {
			BsonClassMap.RegisterClassMap<ChatItem>(cm => {
				cm.AutoMap();
				cm.SetIdMember(cm.MapProperty(o => o.ID));
				cm.IdMemberMap.SetIdGenerator(new ObjectIdGenerator());
			});

			app = new Application();
			app.Start();

			Console.ReadKey();
		}
	}

	public class Application : IObserver<ChatItem> {
		private IObservable<ChatItem> queue;
		private IObservableQueue<ChatItem> o;

		public void Start() {
			queue = new ChatQueue(new ChatQueueMongoConfiguration() {
				CollectionMaxSize = 10000000,
				ConnectionString = "mongodb://server.itex.ge/ChatQueue",
				MaxDocuments = 10000
			});
			//o = queue as IObservableQueue<ChatItem>;

			//o.Enqueue(new ChatItem() {
			//	Created = DateTime.Now,
			//	Message = "Hello",
			//	RoomName = "room",
			//	UserID = 1
			//});

			var ssss = queue.Subscribe(this);
			//ssss.Dispose();
		}

		public void OnCompleted() {

		}

		public void OnError(Exception error) {

		}

		public void OnNext(ChatItem value) {
			Console.WriteLine(value.ToString());
		}
	}
}
