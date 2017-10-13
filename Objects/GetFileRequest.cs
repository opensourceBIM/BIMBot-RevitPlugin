using Newtonsoft.Json;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Objects
{
	public class GetFileRequest
	{
		public string token { get; set; }
		public GetExtendedDataFileRequest request { get; set; }

		public GetFileRequest(string cred, int fileId)
		{
			token = cred;
			request = new GetExtendedDataFileRequest(fileId);
		}
	}

	public class GetExtendedDataFileRequest
	{
		[JsonProperty(PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public GetExtendedDataFileParameters parameters { get; set; }

		public GetExtendedDataFileRequest(int fileId)
		{
			_interface = "ServiceInterface";
			method = "getFile";
			parameters = new GetExtendedDataFileParameters(fileId);
		}
	}

	public class GetExtendedDataFileParameters
	{
		public int fileId { get; set; }

		public GetExtendedDataFileParameters(int id)
		{
			fileId = id;
		}
	}

}
