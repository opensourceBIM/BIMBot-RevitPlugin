using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Autodesk.Revit.Exceptions;
using BimServerExchange.Objects;
using BimServerExchange.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using JsonConverter = Newtonsoft.Json.JsonConverter;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace BimServerExchange.Commands
{
	internal class CmdServerInterface
	{
		public event EventHandler<ProgressEventArgs> Progress;

		public delegate void ReportStatusDelegate(string status, bool reset);

		#region fields
		private Commander _cmd;
		private ReportStatusDelegate _reportStatus;
		private Thread thread;
		private bool _showProgress;
		private bool _monitorProgress;
		private string _uploadStatus;
		#endregion fields

		#region properties
		private Commander Cmd
		{
			get
			{
				if (null != _cmd) return _cmd;

				throw new IcnException("Property Cmd not set to a reference", 10, "CmdServerInterface");
			}
			set { _cmd = value; }
		}
		internal ReportStatusDelegate ReportStatus
		{
			get
			{
				if (null != _reportStatus) return _reportStatus;

				return null;
			}
			set
			{
				if (null == value) return;

				_reportStatus = value;
			}
		}

		internal bool ShowProgress
		{
			get { return _showProgress; }
			set { _showProgress = value; }
		}

		internal bool MonitorProgress
		{
			get { return _monitorProgress; }
			set { _monitorProgress = value; }
		}

		private string UploadStatus
		{
			get { return _uploadStatus ?? string.Empty; }
			set { _uploadStatus = value ?? string.Empty; }
		}

		#endregion properties

		#region ctors, dtor
		internal CmdServerInterface(Commander cmd)
		{
			Cmd = cmd;
			ShowProgress = true;
		}
		#endregion ctors, dtor

		#region private methods
		private string SendRequest(string json, BimServerExchangeData connectionData, bool isLogin = false)
		{
			if (string.IsNullOrEmpty(json) || null == connectionData || (!isLogin && string.IsNullOrEmpty(connectionData.Token))) return string.Empty;

			WebRequest myWebRequest = WebRequest.Create(new Uri($"{connectionData.Url}:{connectionData.Port}/json"));
			HttpWebRequest myHttpWebRequest = (HttpWebRequest) myWebRequest;
			//myHttpWebRequest.Headers.Add("Authorization", "Bearer " + _accessToken);
			myHttpWebRequest.Method = "POST";
			myHttpWebRequest.ContentType = "application/json";

			using (StreamWriter writer = new StreamWriter(myWebRequest.GetRequestStream()))
			{
				writer.Write(json);
				writer.Flush();
				writer.Close();
			}

			HttpWebResponse response = myHttpWebRequest.GetResponse() as HttpWebResponse;
			if (null == response) return string.Empty;

			string res;
			using (Stream responseStream = response.GetResponseStream())
			{
				if (responseStream == null) { return "Invalid response"; }

				StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
				res = reader.ReadToEnd();
			}

			return res;
		}

		private string SendRequestAsync(string json, BimServerExchangeData connectionData, bool isLogin = false)
		{
			UploadStatus = string.Empty;
			string res = string.Empty;
			int jsonLen = json.Length;
			if (ShowProgress) Cmd.InitialiseProgressbar(jsonLen);

			/*Thread*/
			thread = new Thread(() =>
			{
				try
				{
					//if (string.IsNullOrEmpty(json) || null == connectionData || (!isLogin && string.IsNullOrEmpty(connectionData.Token))) return string.Empty;
					if (string.IsNullOrEmpty(json) || null == connectionData || (!isLogin && string.IsNullOrEmpty(connectionData.Token))) throw new IcnException("One or more parameters not set to a reference", 10, "SendRequestAsync");

					WebRequest myWebRequest = WebRequest.Create(new Uri($"{connectionData.Url}:{connectionData.Port}/json"));
					HttpWebRequest myHttpWebRequest = (HttpWebRequest) myWebRequest;
					//myHttpWebRequest.Headers.Add("Authorization", "Bearer " + _accessToken);
					myHttpWebRequest.Method = "POST";
					myHttpWebRequest.ContentType = "application/json";

					using (StreamWriter writer = new StreamWriter(myWebRequest.GetRequestStream()))
					{
						int len = json.Length;
						int sz = 4096;
						if (len < sz)
						{
							writer.Write(json);
						}
						else
						{
							char[] bytes = json.ToCharArray();
							len = bytes.Length;

							while (len > 0)
							{
								writer.Write(bytes, bytes.Length - len, Math.Min(sz, len));

								if (ShowProgress) OnProgress(new ProgressEventArgs(bytes.Length - len));
								len -= sz;
							}

							// just in case, force the progress bar to fill completely
							if (ShowProgress) OnProgress(new ProgressEventArgs(jsonLen));
						}

						writer.Flush();
						writer.Close();
					}

					HttpWebResponse response = myHttpWebRequest.GetResponse() as HttpWebResponse;
					//if (null == response) return string.Empty;
					if (null == response) throw new IcnException("BIMserver did not respond to the request", 10, "SendRequestAsync");

					//string res = string.Empty;
					using (Stream responseStream = response.GetResponseStream())
					{
						//if (responseStream == null) { return "Invalid response"; }
						if (responseStream == null) throw new IcnException("BIMserver did not give a valid response", 10, "SendRequestAsync");

						StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
						res = reader.ReadToEnd();
					}
				}
				catch (ThreadAbortException)
				{
					Thread.CurrentThread.Abort();
					res = "Cancelled";
				}
				catch
				{
				}
			});
			thread.IsBackground = true;
			thread.Priority = ThreadPriority.Highest;
			thread.Start();
			while (thread.IsAlive)
			{
				// check for progress and display it
				Application.DoEvents();
			}

			/* error message handling */
			if (string.IsNullOrEmpty(res)) res = UploadStatus;
			return res;
		}

		private string SendGetRequestAsync(GetDownloadDataRequest request, string json, int serializerId, BimServerExchangeData connectionData, bool isLogin = false)
		{
			string res = string.Empty;
			//int jsonLen = json.Length;
			//Cmd.BimServerExchange.InitialiseProgressbar(jsonLen);

			//Thread thread = new Thread(() =>
			//{
			try
			{
				//if (string.IsNullOrEmpty(json) || null == connectionData || (!isLogin && string.IsNullOrEmpty(connectionData.Token))) return string.Empty;
				if (null == request || request.request.parameters.topicId < 0L || null == connectionData || (!isLogin && string.IsNullOrEmpty(connectionData.Token)))
				{
					throw new IcnException("One or more parameters not set to a reference", 10, "SendGetRequestAsync");
				}

				WebRequest myWebRequest = WebRequest.Create(new Uri($"{connectionData.Url}:{connectionData.Port}/download"));
				HttpWebRequest myHttpWebRequest = (HttpWebRequest) myWebRequest;
				myHttpWebRequest.Headers.Add("topicId", request.request.parameters.topicId.ToString());
				myHttpWebRequest.Headers.Add("token", connectionData.Token);
				myHttpWebRequest.Headers.Add("serializerOid", serializerId.ToString());
				myHttpWebRequest.Method = "GET";
				myHttpWebRequest.ContentType = "application/octet-stream";

				WebResponse resp = myHttpWebRequest.GetResponse();
				//if (null == resp) throw new IcnException("BIMserver did not respond to the request", 10, "SendGetRequestAsync");

				using (Stream stream = resp.GetResponseStream())
				{
					if (null == stream) throw new IcnException("BIMserver did not give a valid response", 10, "SendGetRequestAsync");
					//stream.Length

					using (StreamReader reader = new StreamReader(stream))
					{
						res = reader.ReadToEnd();
					}
				}
			}
			catch (IcnException)
			{
				Thread.CurrentThread.Abort();
			}
			//});
			//thread.IsBackground = true;
			//thread.Priority = ThreadPriority.Highest;
			//thread.Start();
			//while (thread.IsAlive)
			//{
			//	// check for progress and display it
			//	Application.DoEvents();
			//}

			return res;
		}

		private static JsonSerializerSettings DeserializeSettings => new JsonSerializerSettings
		{
			Converters = new List<JsonConverter>
			{
				new StringEnumConverter()
			}
		};

		private static JsonSerializerSettings SerializeSettings => new JsonSerializerSettings
		{
			Converters = new List<JsonConverter>
			{
				new StringEnumConverter(),
				new IsoDateTimeConverter {DateTimeFormat = "yyyy-MM-dd"}
			}
		};
		#endregion private methods

		internal string Login(BimServerExchangeData connectionData)
		{
			if (null == connectionData)
			{
				throw new IcnException("Parameter connectionData is not set to a reference or is not initialised", 10, "CmdServerInterface");
			}

			string res = string.Empty;
			try
			{
				LoginRequest req = new LoginRequest();
				req.request.parameters.username = connectionData.LoginName;
				req.request.parameters.password = connectionData.Password;
				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json)) return res;

				string response = SendRequest(json, connectionData, true);
				if (string.IsNullOrEmpty(response)) return string.Empty;

				// try to parse the response
				LoginResponse loginResponse = JsonConvert.DeserializeObject(response, typeof(LoginResponse), DeserializeSettings) as LoginResponse;
				if (null == loginResponse)
				{
					// see if the login returned an error, if so throw an exception
					ExceptionResponse exception = JsonConvert.DeserializeObject(response, typeof(ExceptionResponse), DeserializeSettings) as ExceptionResponse;
					if (null != exception) throw new IcnException($"{exception.response.exception.message}", 10, "Login");

					return string.Empty;
				}

				res = loginResponse.response.result;
			}
			catch (Exception ex)
			{
				throw new IcnException($"General exception '{ex.Message}'", 10, "CmdServerInterface");
			}

			return res;
		}

		internal void Logout(BimServerExchangeData connectionData)
		{
			if (string.IsNullOrEmpty(connectionData?.Token))
			{
				throw new IcnException("Parameter connectionData is not set to a reference or is not initialised", 10, "CmdServerInterface");
			}

			try
			{
				LogoutRequest req = new LogoutRequest();
				req.token = connectionData.Token;
				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json)) return;

				string resp = SendRequest(json, connectionData);
				if (string.IsNullOrEmpty(resp)) return;

				// ReSharper disable once UnusedVariable
				LogoutResponse lres = JsonConvert.DeserializeObject(resp, typeof(LogoutResponse), DeserializeSettings) as LogoutResponse;
				//if (null == lres) return;
			}
			catch (Exception ex)
			{
				throw new IcnException($"General exception '{ex.Message}'", 10, "CmdServerInterface");
			}
		}

		// not clear yet how to use the upload/checkin. the examples are, to say the least, spotty
		/*
		internal void Upload(BimServerExchangeData connectionData, string path)
		{
			if (null == connectionData || string.IsNullOrEmpty(connectionData.Token) || string.IsNullOrEmpty(path) || !Directory.Exists(path)) return;
			List<string> files = Directory.EnumerateFiles(path, "*.ifc").ToList();
			if (!files.Any()) return;

			BimServerInteface client = WebServiceProxy.Create_BasicHttpBinding_Instance(connectionData, Interface.ServiceInterface);
			try
			{
				// get the id of the project this file must be uploaded to

				foreach (string fpath in files)
				{
					sFile file = new sFile();
					file.data = null;    // byte[] of the file? that's going to be a mess
					file.size = 0l;		// size of the byte[]
					file.filename = fpath;
					file.oid = 0l;
					file.rid = 0;
					file.mime = string.Empty;
					ServicesServiceInterface.uploadFileResponse res = await client.Services.uploadFileAsync(file);
				}
			}
			catch (Exception ex)
			{
				string mssg = ex.Message;
			}
			finally
			{
				client.Services.Close();
			}
		}
		*/

		internal string TestJson(BimServerExchangeData connectionData)
		{
			if (string.IsNullOrEmpty(connectionData?.Token))
			{
				throw new IcnException("Parameter connectionData is not set to a reference or is not initialised", 10, "CmdServerInterface");
			}

			try
			{
				GetProjectRequest req = new GetProjectRequest(connectionData.Token);
				req.request.parameters.OnlyActive = false;
				req.request.parameters.OnlyTopLevel = false;

				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json)) return string.Empty;

				string resp = SendRequest(json, connectionData);
				if (string.IsNullOrEmpty(resp)) return string.Empty;

				string res = SendRequest(json, connectionData);
				if (string.IsNullOrEmpty(res)) return res ?? string.Empty;

				// this is a flattened list that can go directly into a (checked) treelist, using parentId and oid as sorting keys.
				GetProjectResponse response = JsonConvert.DeserializeObject(res, typeof(GetProjectResponse), DeserializeSettings) as GetProjectResponse;
				if (null == response) return string.Empty;

				// for testing extract the 
				StringBuilder builder = new StringBuilder();
				foreach (SProject proj in response.response.result)
				{
					if (-1 != proj.parentId) continue;

					builder.AppendLine($"{proj.name} ({proj.oid})");
				}

				return builder.ToString();
			}
			catch //(Exception ex)
			{
				return "Exception suppressed";
			}
		}

		internal List<SProject> GetAllProjects(BimServerExchangeData connectionData, bool includeDeletedProjects = false)
		{
			if (string.IsNullOrEmpty(connectionData?.Token))
			{
				throw new IcnException("Parameter connectionData is not set to a reference or is not initialised", 10, "CmdServerInterface");
			}

			List<SProject> res = new List<SProject>();
			res.Add(new SProject {name = "BIMserver Projects", parentId = -1, oid = -1});
			try
			{
				GetProjectRequest req = new GetProjectRequest(connectionData.Token);
				req.request.parameters.OnlyActive = false;
				req.request.parameters.OnlyTopLevel = false;

				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json)) return res;

				string response = SendRequest(json, connectionData);
				if (string.IsNullOrEmpty(response)) return res;

				// this is a flattened list that can go directly into a (checked) treelist, using parentId and oid as sorting keys.
				GetProjectResponse resp = JsonConvert.DeserializeObject(response, typeof(GetProjectResponse), DeserializeSettings) as GetProjectResponse;
				if (null == resp) return res;

				foreach (SProject proj in resp.response.result)
				{
					// skip projects that are marked as deleted
					if (!includeDeletedProjects && 0 == string.Compare(proj.state, "DELETED", StringComparison.InvariantCultureIgnoreCase)) continue;

					res.Add(proj);
				}
			}
			catch //(Exception ex)
			{
				res = new List<SProject>();
			}

			return res;
		}

		internal bool SubProjectExists(BimServerExchangeData connectionData, string name, SProject par)
		{
			if (string.IsNullOrEmpty(name)) return true;
			if (null == par || null == connectionData) return true;

			List<SProject> projects = GetAllProjects(connectionData);
			if (null == projects || projects.Count <= 0) return false;

			foreach (SProject proj in projects)
			{
				if (proj.parentId != par.oid) continue;

				if (0 == string.Compare(proj.name, name, StringComparison.InvariantCultureIgnoreCase)) return true;
			}

			return false;
		}

		internal SProject AddProjectTo(BimServerExchangeData connectionData, string name, string desc, SProject par, string ifcFormat)
		{
			if (string.IsNullOrEmpty(name) || null == par || null == connectionData)
			{
				throw new IcnException("One or more parameters not set to a reference", 10, "AddProjectTo");
			}

			if (string.IsNullOrEmpty(ifcFormat)) ifcFormat = "Ifc2x3tc1";

			SProject res;
			try
			{
				AddProjectAsSubProjectRequest req = new AddProjectAsSubProjectRequest(connectionData.Token);
				req.request.parameters.projectName = name;
				req.request.parameters.parentPoid = par.oid;
				req.request.parameters.schema = ifcFormat;

				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json)) throw new IcnException("Could not serialize AddProjectAsSubProjectRequest", 10, "AddProjectTo");

				string response = SendRequest(json, connectionData);
				if (string.IsNullOrEmpty(response)) throw new IcnException("BIMserver did not create a new project", 10, "AddProjectTo");

				// this is a flattened list that can go directly into a (checked) treelist, using parentId and oid as sorting keys.
				AddProjectAsSubProjectResponse resp = JsonConvert.DeserializeObject(response, typeof(AddProjectAsSubProjectResponse), DeserializeSettings) as AddProjectAsSubProjectResponse;
				if (null == resp)
				{
					ExceptionResponse exception = JsonConvert.DeserializeObject(response, typeof(ExceptionResponse), DeserializeSettings) as ExceptionResponse;
					if (null != exception) throw new IcnException($"{exception.response.exception.message}", 10, "AddProjectTo");

					throw new IcnException("Could not deserialize the response from the BIMserver", 10, "AddProjectTo");
				}

				res = resp.response.result;
			}
			catch (Exception ex)
			{
				throw new IcnException($"General error ({ex.Message})", 10, "AddProjectTo");
			}

			return res;
		}

		internal SProject AddRootProject(BimServerExchangeData connectionData, string name, string desc = null, string ifcFormat = null)
		{
			if (string.IsNullOrEmpty(name) || null == connectionData)
			{
				throw new IcnException("One or more parameters not set to a reference", 10, "AddProjectTo");
			}

			if (string.IsNullOrEmpty(ifcFormat)) ifcFormat = "ifc2x3tc1";

			SProject res;
			try
			{
				AddProjectRequest req = new AddProjectRequest(connectionData.Token);
				req.request.parameters.projectName = name;
				req.request.parameters.schema = ifcFormat;

				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json)) throw new IcnException("Could not serialize AddProjectRequest", 10, "AddRootProject");

				string response = SendRequest(json, connectionData);
				if (string.IsNullOrEmpty(response)) throw new IcnException("BIMserver did not create a new project", 10, "AddRootProject");

				// this is a flattened list that can go directly into a (checked) treelist, using parentId and oid as sorting keys.
				AddProjectResponse resp = JsonConvert.DeserializeObject(response, typeof(AddProjectResponse), DeserializeSettings) as AddProjectResponse;
				if (null == resp || null == resp.response.result)
				{
					ExceptionResponse exception = JsonConvert.DeserializeObject(response, typeof(ExceptionResponse), DeserializeSettings) as ExceptionResponse;
					if (null != exception) throw new IcnException($"{exception.response.exception.message}", 10, "AddRootProject");

					throw new IcnException("Could not deserialize the response from the BIMserver", 10, "AddRootProject");
				}

				res = resp.response.result;
			}
			catch (Exception ex)
			{
				throw new IcnException($"General error ({ex.Message})", 10, "AddRootProject");
			}

			return res;
		}

		internal SProject GetProject(BimServerExchangeData connectionData, string name)
		{
			if (string.IsNullOrEmpty(connectionData?.Token) || string.IsNullOrEmpty(name)) return null;

			List<SProject> projects = GetAllProjects(connectionData);
			if (null == projects || projects.Count <= 0) return null;

			List<string> projectNames = name.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries).ToList();
			if (projectNames.Count <= 0) return null;

			SProject par = null;
			// ReSharper disable once ForCanBeConvertedToForeach
			for (int i = 0; i < projectNames.Count; i++)
			{
				string projectname = projectNames[i];
				foreach (SProject proj in projects)
				{
					if (0 != string.Compare(proj.name, projectname, StringComparison.InvariantCultureIgnoreCase)) continue;
					if (null != par && par.oid != proj.parentId) continue;

					// this is the project we are looking for
					par = proj;
				}
			}

			return par;
		}

		// ReSharper disable once MemberCanBePrivate.Global
		internal SProject GetProject(BimServerExchangeData connectionData, int projectId)
		{
			if (string.IsNullOrEmpty(connectionData?.Token) || projectId <= 0) return null;

			List<SProject> projects = GetAllProjects(connectionData);
			if (null == projects || projects.Count <= 0) return null;

			SProject res = null;
			foreach (SProject proj in projects)
			{
				if (proj.oid != projectId) continue;

				res = proj;
				break;
			}

			return res;
		}

		internal SDeserializer GetDeserialiser(BimServerExchangeData connectionData, int projectId, string name)
		{
			if (projectId < 0 || string.IsNullOrEmpty(connectionData?.Token))
			{
				throw new IcnException("One or more parameters not set to a reference", 10, "GetDeserialiser");
			}

			if (string.IsNullOrEmpty(name)) name = "Ifc2x3tc1";

			SDeserializer res = null;

			try
			{
				GetProjectDeserializersRequest req = new GetProjectDeserializersRequest(connectionData.Token, projectId);
				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json)) throw new IcnException("Could not serialize GetDeserialisersRequest", 10, "GetDeserialiser");

				string response = SendRequest(json, connectionData);
				if (string.IsNullOrEmpty(response)) throw new IcnException("BIMserver did not respond", 10, "GetDeserialiser");

				GetProjectDeserializerResponse resp = JsonConvert.DeserializeObject(response, typeof(GetProjectDeserializerResponse), DeserializeSettings) as GetProjectDeserializerResponse;
				if (null == resp) throw new IcnException("Could not deserialize the response from the BIMserver", 10, "GetDeserialiser");

				foreach (SDeserializer deserializer in resp.response.result)
				{
					if (0 != string.Compare(deserializer.name, name, StringComparison.InvariantCultureIgnoreCase)) continue;

					res = deserializer;
					break;
				}
			}
			catch (Exception ex)
			{
				throw new IcnException($"General error ({ex.Message})", 10, "GetDeserialiser");
			}

			return res;
		}

		internal SSerializer GetSerialiser(BimServerExchangeData connectionData, SProject project, string name)
		{
			if (null == project || string.IsNullOrEmpty(connectionData?.Token))
			{
				throw new IcnException("One or more parameters not set to a reference", 10, "GetSerialiser");
			}

			if (string.IsNullOrEmpty(name))
			{
				name = "Ifc2x3tc1";
			}

			SSerializer res = null;

			try
			{
				GetProjectSerializerRequest req = new GetProjectSerializerRequest(connectionData.Token, project);
				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json))
				{
					throw new IcnException("Could not serialize GetDeserialisersRequest", 10, "GetSerialiser");
				}

				string response = SendRequest(json, connectionData);
				if (string.IsNullOrEmpty(response))
				{
					throw new IcnException("BIMserver did not respond", 10, "GetSerialiser");
				}

				GetProjectSerializerResponse resp = JsonConvert.DeserializeObject(response, typeof(GetProjectSerializerResponse), DeserializeSettings) as GetProjectSerializerResponse;
				if (null == resp)
				{
					throw new IcnException("Could not deserialize the response from the BIMserver", 10, "GetSerialiser");
				}

				foreach (SSerializer serializer in resp.response.result)
				{
					if (0 != string.Compare(serializer.name, name, StringComparison.InvariantCultureIgnoreCase))
					{
						continue;
					}

					res = serializer;
					break;
				}
			}
			catch (Exception ex)
			{
				throw new IcnException($"General error ({ex.Message})", 10, "GetDeserialiser");
			}

			return res;
		}

		internal int CheckinFile(BimServerExchangeData connectionData, string path, string comment, int projectId, int deserializerId, bool checkin, out string result)
		{
			ShowProgress = true;
			if (!checkin)
			{
				return UploadFile(connectionData, path, comment, projectId, deserializerId, true, out result);
			}

			if (projectId < 0 || deserializerId < 0 || string.IsNullOrEmpty(connectionData?.Token) || string.IsNullOrEmpty(path) || !File.Exists(path))
			{
				throw new IcnException("One or more parameters is invalid or not set to a reference", 10, "CheckinFile");
			}

			int fileId = -1;
			try
			{
				result = "Unknown error";

				CheckinRequest req = new CheckinRequest(connectionData.Token, projectId, deserializerId);
				req.SetFile(path);
				req.request.parameters.comment = comment;
				// set the filename, the filesize and the Base64 bytes 

				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json)) throw new IcnException("Could not serialize UploadFileRequest", 10, "CheckinFile");

				string fname = Path.GetFileNameWithoutExtension(path);
				if (string.IsNullOrEmpty(fname)) fname = path;
				ReportStatus($"Copying project '{fname}' to the BIMserver", false);
				string response = SendRequestAsync(json, connectionData);
				if (string.IsNullOrEmpty(response)) throw new IcnException("BImserver did not respond", 10, "CheckinFile");

				if (0 == string.Compare(response, "Cancelled", StringComparison.InvariantCultureIgnoreCase))
				{
					result = "Upload cancelled";
					return fileId;
				}

				CheckInResponse resp = JsonConvert.DeserializeObject(response, typeof(CheckInResponse), DeserializeSettings) as CheckInResponse;
				if (null == resp)
				{
					ExceptionResponse exception = JsonConvert.DeserializeObject(response, typeof(ExceptionResponse), DeserializeSettings) as ExceptionResponse;
					if (null != exception) throw new IcnException($"Could not copy the IFC file to the BIMserver ({exception.response.exception.message})", 10, "CheckinFile");

					throw new IcnException("Could not deserialize the response from the BIMserver", 10, "CheckinFile");
				}

				// if response is not an error it would be the process id which may or may not be running still?
				// resp is useless for finding the file, but we can get the SProject and its lastConcreteRevision should be the file we just checked in
				SProject project = GetProject(connectionData, projectId);
				if (null != project) fileId = project.lastRevisionId;

				int topicId = resp.response?.result ?? -1;
				if (topicId < 0) result = "Upload failed";
				else result = "Upload succeeded";

				return topicId;
			}
			catch (IcnException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new IcnException($"General error ({ex.Message})", 10, "CheckinFile");
			}
		}

		internal int MonitorProcessingState(BimServerExchangeData connectionData, int topicId, out string result)
		{
			// This is to make the processing status modeless. It does not currently work because the entire exporter form must be modeless as well
			// or that will still keep blocking revit.
			//if (topicId > 0)
			//{
			//	// the serverinterface doesn't have a UIdoc (doesn't need it) so query the Commander indirectly for a uidoc for one of the subcommanders that do have this property
			//	ProcessingStatusForm frm = new ProcessingStatusForm(Cmd.UIDoc);
			//	frm.Show();
			//	frm.RunStatusUpdate(connectionData, topicId);
			//}
			result = "No running checkin process";
			if (topicId < 0) return -1;

			MonitorProgress = true;
			ShowProgress = false;
			result = "Checkin progress could not be monitored";
			string res = string.Empty;

			ReportStatus?.Invoke("0", true);			// resets the progress bar to run between 0 and 100 and sets it to position 0
			int stage = -1;

			/*Thread */
			thread = new Thread(() =>
			{
				while (MonitorProgress)
				{
					string errors;
					res = GetProgress(connectionData, topicId, out errors);
					if (0 == string.Compare(res, "OK", StringComparison.InvariantCultureIgnoreCase))
					{
						res = "Project is uploaded to the BIMserver";
						OnProgress(new ProgressEventArgs {Value = 100});
						MonitorProgress = false;
					}
					else if (0 == string.Compare(res, "FAILED", StringComparison.InvariantCultureIgnoreCase))
					{
						res = $"Project could not be uploaded to the BIMserver\n{errors ?? string.Empty}";
						OnProgress(new ProgressEventArgs {Value = 0});
						MonitorProgress = false;
					}
					else
					{
						string[] splits = res.Split(new[] {" : "}, StringSplitOptions.RemoveEmptyEntries);
						if (splits.Length > 1)
						{
							// pct is the reported stage running from 0 (or 1?) to #.
							// Excactly how high stage can go is undetermined but it is not consistent with the actual process on the BIMserver
							int.TryParse(splits[1], out int pct);
							int fac = 25;
							if (pct > 0)
							{
								fac = 100 / pct;
								if (fac > 25) fac = 25;
							}
							while (pct > stage)
							{
								stage += 1;
								OnProgress(new ProgressEventArgs {Value = stage * fac});
							}
						}

						if (splits.Length > 2)
						{
							ReportStatus?.Invoke(splits[2], false);
						}

						// this should sleep the current (monitoring) thread, not the main (Revit) one
						Thread.Sleep(TimeSpan.FromSeconds(1));
					}
				}
			});

			thread.IsBackground = true;
			thread.Priority = ThreadPriority.Highest;
			thread.Start();
			while (thread.IsAlive)
			{
				// check for progress and display it
				Application.DoEvents();
			}

			result = res;
			ShowProgress = true;
			MonitorProgress = false;

			return topicId;
		}

		internal void UploadProcessingStatusAsync(BimServerExchangeData connectionData, int topicId, out string result)
		{
			result = "Invalid input";
			if (topicId <= 0) return;

			string vv = "Upload failed";

			thread = new Thread(() =>
			{
				try
				{
					bool loop = true;
					ShowProgress = false;

					if (null != ReportStatus) ReportStatus("0", true);
					int stage = -1;
					while (loop)
					{
						string errors;
						vv = GetProgress(connectionData, topicId, out errors);
						if (0 == string.Compare(vv, "OK", StringComparison.InvariantCultureIgnoreCase))
						{
							vv = "Project is uploaded to the BIMserver";
							loop = false;
						}
						else if (0 == string.Compare(vv, "FAILED", StringComparison.InvariantCultureIgnoreCase))
						{
							vv = $"Project could not be uploaded to the BIMserver\n{errors ?? string.Empty}";
							loop = false;
						}
						else
						{
							string[] splits = vv.Split(new[] {" : "}, StringSplitOptions.RemoveEmptyEntries);
							if (splits.Length > 1)
							{
								int.TryParse(splits[1], out int pct);
								while (pct > stage)
								{
									stage += 1;
									OnProgress(new ProgressEventArgs {Value = stage * 25});
								}
							}

							if (null != ReportStatus && splits.Length > 2)
							{
								ReportStatus(splits[2], false);
							}
						}
					}
				}
				catch (ThreadAbortException)
				{
					Thread.CurrentThread.Abort();
					vv = "Cancelled";
				}
				catch
				{
				}

				ShowProgress = true;
			});

			thread.IsBackground = true;
			thread.Priority = ThreadPriority.Highest;
			thread.Start();
			while (thread.IsAlive)
			{
				// check for progress and display it
				Application.DoEvents();
			}

			result = vv;
		}

		internal string GetProgress(BimServerExchangeData connectionData, int topicId, out string errors)
		{
			errors = string.Empty;

			if (string.IsNullOrEmpty(connectionData?.Token) || topicId < 0)
			{
				return string.Empty;
			}

			string res;
			try
			{
				GetProgressRequest req = new GetProgressRequest(connectionData.Token, topicId);

				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json)) throw new IcnException("Could not serialize GetProgressRequest", 10, "GetProgress");

				//string response = SendRequestAsync(json, connectionData);
				string response = SendRequest(json, connectionData);
				if (string.IsNullOrEmpty(response)) throw new IcnException("BIMserver did not respond", 10, "GetProgress");

				GetProgressResponse resp = JsonConvert.DeserializeObject(response, typeof(GetProgressResponse), DeserializeSettings) as GetProgressResponse;
				if (null == resp?.response?.result)
				{
					ExceptionResponse exception = JsonConvert.DeserializeObject(response, typeof(ExceptionResponse), DeserializeSettings) as ExceptionResponse;
					if (null != exception) throw new IcnException($"{exception.response.exception.message}", 10, "GetProgress");

					throw new IcnException("Could not deserialize the response from the BIMserver", 10, "GetProgress");
				}

				// depending on what GetProgressResponse is do something
				if (string.IsNullOrEmpty(resp.response.result.state))
				{
					//throw new IcnException("Could not deserialize the response from the BIMserver", 10, "GetProgress");
					res = "BIMserver progress can not be monitored : 0";
					return res;
				}

				if (0 == string.Compare(resp.response.result.state, "FINISHED", StringComparison.InvariantCultureIgnoreCase))
				{
					res = "OK";
				}
				else if (resp.response.result.errors.Length > 0)
				{
					StringBuilder builder = new StringBuilder();
					builder.Append(" \nErrors: \n");
					int i = 1;
					foreach (string err in resp.response.result.errors)
					{
						builder.Append($"{i} - {err} \n");
						i += 1;
					}

					res = "Failed";
					errors = builder.ToString();
				}
				else if (resp.response.result.warnings.Length > 0)
				{
					StringBuilder builder = new StringBuilder();
					builder.Append(" \nWarnings: \n");
					int i = 1;
					foreach (string err in resp.response.result.warnings)
					{
						builder.Append($"{i} - {err} \n");
						i += 1;
					}

					res = "Failed";
					errors = builder.ToString();
				}
				else
				{
					int stage = resp.response.result.stage ?? 0;
					res = $"{resp.response.result.state} : {stage} : {resp.response.result.title ?? string.Empty}";
				}
			}
			catch (IcnException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new IcnException($"General error ({ex.Message})", 10, "GetProgress");
			}

			return res;
		}

		internal int UploadFile(BimServerExchangeData connectionData, string path, string comment, int projectId, int deserializerId, bool upload, out string result)
		{
			if (!upload)
			{
				int topicId = CheckinFile(connectionData, path, comment, projectId, deserializerId, true, out result);
				if (topicId < 0) return topicId;

				return MonitorProcessingState(connectionData, topicId, out result);
			}

			if (projectId < 0 || deserializerId < 0 || string.IsNullOrEmpty(connectionData?.Token) || string.IsNullOrEmpty(path) || !File.Exists(path))
			{
				throw new IcnException("One or more parameters is invalid or not set to a reference", 10, "CheckinFile");
			}

			result = "Upload File is not implemented yet";
			int fileId = -1;
			try
			{
				// this must be changed to use UploadRequest en UploadResponse
				CheckinRequest req = new CheckinRequest(connectionData.Token, projectId, deserializerId);
				req.SetFile(path);
				// set the filename, the filesize and the Base64 bytes 

				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json))
				{
					throw new IcnException("Could not serialize UploadFileRequest", 10, "UploadFile");
				}

				string response = SendRequestAsync(json, connectionData);
				if (string.IsNullOrEmpty(response))
				{
					throw new IcnException("BIMserver did not respond", 10, "UploadFile");
				}

				CheckInResponse resp = JsonConvert.DeserializeObject(response, typeof(CheckInResponse), DeserializeSettings) as CheckInResponse;
				if (null == resp)
				{
					throw new IcnException("Could not deserialize the response from the BIMserver", 10, "UploadFile");
				}

				// if response is not an error it would be the process id which may or may not be running still?
				// resp is useless for finding the file, but we can get the SProject and its lastConcreteRevision should be the file we just checked in
				SProject project = GetProject(connectionData, projectId);
				if (null != project && project.parentId < 0)
				{
					fileId = project.lastRevisionId;
				}
			}
			catch (Exception ex)
			{
				throw new IcnException($"General error ({ex.Message})", 10, "CheckinFile");
			}

			return fileId;
		}

		internal void CheckoutFile(BimServerExchangeData connectionData, string path, int revisionId, int serializerId, bool checkout = true)
		{
			if (!checkout)
			{
				DownloadFile(connectionData, path, revisionId, serializerId);
				return;
			}

			if (revisionId < 0 || serializerId < 0 || string.IsNullOrEmpty(connectionData?.Token) || string.IsNullOrEmpty(path))
			{
				throw new IcnException("One or more parameters is invalid or not set to a reference", 10, "CheckoutFile");
			}

			try
			{
				CheckOutRequest req = new CheckOutRequest(connectionData.Token, revisionId, serializerId);

				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json))
				{
					throw new IcnException("Could not serialize UploadFileRequest", 10, "CheckinFile");
				}

				string response = SendRequestAsync(json, connectionData);
				if (string.IsNullOrEmpty(response))
				{
					throw new IcnException("BIMserver did not respond", 10, "CheckinFile");
				}

				//CheckOutResponse resp = JsonConvert.DeserializeObject(response, typeof(CheckOutResponse), DeserializeSettings) as CheckOutResponse;
				//if (null == resp)
				//{
				//	ExceptionResponse exception = JsonConvert.DeserializeObject(response, typeof(ExceptionResponse), DeserializeSettings) as ExceptionResponse;
				//	if (null != exception) throw new IcnException($"{exception.response.exception.message}", 10, "Checkout");
				//	throw new IcnException("Could not deserialize the response from the BIMserver", 10, "CheckinFile");
				//}
				// handle the bimserver returning an error/exception response

				// extract the data from the response and write it to the given file path (after decoding from Base64)
			}
			catch (Exception ex)
			{
				throw new IcnException($"General error ({ex.Message})", 10, "CheckoutFile");
			}
		}

		protected virtual void OnProgress(ProgressEventArgs e)
		{
			Progress?.Invoke(this, e);
		}

		internal List<SRevision> GetAllFilesOfProject(BimServerExchangeData connectionData, SProject project)
		{
			if (null == project || string.IsNullOrEmpty(connectionData?.Token))
			{
				throw new IcnException("One or more parameters are not set to a value or reference", 10, "GetAllFilesOfProject");
			}
			List<SRevision> res = new List<SRevision>();

			try
			{
				GetFilesRequest req = new GetFilesRequest(connectionData.Token, project.oid);

				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json))
				{
					throw new IcnException("Could not serialize UploadFileRequest", 10, "GetAllFilesOfProject");
				}

				string response = SendRequestAsync(json, connectionData);
				if (string.IsNullOrEmpty(response))
				{
					throw new IcnException("BIMserver did not respond", 10, "GetAllFilesOfProject");
				}

				GetFilesResponse resp = JsonConvert.DeserializeObject(response, typeof(GetFilesResponse), DeserializeSettings) as GetFilesResponse;
				if (null == resp?.response?.result)
				{
					// see if the login returned an error, if so throw an exception
					ExceptionResponse exception = JsonConvert.DeserializeObject(response, typeof(ExceptionResponse), DeserializeSettings) as ExceptionResponse;
					if (null != exception) throw new IcnException($"{exception.response.exception.message}", 10, "GetAllFilesOfProject");

					throw new IcnException("Could not deserialize the response from the BIMserver", 10, "GetAllFilesOfProject");
				}

				foreach (SRevision file in resp.response.result)
				{
					res.Add(file);
				}
			}
			catch (IcnException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new IcnException($"General error ({ex.Message})", 10, "GetAllFilesOfProject");
			}

			return res;
		}

		internal void TagUploadWithText(BimServerExchangeData connectionData, int fileId, string tag)
		{
			if (string.IsNullOrEmpty(connectionData?.Token) || fileId < 0 || string.IsNullOrEmpty(tag))
			{
				throw new IcnException("One or more parameters not set to a reference", 10, "TagUploadWithText");
			}

			// to actually tag the revision/file call ("ServiceInterface", "setRevisionTag") with token, fileId and tag
			try
			{
				SetRevisionTagRequest req = new SetRevisionTagRequest(connectionData.Token, fileId, tag);
				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json))
				{
					throw new IcnException("Could not serialize GetDeserialisersRequest", 10, "TagUploadWithText");
				}

				string response = SendRequest(json, connectionData);
				if (string.IsNullOrEmpty(response))
				{
					throw new IcnException("BIMserver did not respond", 10, "TagUploadWithText");
				}

				// tagging does not give a valid response though it may throw an exception
				ExceptionResponse exception = JsonConvert.DeserializeObject(response, typeof(ExceptionResponse), DeserializeSettings) as ExceptionResponse;
				if (null != exception)
				{
					if (!string.IsNullOrEmpty(exception.response?.exception?.message ?? string.Empty))
					{
						string err = exception.response?.exception?.message ?? "unknown error";
						throw new IcnException($"BIMserver reported an error: '{err}'", 10, "TagUploadWithText");
					}
				}
			}
			catch (Exception ex)
			{
				throw new IcnException($"General error ({ex.Message})", 10, "TagUploadWithText");
			}
		}

		internal void DownloadFile(BimServerExchangeData connectionData, string path, int revisionId, int serialiserId)
		{
			if (String.IsNullOrEmpty(connectionData?.Token) || revisionId < 0 || serialiserId < 0)
			{
				throw new IcnException("One or more parameters not set to a reference", 10, "DownloadFile");
			}

			string fpath = Path.GetDirectoryName(path);
			if (string.IsNullOrEmpty(fpath))
			{
				throw new IcnException($"File name '{path}' has no folder information", 10, "DownloadFile");
			}

			if (!Directory.Exists(fpath))
			{
				Directory.CreateDirectory(fpath);
			}

			try
			{
				DownloadRequest req = new DownloadRequest(connectionData.Token, revisionId, serialiserId);

				string json = JsonConvert.SerializeObject(req, SerializeSettings);
				if (string.IsNullOrEmpty(json))
				{
					throw new IcnException("Could not serialize DownloadFileRequest", 10, "DownloadFile");
				}

				string response = SendRequestAsync(json, connectionData);
				if (string.IsNullOrEmpty(response))
				{
					throw new IcnException("BIMserver did not respond", 10, "DownloadFile");
				}

				IdResponse resp = JsonConvert.DeserializeObject(response, typeof(IdResponse), DeserializeSettings) as IdResponse;
				if (null == resp)
				{
					ExceptionResponse exception = JsonConvert.DeserializeObject(response, typeof(ExceptionResponse), DeserializeSettings) as ExceptionResponse;
					if (null != exception) throw new IcnException($"{exception.response.exception.message}", 10, "DownloadFile");

					throw new IcnException("Could not deserialize the response from the BIMserver", 10, "DownloadFile");
				}

				long topicId = resp.response.result;
				if (topicId < 0)
				{
					throw new IcnException("Download is not initialised", 10, "DownloadFile");
				}

				// response is a TopicId of the download process
				// use the getDownloadData method to get the actual data
				GetDownloadDataRequest dwnl = new GetDownloadDataRequest(connectionData.Token, topicId);
				json = JsonConvert.SerializeObject(dwnl, SerializeSettings);
				if (string.IsNullOrEmpty(json))
				{
					throw new IcnException("Could not serialize GetDownloadDataRequest", 10, "DownloadFile");
				}

				response = SendGetRequestAsync(dwnl, json, serialiserId, connectionData);
				if (string.IsNullOrEmpty(response))
				{
					throw new IcnException("BIMserver did not respond", 10, "DownloadFile");
				}

				//DownloadDataResponse resp = JsonConvert.DeserializeObject(response, typeof(DownloadDataResponse), DeserializeSettings) as DownloadDataResponse;
				//if (null == resp)
				//{
				//	ExceptionResponse exception = JsonConvert.DeserializeObject(response, typeof(ExceptionResponse), DeserializeSettings) as ExceptionResponse;
				//	if (null != exception) throw new IcnException($"{exception.response.exception.message}", 10, "Checkout");
				//	throw new IcnException("Could not deserialize the response from the BIMserver", 10, "DownloadFile");
				//}


				// extract the data from the response and write it to the given file path (after decoding from Base64)
			}
			catch (IcnException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new IcnException($"General error ({ex.Message})", 10, "DownloadFile");
			}
		}

		internal void CancelDownload()
		{
			UploadStatus = "Cancelled";
			thread.Abort();
		}

		internal void StopMonitoringProgress()
		{
			MonitorProgress = false;
		}
	}
}
