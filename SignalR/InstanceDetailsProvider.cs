using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace SignalR {

	public class InstanceDetailsProvider {

		#region Instance MetaData Values
		private static string GetInstanceMetaData(string route) {
			string metaValue = String.Empty;
			string urlFormat = "http://169.254.169.254/{0}";

			//Special url to get Amazon Meta Data
			HttpWebRequest request = HttpWebRequest.CreateHttp(String.Format(urlFormat, route));

			//Initiate request and get response
			using(HttpWebResponse response = request.GetResponse() as HttpWebResponse)
			using(Stream stream = response.GetResponseStream())
			using(StreamReader reader = new StreamReader(stream)) {
				metaValue = reader.ReadToEnd();
			}
			return metaValue;
		}

		public static string GetLocalInstanceMetaDataValue(EC2InstanceMetaData valueType) {
			string metaValue = String.Empty;
			string actionFormat = "latest/meta-data/{0}";

			try {
				string metaDataKey = String.Empty;

				switch(valueType) {
					case EC2InstanceMetaData.AMIId: {
							metaDataKey = "amiid";
							break;
						}
					case EC2InstanceMetaData.AMILaunchIndex: {
							metaDataKey = "ami-launch-index";
							break;
						}
					case EC2InstanceMetaData.AMIManifestPath: {
							metaDataKey = "ami-manifest-path";
							break;
						}
					case EC2InstanceMetaData.BlockDeviceMapping: {
							metaDataKey = "block-device-mapping/";
							break;
						}
					case EC2InstanceMetaData.HostName: {
							metaDataKey = "hostname";
							break;
						}
					case EC2InstanceMetaData.InstanceAction: {
							metaDataKey = "instance-action";
							break;
						}
					case EC2InstanceMetaData.InstanceId: {
							metaDataKey = "instance-id";
							break;
						}
					case EC2InstanceMetaData.InstanceType: {
							metaDataKey = "instance-type";
							break;
						}
					case EC2InstanceMetaData.KernelId: {
							metaDataKey = "kernel-id";
							break;
						}
					case EC2InstanceMetaData.LocalHostName: {
							metaDataKey = "local-hostname";
							break;
						}
					case EC2InstanceMetaData.LocalIPv4: {
							metaDataKey = "local-ipv4";
							break;
						}
					case EC2InstanceMetaData.IPv4Associations: {
							metaDataKey = "ipv4-associations";
							break;
						}
					case EC2InstanceMetaData.MacAddress: {
							metaDataKey = "mac";
							break;
						}
					case EC2InstanceMetaData.Network: {
							metaDataKey = "network/";
							break;
						}
					case EC2InstanceMetaData.Placement: {
							metaDataKey = "placement/";
							break;
						}
					case EC2InstanceMetaData.PublicHostName: {
							metaDataKey = "public-hostname";
							break;
						}
					case EC2InstanceMetaData.PublicIPv4: {
							metaDataKey = "public-ipv4";
							break;
						}
					case EC2InstanceMetaData.PublicKeys: {
							metaDataKey = "public-keys/";
							break;
						}
					case EC2InstanceMetaData.ReservationId: {
							metaDataKey = "reservation-id";
							break;
						}
					case EC2InstanceMetaData.SecurityGroups: {
							metaDataKey = "security-groups";
							break;
						}
				}

				//Get the metadata value
				metaValue = GetInstanceMetaData(String.Format(actionFormat, metaDataKey));
			} catch {
				metaValue = String.Empty;
			}
			return metaValue;
		}

		public static EC2InstanceIdentityDocument GetInstanceIdentityDocument() {
			EC2InstanceIdentityDocument doc = null;
			var identityDoc = GetInstanceMetaData("latest/dynamic/instance-identity/document");

			if(!String.IsNullOrWhiteSpace(identityDoc)) {
				doc = JsonConvert.DeserializeObject<EC2InstanceIdentityDocument>(identityDoc);
			}

			return doc;
		}
		#endregion

		public static EC2VirtualMachine GetLocalMachineDetails() {
			var doc = GetInstanceIdentityDocument();

			return new EC2VirtualMachine() {
				AMIID = doc.ImageID,
				Architecture = doc.Architecture,
				InstanceID = doc.InstanceID,
				InstanceType = doc.InstanceType,
				IpAddress = GetLocalInstanceMetaDataValue(EC2InstanceMetaData.PublicIPv4),
				LaunchTime = doc.PendingTime,
				Platform = "windows",
				PrivateDnsName = GetLocalInstanceMetaDataValue(EC2InstanceMetaData.LocalHostName),
				PrivateIpAddress = doc.PrivateIP,
				PublicDnsName = GetLocalInstanceMetaDataValue(EC2InstanceMetaData.PublicHostName),
				State = EC2VirtualMachineStates.Running,
				Region = doc.Region
			};
		}

		public static string GetDnsName() {
			//return string.Format("{0}/SignalR", InstanceDetailsProvider.GetLocalMachineDetails().PublicDnsName);
			//return "localhost:41585/SignalR";

			return "ec2-184-72-3-243.us-west-1.compute.amazonaws.com/SignalR";
		}
	}


	[Serializable]
	public enum EC2VirtualMachineStates {
		Pending = 0,
		Running = 16,
		ShuttingDown = 32,
		Terminated = 48,
		Stopping = 64,
		Stopped = 80
	}

	[Serializable]
	public enum Protocols {
		HTTP = 1,
		HTTPS = 2,
		TCP = 3,
		SSL = 4
	}

	[Serializable]
	public enum EC2InstanceTypes {
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		[EnumMember(Value = "t1.micro")]
		T1Micro = 1,

		[EnumMember(Value = "m1.small")]
		M1Small = 2,

		[EnumMember(Value = "m1.medium")]
		M1Medium = 3,

		[EnumMember(Value = "m1.large")]
		M1Large = 4,

		[EnumMember(Value = "m1.xlarge")]
		M1XLarge = 5,

		[EnumMember(Value = "m2.xlarge")]
		M2XLarge = 6,

		[EnumMember(Value = "m2.2xlarge")]
		M22XLarge = 7,

		[EnumMember(Value = "m2.4xlarge")]
		M24XLarge = 8,

		[EnumMember(Value = "m3.xlarge")]
		M3XLarge = 9,

		[EnumMember(Value = "m3.2xlarge")]
		M32XLarge = 10,

		[EnumMember(Value = "c1.medium")]
		C1Medium = 11,

		[EnumMember(Value = "c1.xlarge")]
		C1XLarge = 12,

		[EnumMember(Value = "cc2.8xlarge")]
		CC28XLarge = 13,

		[EnumMember(Value = "cr1.8xlarge")]
		CR18XLarge = 14,

		[EnumMember(Value = "hi1.4xlarge")]
		HI14XLarge = 15,

		[EnumMember(Value = "hs1.8xlarge")]
		HS18XLarge = 16,

		[EnumMember(Value = "cg1.4xlarge")]
		CG14XLarge = 17
	}

	[Serializable]
	public enum EC2InstanceMetaData {
		AMIId = 1,
		AMILaunchIndex = 2,
		AMIManifestPath = 3,
		BlockDeviceMapping = 4,
		HostName = 5,
		InstanceAction = 6,
		InstanceId = 7,
		InstanceType = 8,
		KernelId = 9,
		LocalHostName = 10,
		LocalIPv4 = 11,
		IPv4Associations = 12,
		MacAddress = 13,
		Network = 14,
		Placement = 15,
		PublicHostName = 16,
		PublicIPv4 = 17,
		PublicKeys = 18,
		ReservationId = 19,
		SecurityGroups = 20
	}

	[Serializable]
	public class EC2InstanceIdentityDocument {
		[JsonProperty("instanceId")]
		public string InstanceID { get; set; }

		[JsonProperty("billingProducts")]
		public List<string> BillingProducts { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }

		[JsonProperty("accountId")]
		public string AccountID { get; set; }

		[JsonProperty("instanceType")]
		public string InstanceType { get; set; }

		[JsonProperty("architecture")]
		public string Architecture { get; set; }

		[JsonProperty("kernelId")]
		public string KernelID { get; set; }

		[JsonProperty("ramdiskId")]
		public string RamdiskID { get; set; }

		[JsonProperty("pendingTime")]
		public DateTime PendingTime { get; set; }

		[JsonProperty("imageId")]
		public string ImageID { get; set; }

		[JsonProperty("availabilityZone")]
		public string AvailabilityZone { get; set; }

		[JsonProperty("devpayProductCodes")]
		public List<string> DevpayProductCodes { get; set; }

		[JsonProperty("privateIp")]
		public string PrivateIP { get; set; }

		[JsonProperty("region")]
		public string Region { get; set; }
	}


	[Serializable]
	public class EC2VirtualMachine {
		[Key]
		public string ID { get; set; }

		public string InstanceID { get; set; }
		public string Architecture { get; set; }
		public EC2VirtualMachineStates State { get; set; }
		public string InstanceType { get; set; }
		public string IpAddress { get; set; }
		public DateTime LaunchTime { get; set; }
		public string Platform { get; set; }
		public string PrivateDnsName { get; set; }
		public string PrivateIpAddress { get; set; }
		public string PublicDnsName { get; set; }
		public string Region { get; set; }
		public string AMIID { get; set; }
	}
}