
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global

namespace BimServerExchange.Objects
{
	public class GetExtendedDataResponse
	{
		public GetExtendedDataResponseByRevision response { get; set; }
	}

	public class GetExtendedDataResponseByRevision
	{
		public GetExtendedDataResponseByRevisionResult[] result { get; set; }
	}

	public class GetExtendedDataResponseByRevisionResult
	{
		public string __type { get; set; }
		public long added { get; set; }
		public int fileId { get; set; }
		public int oid { get; set; }
		public int projectId { get; set; }
		public int revisionId { get; set; }
		public int rid { get; set; }
		public int schemaId { get; set; }
		public int size { get; set; }
		public string title { get; set; }
		public object url { get; set; }
		public int userId { get; set; }
	}
}
