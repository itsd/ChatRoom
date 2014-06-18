using SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SignalR.Controllers {
	[RoutePrefix("balancer")]
	public class BalancerController : ApiController {

		private string _dnsName;

		public BalancerController() {
			_dnsName = InstanceDetailsProvider.GetDnsName();
		}

		[Route("instance"), HttpGet]
		public InstanceModel GetLessLoadedInstanceName() {

			if(Utils.Instance.GetAllInstance() == 0) {
				Utils.Instance.SaveInstance(new Instance {
					Load = 0,
					Url = _dnsName
				});
			}

			var instance = Utils.Instance.GetLessLoadedInstance();
			return new InstanceModel { Url = string.Format("http://{0}", instance.Url) };
		}

		[Route("instance/current"), HttpGet]
		public InstanceModel GetCurrentInstanceDns() {
			return new InstanceModel {
				Url = _dnsName
			};
		}

		[Route("instance/registerCurrent"), HttpGet]
		public HttpResponseMessage RegisterCurrentInstance() {


			Utils.Instance.SaveInstance(new Instance {
				Load = 0,
				Url = _dnsName
			});

			return new HttpResponseMessage(HttpStatusCode.Created);
		}

		[Route("instance/{name}"), HttpGet]
		public void SaveInstance(string name) {
			Utils.Instance.SaveInstance(new Instance {
				Load = 0,
				Url = name
			});
		}
	}
}
