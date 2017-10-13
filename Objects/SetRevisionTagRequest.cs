using Newtonsoft.Json;

namespace BimServerExchange.Objects
{
	public class SetRevisionTagRequest
	{
		public string token { get; set; }
		public RevisionTagRequest request { get; set; }

		public SetRevisionTagRequest(string cred, int projectId, string tag)
		{
			token = cred ?? string.Empty;
			request = new RevisionTagRequest(projectId, tag);
		}
	}

	public class RevisionTagRequest{
		[JsonProperty(PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public RevisionTagParameters parameters { get; set; }

		public RevisionTagRequest(int projectId, string tag)
		{
			_interface = "ServiceInterface";
			method = "setRevisionTag";
			parameters = new RevisionTagParameters(projectId, tag);
		}
	}

	public class RevisionTagParameters
	{
		public int roid { get; set; }
		public string tag { get; set; }

		public RevisionTagParameters(int projectId, string text)
		{
			roid = projectId;
			tag = text;
		}
	}
}
