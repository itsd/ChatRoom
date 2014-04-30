using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.SignalR.Infrastructure {
	public class UserMapping<K, T> {
		private readonly Dictionary<K, T> _users = new Dictionary<K, T> { };

		public void Add(K key, T value) {
			lock(_users) {
				if(!_users.ContainsKey(key)) {
					_users.Add(key, value);
				}
			}
		}

		public void RemoveByKey(K key) {
			lock(_users) {
				if(_users.ContainsKey(key)) {
					_users.Remove(key);
				}
			}
		}

		public T Get(K key) {
			lock(_users) {
				return _users.ContainsKey(key) ? _users[key] : default(T);
			}
		}

		public IEnumerable<T> ValuesList {
			get { return _users.Values; }
		}
	}
}