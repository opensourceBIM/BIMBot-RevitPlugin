using Newtonsoft.Json;

namespace BimServerExchange.Objects
{
	public class GetFilesRequest
	{
		public string token { get; set; }
		public GetRevisionsByPoidRequest request { get; set; }

		public GetFilesRequest(string cred, int projectId)
		{
			token = cred ?? string.Empty;
			request = new GetRevisionsByPoidRequest(projectId);
		}
	}

	public class GetRevisionsByPoidRequest
	{
		[JsonProperty(PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public GetRevisionsByPoidParameters parameters { get; set; }

		public GetRevisionsByPoidRequest(int projectId)
		{
			_interface = "ServiceInterface";
			method = "getAllRevisionsOfProject";
			parameters = new GetRevisionsByPoidParameters(projectId);
		}
	}

	public class GetRevisionsByPoidParameters
	{
		public int poid { get; set; }

		public GetRevisionsByPoidParameters(int projectId)
		{
			poid = projectId;
		}
	}
}
