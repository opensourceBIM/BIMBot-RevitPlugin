using System;
using System.IO;
using BimServerExchange.Runtime;
using Newtonsoft.Json;

namespace BimServerExchange.Objects
{
	public class CheckinRequest
	{
		public string token { get; set; }
		public UploadFileRequest request { get; set; }

		public CheckinRequest(string cred, int projectId, int deserializerId)
		{
			token = cred ?? string.Empty;
			request = new UploadFileRequest(projectId, deserializerId);
		}
		public void SetFile(string path)
		{
			if (string.IsNullOrEmpty(path) || !File.Exists(path)) throw new IcnException($"File '{path ?? string.Empty}' could not be found", 10, "SetFile");

			FileInfo info = new FileInfo(path);
			request.parameters.fileSize = (int)info.Length;
			request.parameters.fileName = Path.GetFileName(path);
			request.parameters.data = Convert.ToBase64String(File.ReadAllBytes(path));
		}
	}

	public class UploadFileRequest
	{
		[JsonProperty(PropertyName = "interface")]
		public string _interface { get; set; }
		public string method { get; set; }
		public UploadParameters parameters { get; set; }

		public UploadFileRequest(int projectId, int deserializerId)
		{
			_interface = "ServiceInterface";
			method = "checkin";
			parameters = new UploadParameters(projectId, deserializerId);
		}
	}

	public class UploadParameters
	{
		public int poid { get; set; }
		public string comment { get; set; }
		public int deserializerOid { get; set; }
		public int fileSize { get; set; }
		public string fileName { get; set; }
		public string data { get; set; }
		public string merge { get; set; }
		public string sync { get; set; }

		public UploadParameters(int projectId, int deserializerId)
		{
			poid = projectId;
			comment = string.Empty;
			deserializerOid = deserializerId;
			fileSize = 0;
			fileName = string.Empty;
			data = null;
			merge = "false";
			sync = "false";
		}
	}
}
