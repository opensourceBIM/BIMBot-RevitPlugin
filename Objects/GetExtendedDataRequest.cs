using Newtonsoft.Json;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Objects
{
	public class GetExtendedDataRequest
	{
		public string token { get; set; }
		public GetExtendedDataByRoidRequest request { get; set; }

		public GetExtendedDataRequest(string cred, int revisionId)
		{
			token = cred ?? string.Empty;
			request = new GetExtendedDataByRoidRequest(revisionId);
		}
	}

	public class GetExtendedDataByRoidRequest
	{
		[JsonProperty(PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public GetExtendedDataByRoidParameters parameters { get; set; }

		public GetExtendedDataByRoidRequest(int revisionId)
		{
			_interface = "ServiceInterface";
			method = "getAllExtendedDataOfRevision";
			parameters = new GetExtendedDataByRoidParameters(revisionId);
		}
	}

	public class GetExtendedDataByRoidParameters
	{
		public int roid { get; set; }

		public GetExtendedDataByRoidParameters(int revisionId)
		{
			roid = revisionId;
		}
	}
}
