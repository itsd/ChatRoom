using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR.Models {
	public class Instance {
		public object ID { get; set; }
		public string Url { get; set; }
		public int Load { get; set; }
	}
}