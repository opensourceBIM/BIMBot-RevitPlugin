using System.Globalization;
using Newtonsoft.Json;

namespace BimServerExchange.Objects
{
	public class GetProjectSerializerRequest
	{
		public string token { get; set; }
		public ProjectSerializerRequest request { get; set; }

		public GetProjectSerializerRequest(string cred, SProject project)
		{
			token = cred ?? string.Empty;
			request = new ProjectSerializerRequest(project);
		}
	}

	public class ProjectSerializerRequest
	{
		[JsonProperty(PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public ProjectSerializerParameters parameters { get; set; }

		public ProjectSerializerRequest(SProject project)
		{
			_interface = "PluginInterface";
			method = "getAllSerializersForPoids";
			parameters = new ProjectSerializerParameters(project);
		}
	}

	public class ProjectSerializerParameters
	{
		public string onlyEnabled { get; set; }
		public string[] poids { get; set; }

		public ProjectSerializerParameters(SProject project)
		{
			onlyEnabled = "false";
			poids = new string[1];
			poids[0] = project?.oid.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
		}
	}

}
