using Newtonsoft.Json;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace BimServerExchange.Objects
{
	public class AddProjectRequest
	{
		public string token { get; set; }
		public AddRootProjectRequest request { get; set; }

		public AddProjectRequest(string cred)
		{
			token = cred ?? string.Empty;
			request = new AddRootProjectRequest();
		}
	}

	public class AddRootProjectRequest
	{
		[JsonProperty (PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public AddRootProjectParameters parameters { get; set; }

		public AddRootProjectRequest()
		{
			_interface = "ServiceInterface";
			method = "addProject";
			parameters = new AddRootProjectParameters();
		}
	}

	public class AddRootProjectParameters
	{
		public string projectName { get; set; }
		public string schema { get; set; }

		public AddRootProjectParameters()
		{
			projectName = string.Empty;
			schema = "ifc2x3tc1";
		}
	}
}
