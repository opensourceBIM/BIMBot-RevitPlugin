using Newtonsoft.Json;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace BimServerExchange.Objects
{
	public class GetProgressRequest
	{
		public string token { get; set; }
		public ProgressRequest request { get; set; }

		public GetProgressRequest(string cred, int topicId)
		{
			token = cred ?? string.Empty;
			request = new ProgressRequest(topicId);
		}
	}

	public class ProgressRequest
	{
		[JsonProperty(PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public GetProgressParameters parameters { get; set; }

		public ProgressRequest(int topicId)
		{
			_interface = "NotificationRegistryInterface";
			method = "getProgress";
			parameters = new GetProgressParameters(topicId);
		}
	}

	public class GetProgressParameters
	{
		public int topicId { get; set; }

		public GetProgressParameters(int id)
		{
			topicId = id;
		}
	}
}
