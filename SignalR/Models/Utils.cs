using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SignalR.Models {
	public class Utils {

		private readonly static Lazy<Utils> _instance = new Lazy<Utils>(() => new Utils());

		public static Utils Instance {
			get {
				return _instance.Value;
			}
		}

		private MongoDatabase _mongoDatabase(string connectionString) {
			MongoUrl url = MongoUrl.Create(connectionString);
			MongoClient client = new MongoClient(url);
			MongoServer server = client.GetServer();
			return server.GetDatabase(url.DatabaseName);
		}

		private MongoDatabase _mongoDB {
			get {
				return _mongoDatabase(ConfigurationManager.ConnectionStrings["MongoContext"].ConnectionString + "/Instances");
			}
		}

		public MongoCollection GetCollection(string collectionName, string connectionName = null) {
			return _mongoDB.GetCollection(collectionName);
		}

		public MongoCollection<T> GetCollection<T>(string collectionName, string connectionName = null) {
			return _mongoDB.GetCollection<T>(collectionName);
		}

		public Instance GetLessLoadedInstance() {
			return (from x in _mongoDB.GetCollection<Instance>("Instances").AsQueryable()
					orderby x.Load ascending
					select x)
					.FirstOrDefault();
		}

		public void IncreaseInstanceLoad(object instanceID) {
			_mongoDB.GetCollection<Instance>("Instances").Update(
				Query.EQ("_id", BsonValue.Create(instanceID)),
				Update.Inc("Load", 1),
				WriteConcern.Unacknowledged);
		}

		public int GetAllInstance() {
			return _mongoDB.GetCollection<Instance>("Instances").AsQueryable().Count();
		}

		public void IncreaseInstanceLoad(string instanceUrl) {
			_mongoDB.GetCollection<Instance>("Instances").Update(
					Query.EQ("Url", instanceUrl),
					Update.Inc("Load", 1),
					WriteConcern.Unacknowledged);
		}

		public void DecreaseInstanceLoad(object instanceID) {
			_mongoDB.GetCollection<Instance>("Instances").Update(
			Query.EQ("_id", BsonValue.Create(instanceID)),
			Update.Inc("Load", -1),
			WriteConcern.Unacknowledged);
		}

		public void DecreaseInstanceLoad(string instanceUrl) {
			_mongoDB.GetCollection<Instance>("Instances").Update(
			Query.EQ("Url", instanceUrl),
			Update.Inc("Load", -1),
			WriteConcern.Unacknowledged);
		}

		public void SaveInstance(Instance instance) {
			_mongoDB.GetCollection<Instance>("Instances").Save(instance);
		}
	}
}