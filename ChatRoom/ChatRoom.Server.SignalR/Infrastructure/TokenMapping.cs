using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Server.SignalR.Infrastructure {
	public class TokenMapping<T, K> {
		private readonly Dictionary<T, HashSet<K>> _tokens = new Dictionary<T, HashSet<K>>();

		public int Count {
			get {
				return _tokens.Count;
			}
		}

		public Dictionary<T, HashSet<K>> Tokens {
			get { return _tokens; }
		}

		public void Add(T key, K token) {
			lock(_tokens) {
				HashSet<K> connections;
				if(!_tokens.TryGetValue(key, out connections)) {
					connections = new HashSet<K>();
					_tokens.Add(key, connections);
				}

				lock(connections) {
					connections.Add(token);
				}
			}
		}

		public IEnumerable<K> GetTokens(T key) {
			HashSet<K> tokens;
			if(_tokens.TryGetValue(key, out tokens)) {
				return tokens;
			}

			return Enumerable.Empty<K>();
		}

		public void Remove(T key, K token) {
			lock(_tokens) {
				HashSet<K> tokens;
				if(!_tokens.TryGetValue(key, out tokens)) {
					return;
				}

				lock(tokens) {
					tokens.Remove(token);

					if(tokens.Count == 0) {
						_tokens.Remove(key);
					}
				}
			}
		}
	}
}