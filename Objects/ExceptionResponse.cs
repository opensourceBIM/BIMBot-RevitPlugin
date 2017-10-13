// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace BimServerExchange.Objects
{
	public class ExceptionResponse
	{
		public ExceptionReply response { get; set; }
	}

	public class ExceptionReply
	{
		public ExceptionContent exception { get; set; }
	}

	public class ExceptionContent
	{
		public string __type { get; set; }
		public string message { get; set; }
	}
}
