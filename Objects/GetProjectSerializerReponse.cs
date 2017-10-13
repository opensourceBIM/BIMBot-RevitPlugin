namespace BimServerExchange.Objects
{
	public class GetProjectSerializerResponse
	{
		public ProjectSerializerResponse response { get; set; }
	}

	public class ProjectSerializerResponse
	{
		public SSerializer[] result { get; set; }
	}

	public class SSerializer
	{
		public string __type { get; set; }
		public string description { get; set; }
		public bool enabled { get; set; }
		public string name { get; set; }
		public int objectIDMId { get; set; }
		public int oid { get; set; }
		public int pluginDescriptorId { get; set; }
		public int renderEngineId { get; set; }
		public int rid { get; set; }
		public int settingsId { get; set; }
		public bool streaming { get; set; }
		public int userSettingsId { get; set; }
	}
}
