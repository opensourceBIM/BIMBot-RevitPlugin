using Newtonsoft.Json;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Objects
{
	public class GetDownloadDataRequest
	{
		public string token { get; set; }
		public DownloadDataRequest request { get; set; }

		public GetDownloadDataRequest(string cred, long topicId)
		{
			token = cred ?? string.Empty;
			request = new DownloadDataRequest(topicId);
		}
	}

	public class DownloadDataRequest
	{
		[JsonProperty(PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public DownloadDataParameters parameters { get; set; }

		public DownloadDataRequest(long topicId)
		{
			_interface = "ServiceInterface";
			method = "getDownloadData";
			parameters = new DownloadDataParameters(topicId);
		}
	}

	public class DownloadDataParameters
	{
		public long topicId { get; set; }

		public DownloadDataParameters(long tId)
		{
			topicId = tId;
		}
	}
}
