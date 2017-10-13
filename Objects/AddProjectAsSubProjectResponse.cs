// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Objects
{
	public class AddProjectAsSubProjectResponse
	{
		public AddSubProjectResponse response { get; set; }
	}

	public class AddSubProjectResponse
	{
		public SProject result { get; set; }
	}
}
