using System;
using Newtonsoft.Json;

namespace BimServerExchange.Objects
{
	public class GetFilesResponse
	{
		public GetRevisinsByPoidResponse response { get; set; }
	}

	public class GetRevisinsByPoidResponse
	{
		public SRevision[] result { get; set; }
	}

	public class SRevision
	{
		public string __type { get; set; }
		public object bmi { get; set; }
		public object[] checkouts { get; set; }
		public string comment { get; set; }
		public int[] concreteRevisions { get; set; }
		public long date { get; set; }
		public object[] extendedData { get; set; }
		public bool hasGeometry { get; set; }
		public int id { get; set; }
		public int lastConcreteRevisionId { get; set; }
		public object lastError { get; set; }
		public int[] logs { get; set; }
		public int oid { get; set; }
		public int projectId { get; set; }
		public int rid { get; set; }
		public int serviceId { get; set; }
		public object[] servicesLinked { get; set; }
		public int size { get; set; }
		public string tag { get; set; }
		public int userId { get; set; }

		[JsonIgnore]
		public string Datum
		{
			get
			{
				DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local); // time in milisecond is not relative to Utc, that is about 2 hours off
				DateTime datum = epoch.AddMilliseconds(date);
				// looks like the time in miliseconds is a little off or the epoch time is (1970, 1, 1, 0, 0, DateTimeKind.Amsterdam)
				return datum.ToString("dd-MMM-yyyy HH:mm");
			}
		}

		[JsonIgnore]
		public string Desc
		{
			get
			{
				if (!string.IsNullOrEmpty(tag)) return tag;

				return comment ?? string.Empty;
			}
		}
	}
}
