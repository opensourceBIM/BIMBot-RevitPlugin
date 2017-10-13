using System.Collections.Generic;
using System.IO;
using System.Linq;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Runtime
{
	// ReSharper disable once UnusedMember.Global
	public class IFCConfigurationManager
	{
		#region fields
		private Runtime.IFCExporterConfigurations _configurations;
		#endregion fields

		#region properties
		private Runtime.IFCExporterConfigurations Configurations
		{
			get
			{
				if (null != _configurations) return _configurations;

				_configurations = new IFCExporterConfigurations();
				_configurations.AddBuiltInConfigurations();
				_configurations.AddProjectConfigurations();
				// read configurations from the saved configuration location (3B specific extension)
				string fdir = string.Empty;//DrieBInstellingenManager.Global.GetValue("IFCTools", "SavedConfigurationsFolder");
				if (!string.IsNullOrEmpty(fdir) && Directory.Exists(fdir))
				{
					_configurations.AddSavedConfigurations(fdir);
				}
				return _configurations;
			}
		}

		public Runtime.IFCExporterConfiguration this[string name]
		{
			get
			{
				if (string.IsNullOrEmpty(name)) return null;
				if (!Configurations.Dict.ContainsKey(name)) return null;

				return Configurations.Dict[name];
			}
		}

		public List<string> ConfigurationNames
		{
			get
			{
				List<string> keys = Configurations.Dict.Keys.ToList();
				return keys;
			}
		}
		#endregion properties

		#region ctors, dtor
		public IFCConfigurationManager()
		{
		}
		#endregion ctors, dtor

		#region application methods
		public bool SaveToFile(string name)
		{
			if (string.IsNullOrEmpty(name)) return false;
			if (!Configurations.Dict.ContainsKey(name)) return false;

			string fdir = string.Empty; //DrieBInstellingenManager.Global.GetValue("IFCTools", "SavedConfigurationsFolder");
			if (string.IsNullOrEmpty(fdir) || !Directory.Exists(fdir)) return false;

			string path = Path.Combine(fdir, name + ".3bic");
			if (File.Exists(path))
			{
				string bpath = Path.ChangeExtension(path, "bak");
				if (File.Exists(bpath)) File.Delete(bpath);
				File.Move(path, bpath);
				File.Delete(path);
			}
			return Configurations.Dict[name].SaveToFile(path);
		}

		public bool ConfigurationExists(string name)
		{
			if (string.IsNullOrEmpty(name)) return false;

			return Configurations.Dict.ContainsKey(name);
		}

		public List<string> GetConfigurationNames()
		{
			List<string> keys = Configurations.Dict.Keys.ToList();
			return keys;
		}
		#endregion application methods
	}
}
