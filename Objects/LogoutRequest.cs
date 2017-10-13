namespace BimServerExchange.Objects
{
	public class LogoutRequest
	{
		public string token { get; set; }
		public LORequest request { get; set; }
	}

	public class LORequest
	{
		public string _interface { get; set; }
		public string method { get; set; }
		public LogoutParameters parameters { get; set; }
	}

	public class LogoutParameters
	{
	}
}
