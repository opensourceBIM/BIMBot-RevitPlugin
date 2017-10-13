using System;
using System.Globalization;
using System.IO;
using System.Net;
using Autodesk.Revit.DB;

// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Runtime
{
	internal class BimServerExchangeData
	{
		#region fields
		private string _url;
		private int _port;
		private string _loginName;
		private string _password;
		private string _ifcFormat;
		private string _projectname;
		private string _token;
		private int _bitfields;
		private string _ifcConfiguration;
		#endregion fields

		#region properties
		internal string Url
		{
			get
			{
				if (string.IsNullOrEmpty(_url)) return "http://localhost";
				return _url;
			}
			set
			{
				_url = value;
			}
		}
		internal string Port
		{
			get
			{
				if (_port <= 0) return "8082";
				return _port.ToString(CultureInfo.InvariantCulture);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					_port = 0;
					return;
				}
				int.TryParse(value, out _port);
			}
		}
		internal string LoginName
		{
			get
			{
				if (string.IsNullOrEmpty(_loginName)) return string.Empty;
				return _loginName;
			}
			set
			{
				_loginName = value;
			}
		}
		internal string Password
		{
			get
			{
				if (string.IsNullOrEmpty(_password)) return string.Empty;
				return _password;
			}
			set
			{
				_password = value;
			}
		}
		internal string IfcFormat
		{
			get
			{
				if (string.IsNullOrEmpty(_ifcFormat)) return string.Empty;
				return _ifcFormat;
			}
			set
			{
				_ifcFormat = value;
			}
		}
		internal string ProjectName
		{
			get
			{
				if (string.IsNullOrEmpty(_projectname)) return string.Empty;
				return _projectname;
			}
			set
			{
				_projectname = value;
			}
		}
		internal string Token
		{
			get
			{
				if (string.IsNullOrEmpty(_token)) return string.Empty;
				return _token;
			}
			set
			{
				_token = value;
			}
		}
		public bool UseStandardRevitExporter
		{
			get
			{
				return (_bitfields & 1) == 1;
			}
			set
			{
				_bitfields |= 1;
				if (!value) _bitfields ^= 1;
			}
		}
		public bool BimServerStreaming
		{
			get
			{
				return (_bitfields & 4) == 0;
			}
			set
			{
				_bitfields |= 4;
				if (value) _bitfields ^= 4;
			}
		}
		internal string IfcConfiguration
		{
			get
			{
				if (string.IsNullOrEmpty(_ifcConfiguration)) return string.Empty;
				return _ifcConfiguration;
			}
			set
			{
				_ifcConfiguration = value;
			}
		}
		#endregion properties

		#region ctors, dtor
		internal BimServerExchangeData()
		{
			_url = string.Empty;
			_port = 0;
			_loginName = string.Empty;
			_password = string.Empty;
			_token = string.Empty;

			// if we keep track of the previous setting we must retreive that data here
			// this probably only can be done through Properties.Settings.Default
			LoadFromProperties(false);
		}

		internal void CopyFrom(BimServerExchangeData data)
		{
			_url = string.Empty;
			_port = 0;
			_loginName = string.Empty;
			_password = string.Empty;
			_token = string.Empty;
			_projectname = string.Empty;
			_ifcFormat = string.Empty;
			_ifcConfiguration = string.Empty;
			_bitfields = 0;
			if (null == data) return;

			Url = data.Url;
			Port = data.Port;
			LoginName = data.LoginName;
			Password = data.Password;
			Token = data.Token;
			IfcFormat = data.IfcFormat;
			ProjectName = data.ProjectName;
			IfcConfiguration = data.IfcConfiguration;
			_bitfields = data._bitfields;
		}
		#endregion ctors, dtor

		#region IO
		internal void SaveToProperties(bool saveProjectName, string path = null)
		{
			if (string.IsNullOrEmpty(path))
			{
				Properties.Settings.Default.BimServerUrl = Url;
				Properties.Settings.Default.BimServerPort = Port;
				Properties.Settings.Default.BimServerLogin = LoginName;
				Properties.Settings.Default.BimServerPwd = Password;
				Properties.Settings.Default.BimServerIFCFormat = IfcFormat;
				if (saveProjectName) Properties.Settings.Default.BimServerProject = ProjectName;
				Properties.Settings.Default.BimServerOptions = _bitfields;
				Properties.Settings.Default.BimServerExportConfig = IfcConfiguration;
				Properties.Settings.Default.Save();
				return;
			}

			if (File.Exists(path))
			{
				string bpath = Path.ChangeExtension(path, "bak");
				if (File.Exists(bpath)) File.Delete(bpath);
				File.Move(path, bpath);
			}

			using (StreamWriter writer = new StreamWriter(path))
			{
				writer.WriteLine(";BIMserver Exchange");
				writer.WriteLine(";");
				writer.WriteLine($"BimServerUrl={Url}");
				writer.WriteLine($"BimServerPort={Port}");
				writer.WriteLine($"BimServerLogin={LoginName}");
				writer.WriteLine($"BimServerPwd={Password}");
				writer.WriteLine($"BimServerIFCFormat={IfcFormat}");
				if (saveProjectName) writer.WriteLine($"BimServerProject={ProjectName}");
				writer.WriteLine($"BimServerOptions={_bitfields}");
				writer.WriteLine($"BimServerExportConfig={IfcConfiguration}");
			}
		}

		internal void LoadFromProperties(bool loadProjectName, string path = null)
		{
			if (string.IsNullOrEmpty(path) || !File.Exists(path))
			{
				Url = Properties.Settings.Default.BimServerUrl;
				Port = Properties.Settings.Default.BimServerPort;
				LoginName = Properties.Settings.Default.BimServerLogin;
				Password = Properties.Settings.Default.BimServerPwd;
				IfcFormat = Properties.Settings.Default.BimServerIFCFormat;
				if (loadProjectName) ProjectName = Properties.Settings.Default.BimServerProject;
				_bitfields = Properties.Settings.Default.BimServerOptions;
				IfcConfiguration = Properties.Settings.Default.BimServerExportConfig;
				return;
			}


			using (StreamReader reader = new StreamReader(path))
			{
				while (!reader.EndOfStream)
				{
					string val = reader.ReadLine();
					if (string.IsNullOrEmpty(val) || val.StartsWith(";", StringComparison.InvariantCultureIgnoreCase)) continue;

					string[] splits = val.Split(new[] {"="}, StringSplitOptions.RemoveEmptyEntries);
					if (splits.Length < 2) return;

					if (0 == string.Compare(val, "BimServerUrl", StringComparison.InvariantCultureIgnoreCase)) Url = val;
					if (0 == string.Compare(val, "BimServerPort", StringComparison.InvariantCultureIgnoreCase)) Port = val;
					if (0 == string.Compare(val, "BimServerLogin", StringComparison.InvariantCultureIgnoreCase)) LoginName = val;
					if (0 == string.Compare(val, "BimServerPwd", StringComparison.InvariantCultureIgnoreCase)) Password = val;
					if (0 == string.Compare(val, "BimServerIFCFormat", StringComparison.InvariantCultureIgnoreCase)) IfcFormat = val;
					if (loadProjectName)
					{
						if (0 == string.Compare(val, "BimServerProject", StringComparison.InvariantCultureIgnoreCase)) ProjectName = val;
					}
					if (0 == string.Compare(val, "BimServerOptions", StringComparison.InvariantCultureIgnoreCase))
					{
						if (int.TryParse(val, out int ival)) _bitfields = ival;
					}
					if (0 == string.Compare(val, "BimServerExportConfig", StringComparison.InvariantCultureIgnoreCase)) IfcConfiguration = val;
				}
			}

		}

		internal void ResetProperties()
		{
			Properties.Settings.Default.BimServerUrl = string.Empty;
			Properties.Settings.Default.BimServerPort = string.Empty;
			Properties.Settings.Default.BimServerLogin = string.Empty;
			Properties.Settings.Default.BimServerPwd = string.Empty;
			Properties.Settings.Default.BimServerIFCFormat = string.Empty;
			Properties.Settings.Default.BimServerProject = string.Empty;
			Properties.Settings.Default.BimServerOptions = 1;
			Properties.Settings.Default.BimServerExportConfig = string.Empty;
			Properties.Settings.Default.Save();
		}
		#endregion IO
	}
}
