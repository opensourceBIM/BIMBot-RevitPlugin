namespace BimServerExchange.Objects
{
	public class LogoutResponse
	{
		public Reply response { get; set; }
	}

	public class Reply
	{
		public LogoutResult result { get; set; }
	}

	public class LogoutResult
	{
	}
}
