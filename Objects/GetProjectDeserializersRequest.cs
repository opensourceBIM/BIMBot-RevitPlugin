using Newtonsoft.Json;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Objects
{
	public class GetProjectDeserializersRequest
	{
		public string token { get; set; }
		public GetAllDeserialisersRequest request { get; set; }

		public GetProjectDeserializersRequest(string cred, int projectId = -1)
		{
			token = cred ?? string.Empty;
			request = new GetAllDeserialisersRequest(projectId);
		}
	}

	public class GetAllDeserialisersRequest
	{
		[JsonProperty(PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public GetDeserialisersParameters parameters { get; set; }

		public GetAllDeserialisersRequest(int projectid)
		{
			_interface = "PluginInterface";
			method = "getAllDeserializersForProject";
			parameters = new GetDeserialisersParameters(projectid);
		}
	}

	public class GetDeserialisersParameters
	{
		public string onlyEnabled { get; set; }
		public int poid { get; set; }

		public GetDeserialisersParameters(int projectId)
		{
			onlyEnabled = "false";
			poid = projectId;
		}
	}
}
