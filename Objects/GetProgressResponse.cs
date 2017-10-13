
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Objects
{
	public class GetProgressResponse
	{
		public ProgressResponse response { get; set; }
	}

	public class ProgressResponse
	{
		public GetProgressResult result { get; set; }
	}

	public class GetProgressResult
	{
		public string __type { get; set; }
		public long? end { get; set; }
		public string[] errors { get; set; }
		public object[] infos { get; set; }
		public int oid { get; set; }
		public int? progress { get; set; }
		public int rid { get; set; }
		public int? stage { get; set; }
		public long? start { get; set; }
		public string state { get; set; }
		public string title { get; set; }
		public object[] warnings { get; set; }
	}
}
