using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace BimServerExchange.Runtime
{
	internal class KnownProjects : IDisposable
	{
		#region types and enums
		private static string RootElementXml = "Projects";
		private static string ElementXml = "Project";
		private static string BimServerXml = "BimServer";
		private static string RevitProjectXml = "RevitProject";
		private static string RevitNameXml = "Name";
		#endregion types and enums

		#region fields
		private string _savePath;
		private Dictionary<string, string> _projects;
		private List<string> _keys;
		private bool _modified;

		#region IDisposable
		private bool _disposed;
		#endregion IDisposable
		#endregion fields

		#region properties
		private Dictionary<string, string> ProjectsDict
		{
			get
			{
				if (null != _projects) return _projects;

				_projects = new Dictionary<string, string>();
				ReadProjects();
				_modified = false;
				return _projects;
			}
		}

		internal List<string> Projects
		{
			get
			{
				if (null != _keys) return _keys;

				_keys = new List<string>();
				foreach (string key in ProjectsDict.Keys) _keys.Add(key);

				return _keys;
			}
		}

		internal string this[string key]
		{
			get
			{
				if (string.IsNullOrEmpty(key) || !ProjectsDict.ContainsKey(key)) return null;

				return ProjectsDict[key] ?? string.Empty;
			}
			set
			{
				if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value)) return;

				if (!ProjectsDict.ContainsKey(key))
				{
					ProjectsDict.Add(key, value);
					Projects.Add(key);
					_modified = true;
					return;
				}

				ProjectsDict[key] = value;
				_modified = true;
			}
		}

		private string SavePath
		{
			get
			{
				if (!string.IsNullOrEmpty(_savePath)) return _savePath;

				_savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BimServerEx", "Projects.xml");
				return _savePath;
			}
		}
		#endregion properties

		#region ctors, dtor
		internal KnownProjects()
		{
			_modified = false;
			_disposed = false;
		}

		~KnownProjects()
		{
			Dispose(false);
		}

		#region IDisposable
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			// if already disposed do nothing (the object is in an uncertain state and may be partially destroyed)
			if (_disposed) return;
			// only dispose once
			_disposed = true;

			if (disposing)
			{
				// Dispose is called from code (this is the normal operation with a using() block)
			}

			// any data that needs to be cleaned up regardless of how this object is destroyed goes here

			// write the Xml here
			try
			{
				if (null != _projects) WriteProjects();
			}
			catch (Exception ex)
			{
				// we can't report this error anywhere else. The dialog is gone or about to go
				MessageBox.Show($@"Saving projects file '{SavePath}' failed ({ex.Message})");
			}

			// repeat for other DrieBInstellingenData
			if (null != _projects)
			{
				_projects.Clear();
			}

			// do not destroy any nested object until after this point. This minimises the risk that
			// Write() throws an exception (which will be silently swallowed but prevents the xml from
			// being written. Also null/zero all fields so the Write() method can better detect they can
			// no longer be accessed
			_projects = null;
		}

		#endregion IDisposable
		#endregion ctors, dtor

		#region application methods
		internal void Add(string revitProject, string bimProject)
		{
			// input validation
			if (string.IsNullOrEmpty(revitProject))
			{
				throw new IcnException("Parameter revitProject is not set to a value or reference", 20, "KnownProjects.Add");
			}

			if (!File.Exists(revitProject))
			{
				throw new IcnException($"Parameter revitProject '{revitProject}' does not exist", 20, "KnownProjects.Add");
			}

			if (string.IsNullOrEmpty(bimProject))
			{
				throw new IcnException("Parameter bimProject is not set to a value or reference", 20, "KnownProjects.Add");
			}

			if (!ProjectsDict.ContainsKey(revitProject))
			{
				ProjectsDict.Add(revitProject, bimProject);
				Projects.Add(revitProject);
				_modified = true;
				return;
			}

			ProjectsDict[revitProject] = bimProject;
			_modified = true;
		}

		internal bool ContainsKey(string revitProject)
		{
			if (string.IsNullOrEmpty(revitProject)) return false;

			return ProjectsDict.ContainsKey(revitProject);
		}

		internal void Save(bool force = false)
		{
			if (force) _modified = true;
			WriteProjects();
		}
		#endregion application methods

		#region private methods
		#region IO
		private void ReadProjects()
		{
			// get the path to the storage file
			string path = SavePath;
			if (!File.Exists(path)) return;

			// read the file and extract all revit project/bimserver project pairs
			try
			{
				XmlDocument xdoc = new XmlDocument();
				xdoc.Load(path);

				XmlElement root = xdoc.DocumentElement;
				if (null == root || 0 != string.Compare(root.Name, RootElementXml, StringComparison.InvariantCultureIgnoreCase)) return;

				XmlNodeList nodes = root.SelectNodes(ElementXml);
				if (null == nodes || nodes.Count <= 0) return;

				foreach (XmlNode node in nodes)
				{
					XmlElement elem = node as XmlElement;
					if (null == elem) continue;

					string revitProject = elem.GetAttribute(RevitProjectXml);
					if (string.IsNullOrEmpty(revitProject) || ProjectsDict.ContainsKey(revitProject)) continue;

					string bimServerProject = elem.GetAttribute(BimServerXml);
					if (string.IsNullOrEmpty(bimServerProject)) continue;

					ProjectsDict.Add(revitProject, bimServerProject);
				}
			}
			catch (Exception ex)
			{
				throw new IcnException($"Error reading projects file '{path}' ({ex.Message})", 20, "KnownProjects");
			}

			_modified = false;
		}

		private void WriteProjects()
		{
			if (!_modified) return;

			string path = SavePath;
			if (File.Exists(path)) File.Delete(path);

			string fdir = Path.GetDirectoryName(path);
			if (string.IsNullOrEmpty(fdir)) return;
			if (!Directory.Exists(fdir)) Directory.CreateDirectory(fdir);

			XmlDocument xdoc = new XmlDocument();
			XmlElement root = xdoc.CreateElement(RootElementXml);
			xdoc.AppendChild(root);

			foreach (string revitProject in ProjectsDict.Keys)
			{
				string bimServerProject = ProjectsDict[revitProject];
				if (string.IsNullOrEmpty(bimServerProject)) continue;

				string leaf = Path.GetFileNameWithoutExtension(revitProject);

				XmlElement elem = xdoc.CreateElement(ElementXml);
				elem.SetAttribute(BimServerXml, bimServerProject);
				elem.SetAttribute(RevitProjectXml, revitProject);
				elem.SetAttribute(RevitNameXml, leaf);
				root.AppendChild(elem);
			}

			xdoc.Save(path);
			_modified = false;
		}
		#endregion IO
		#endregion private methods
	}
}
