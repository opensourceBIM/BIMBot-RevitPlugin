// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global
namespace BimServerExchange.Objects
{
	public class GetFileResponse
	{
		public Response response { get; set; }
	}

	public class Response
	{
		public SFile result { get; set; }
	}

	public class SFile
	{
		public string __type { get; set; }
		public string data { get; set; }
		public string filename { get; set; }
		public string mime { get; set; }
		public int oid { get; set; }
		public int rid { get; set; }
		public int size { get; set; }
	}

}
