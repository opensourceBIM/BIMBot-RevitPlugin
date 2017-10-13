using Newtonsoft.Json;

namespace BimServerExchange.Objects
{
	public class AddProjectAsSubProjectRequest
	{
		public string token { get; set; }
		public AddSubProjectRequest request { get; set; }

		public AddProjectAsSubProjectRequest(string cred)
		{
			token = cred ?? string.Empty;
			request = new AddSubProjectRequest();
		}
	}

	public class AddSubProjectRequest
	{
		[JsonProperty (PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public AddSubProjectParameters parameters { get; set; }

		public AddSubProjectRequest()
		{
			_interface = "ServiceInterface";
			method = "addProjectAsSubProject";
			parameters = new AddSubProjectParameters();
		}
	}

	public class AddSubProjectParameters
	{
		public string projectName { get; set; }
		public int parentPoid { get; set; }
		public string schema { get; set; }

		public AddSubProjectParameters()
		{
			projectName = string.Empty;
			parentPoid = -1;
			schema = "ifc2x3tc1";
		}
	}
}
