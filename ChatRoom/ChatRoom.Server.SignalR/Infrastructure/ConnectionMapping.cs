using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.SignalR.Infrastructure {
	public class ConnectionMapping<K> {
		private readonly Dictionary<K, HashSet<string>> _connections = new Dictionary<K, HashSet<string>>();

		public int Count {
			get {
				return _connections.Count;
			}
		}

		public void Add(K key, string connectionId) {
			lock(_connections) {
				HashSet<string> connections;
				if(!_connections.TryGetValue(key, out connections)) {
					connections = new HashSet<string>();
					_connections.Add(key, connections);
				}

				lock(connections) {
					connections.Add(connectionId);
				}
			}
		}

		public IEnumerable<string> GetConnections(K key) {
			HashSet<string> connections;
			if(_connections.TryGetValue(key, out connections)) {
				return connections;
			}

			return Enumerable.Empty<string>();
		}

		public void Remove(K key, string connectionId) {
			lock(_connections) {
				HashSet<string> connections;
				if(!_connections.TryGetValue(key, out connections)) {
					return;
				}

				lock(connections) {
					connections.Remove(connectionId);

					if(connections.Count == 0) {
						_connections.Remove(key);
					}
				}
			}
		}

		public IEnumerable<K> GetAllKeys() {
			return _connections.Select(x => x.Key);
		}

		public IEnumerable<string> GetAllByKey(K key) {
			if(_connections.ContainsKey(key)) {
				return _connections[key].Select(x => x);
			}
			return default(IEnumerable<string>);
		}

		public int CountForKey(K key) {
			return _connections.ContainsKey(key) ? _connections[key].Count : 0;
		}
	}
}