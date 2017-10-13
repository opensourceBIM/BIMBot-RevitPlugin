// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace BimServerExchange.Objects
{
	public class GetProjectDeserializerResponse
	{
		public GetDeserialiserResp response { get; set; }
	}

	public class GetDeserialiserResp
	{
		public SDeserializer[] result { get; set; }
	}

	public class SDeserializer
	{
		public string __type { get; set; }
		public string description { get; set; }
		public bool enabled { get; set; }
		public string name { get; set; }
		public int oid { get; set; }
		public int pluginDescriptorId { get; set; }
		public int rid { get; set; }
		public int settingsId { get; set; }
		public int userSettingsId { get; set; }
	}
}
