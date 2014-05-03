using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.SignalR.Infrastructure {
	public class RoomMapping<K> {
		private readonly Dictionary<K, HashSet<int>> _connections = new Dictionary<K, HashSet<int>>();

		public int Count {
			get {
				return _connections.Count;
			}
		}

		public void Add(K key, int userId) {
			lock(_connections) {
				HashSet<int> connections;
				if(!_connections.TryGetValue(key, out connections)) {
					connections = new HashSet<int>();
					_connections.Add(key, connections);
				}

				lock(connections) {
					connections.Add(userId);
				}
			}
		}

		public IEnumerable<int> GetConnections(K key) {
			HashSet<int> connections;
			if(_connections.TryGetValue(key, out connections)) {
				return connections;
			}

			return Enumerable.Empty<int>();
		}

		public void Remove(K key, int userId) {
			lock(_connections) {
				HashSet<int> connections;
				if(!_connections.TryGetValue(key, out connections)) {
					return;
				}

				lock(connections) {
					connections.Remove(userId);

					if(connections.Count == 0) {
						_connections.Remove(key);
					}
				}
			}
		}

		public IEnumerable<K> GetAllKeys() {
			return _connections.Select(x => x.Key);
		}

		public K GetByValues(IEnumerable<int> values) {
			var result = default(K);

			result = (from x in _connections
					  from k in values
					  where x.Value.Count == values.Count() && x.Value.Any(y => y == k)
					  select x.Key).FirstOrDefault();

			return result;
		}

		public IEnumerable<int> GetAllByKey(K key) {
			if(_connections.ContainsKey(key)) {
				return _connections[key].Select(x => x);
			}
			return default(IEnumerable<int>);
		}

		public IEnumerable<K> GetAllByContainingValue(int value) {
			return from x in _connections
				   where x.Value.Contains(value)
				   select x.Key;
		}

		public int CountForKey(K key) {
			return _connections.ContainsKey(key) ? _connections[key].Count : 0;
		}
	}
}