using System;
using System.Text;
using Newtonsoft.Json;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Objects
{
	public class DownloadRequest
	{
		public string token { get; set; }
		public DwnlRequest request { get; set; }

		public DownloadRequest(string cred, int revisionId, int serializerId)
		{
			token = cred ?? string.Empty;
			request = new DwnlRequest(revisionId, serializerId);
		}
	}

	public class DwnlRequest
	{
		[JsonProperty(PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public DownloadParameters parameters { get; set; }

		public DwnlRequest(int revisionId, int serializerId)
		{
			_interface = "ServiceInterface";
			method = "download";

			parameters = new DownloadParameters(revisionId, serializerId);
		}
	}

	public class DownloadParameters
	{
		public int[] roids { get; set; }
		public string query { get; set; }
		public object serializerOid { get; set; }
		public string sync { get; set; }

		public DownloadParameters(int revisionId, int serializerId)
		{
			roids = new int[1];
			roids[0] = revisionId;
			serializerOid = serializerId;
			sync = "false";
			//sync = "true";

			string q = "{\"includeAllFields\": true }";
			query = Convert.ToBase64String(Encoding.ASCII.GetBytes(q));
		}
	}
}
