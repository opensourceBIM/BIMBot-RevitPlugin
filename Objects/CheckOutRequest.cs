using Newtonsoft.Json;

namespace BimServerExchange.Objects
{
	public class CheckOutRequest
	{
		public string token { get; set; }
		public CheckoutRequest request { get; set; }

		public CheckOutRequest(string cred, int revisionId, int serializerId)
		{
			token = cred ?? string.Empty;
			request = new CheckoutRequest(revisionId, serializerId);
		}
	}

	public class CheckoutRequest
	{
		[JsonProperty(PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public CheckOutParameters parameters { get; set; }

		public CheckoutRequest(int revisionId, int serializerId)
		{
			_interface = "ServiceInterface";
			method = "checkout";

			parameters = new CheckOutParameters(revisionId, serializerId);
		}
	}

	public class CheckOutParameters
	{
		public object roid { get; set; }
		public object serializerOid { get; set; }
		public string sync { get; set; }

		public CheckOutParameters(int revisionId, int serializerId)
		{
			roid = revisionId;
			serializerOid = serializerId;
			sync = "false";
		}
	}
}
