using Microsoft.AspNet.SignalR;
using Nostromo.Common.Interfaces;
using SignalR.ChatServer.Queues;
using SignalR.ChatServer.Queues.QueueConfigurations;
using SignalR.ChatServer.Queues.QueueItems;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SignalR.ChatServer.Context {
	public class PostContext : IObserver<PostItem> {

		private readonly static Lazy<PostContext> _instance = new Lazy<PostContext>(() => new PostContext());

		private IObservable<PostItem> queue;
		private IObservableQueue<PostItem> o;

		public static PostContext Instance {
			get {
				return _instance.Value;
			}
		}

		public void StartListening() {
			queue = new PostQueue(new PostQueueMongoConfiguration {
				CollectionMaxSize = 10000000,
				ConnectionString = ConfigurationManager.ConnectionStrings["MongoContext"].ConnectionString + "/ChatQueue",
				MaxDocuments = 10000
			});
			queue.Subscribe(this);
		}

		public void PostComment(PostItem post) {
			o = queue as IObservableQueue<PostItem>;
			o.Enqueue(post);
		}

		public void OnCompleted() {
			//throw new NotImplementedException();
		}

		public void OnError(Exception error) {
			//throw new NotImplementedException();
		}

		public void OnNext(PostItem value) {
			var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
			context.Clients.All.sendMessage(value.Comment);
		}
	}
}