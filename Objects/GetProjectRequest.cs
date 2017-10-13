using Newtonsoft.Json;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace BimServerExchange.Objects
{
	public class GetProjectRequest
	{
		public string token { get; set; }
		public GetAllProjectRequest request { get; set; }

		public GetProjectRequest(string cred)
		{
			token = cred ?? string.Empty;
			request = new GetAllProjectRequest();
		}
	}

	public class GetAllProjectRequest
	{
		[JsonProperty (PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public GetAllProjectParameters parameters { get; set; }

		public GetAllProjectRequest()
		{
			_interface = "ServiceInterface";
			method = "getAllProjects";
			parameters = new GetAllProjectParameters();
		}
	}

	public class GetAllProjectParameters
	{
		public string onlyTopLevel { get; set; }
		public string onlyActive { get; set; }

		[JsonIgnore]
		public bool OnlyTopLevel
		{
			set
			{
				if (value) onlyTopLevel = "true";
				else onlyTopLevel = "false";
			}
		}

		[JsonIgnore]
		public bool OnlyActive
		{
			set
			{
				if (value) onlyActive = "true";
				else onlyActive = "false";
			}
		}

		public GetAllProjectParameters()
		{
			onlyTopLevel = "false";
			onlyActive = "false";
		}
	}
}
