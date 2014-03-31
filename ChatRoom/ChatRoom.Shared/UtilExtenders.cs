using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Shared {
	public static class UtilExtenders {
		public static string ToMD5Hash(this string clearText) {
			UTF8Encoding encoding = new UTF8Encoding();
			return Convert.ToBase64String(MD5CryptoServiceProvider.Create().ComputeHash(encoding.GetBytes(clearText)));
		}

		public static T ScreamIfNull<T>(this T obj, string name = null) where T : class {
			if(obj == null) throw new ArgumentNullException(name ?? typeof(T).Name);
			return obj;
		}
	}
}
