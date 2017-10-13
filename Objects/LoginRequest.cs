using Newtonsoft.Json;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace BimServerExchange.Objects
{
	public class LoginRequest
	{
		public Request request { get; set; }

		internal LoginRequest()
		{
			request = new Request();
		}
	}

	public class Request
	{
		[JsonProperty(PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public Parameters parameters { get; set; }

		internal Request()
		{
			_interface = "AuthInterface";
			method = "login";
			parameters = new Parameters();
		}
	}

	public class Parameters
	{
		public string username { get; set; }
		public string password { get; set; }

		internal Parameters()
		{
			username = string.Empty;
			password = string.Empty;
		}
	}
}
