// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;
using System.Globalization;
using Newtonsoft.Json;

namespace BimServerExchange.Objects
{
	public class GetProjectResponse
	{
		public ProjectsResponse response { get; set; }
	}

	public class ProjectsResponse
	{
		public SProject[] result { get; set; }
	}

	public class SProject
	{
		public string __type { get; set; }
		public object[] checkouts { get; set; }
		public object[] concreteRevisions { get; set; }
		public int createdById { get; set; }
		public long createdDate { get; set; }
		public string description { get; set; }
		public string exportLengthMeasurePrefix { get; set; }
		public object[] extendedData { get; set; }
		public int geoTagId { get; set; }
		public int[] hasAuthorizedUsers { get; set; }
		public int id { get; set; }
		public int lastConcreteRevisionId { get; set; }
		public int lastRevisionId { get; set; }
		public int[] logs { get; set; }
		public object[] modelCheckers { get; set; }
		public string name { get; set; }
		public object[] newServices { get; set; }
		public int oid { get; set; }
		public int parentId { get; set; }
		public object[] revisions { get; set; }
		public int rid { get; set; }
		public string schema { get; set; }
		public bool sendEmailOnNewRevision { get; set; }
		public object[] services { get; set; }
		public string state { get; set; }
		public int?[] subProjects { get; set; }

		[JsonIgnore]
		public string CreateDate
		{
			get
			{
				DateTime dt = new DateTime(createdDate);
				return dt.ToString("dd-MMM-yyyy hh::mm", new CultureInfo("nl-NL"));
			}
		}
	}
}
