using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BimServerExchange.Runtime;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace BimServerExchange.Commands
{
	internal class CmdIfcExporterAPI
	{
		#region fields
		private Commander _cmd;
		private UIDocument _uidoc;
		private IFCExporterAPI _exporter;
		#endregion fields

		#region properties
		private Commander Cmd
		{
			get
			{
				if (null != _cmd) return _cmd;
				throw new IcnException("Property Cmd not set to a reference", 10, "CmdServerInterface");
			}
			set
			{
				_cmd = value;
			}
		}

		private UIDocument UIDoc
		{
			get
			{
				if (null != _uidoc) return _uidoc;

				throw new IcnException("Propety UIDoc not set to a reference", 10, "CmdIfcExporter");
			}
			set
			{
				if (null == value) return;

				_uidoc = value;
			}
		}
		internal IFCExporterAPI Exporter
		{
			get
			{
				if (null != _exporter) return _exporter;

				_exporter = new IFCExporterAPI(Cmd, UIDoc);
				return _exporter;
			}
		}
		#endregion properties

		#region ctors, dtor
		internal CmdIfcExporterAPI(Commander cmd, UIDocument uidoc)
		{
			//Exporter = exporter;
			Cmd = cmd;
			UIDoc = uidoc;
		}
		#endregion ctors, dtor

		#region private methods
		private List<RevitLinkInstance> GetLinkInstances()
		{
			// verzamel de te exporteren links
			List<string> linkedProjects = ElementCollectionHelper.GetLinkedFiles(UIDoc.Application, false);
			List<RevitLinkInstance> instances = ElementCollectionHelper.GetAllProjectElements(UIDoc.Document)
					.OfType<RevitLinkInstance>()
					.Where(c => c.Name.ToLower().Contains(".rvt"))
					.ToList();

			for (int i= instances.Count - 1; i>= 0; i--)
			{
				RevitLinkInstance inst = instances[i];
				if (null == inst)
				{
					instances.RemoveAt(i);
					continue;
				}

				ElementId id = inst.GetTypeId();
				ElementType etype = UIDoc.Document.GetElement(id) as ElementType;
				string instName = etype?.Name;
				if (string.IsNullOrEmpty(instName))
				{
					instances.RemoveAt(i);
					continue;
				}

				if (!linkedProjects.Contains(instName, StringComparer.InvariantCultureIgnoreCase))
				{
					instances.RemoveAt(i);
				}
			}

			if (instances.Count > 0)
			{
				StringBuilder builder = new StringBuilder();
				builder.AppendLine("De following linked files are exported:");
				foreach (RevitLinkInstance rli in instances)
				{
					builder.AppendLine(rli.Name);
				}

				Cmd.ShowResult(builder.ToString());
			}

			return instances;
		}
		#endregion private methods

		#region application methods
		/// <summary>
		/// Returns the save path for the IFC document based on the active document
		/// </summary>
		/// <returns>The path to save the IFC file to</returns>
		internal string GetSavePath()
		{
			//string path = Exporter.UIDoc?.Document.PathName;
			string path = Exporter.SavePath;
			if (string.IsNullOrEmpty(path)) return path;
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);

			string fname = Path.GetFileName(UIDoc.Document.PathName);
			if (string.IsNullOrEmpty(fname)) fname = "IFCExport.ifc";
			path = Path.Combine(path, fname);

			return Path.ChangeExtension(path, "IFC");
		}

		/// <summary>
		/// save the given document to the given path (may be the document save path as the extension will be forced to ifc)
		/// </summary>
		/// <param name="path">Path to save the ifc document to</param>
		/// <param name="exportConfiguration">Optional name of the IFC export configuration to use. Ignored if null, empty or unknown</param>
		/// <param name="document">Revit Document to save to IFC</param>
		internal void SaveIfcDocument(string path, string exportConfiguration, Document document)
		{
			if (null == document || string.IsNullOrEmpty(path))
			{
				throw new Exception("Missing parameter(s) in call to SaveIfcDocument");
			}

			IFCExporterConfiguration config = null;
			if (!string.IsNullOrEmpty(exportConfiguration))
			{
				IFCConfigurationManager configs = new IFCConfigurationManager();
				config = configs[exportConfiguration];
			}

			string folder = Path.GetDirectoryName(path);
			if (string.IsNullOrEmpty(folder))
			{
				throw new Exception("Missing directory name in call to SaveIfcDocument");
			}

			// run this in a separate thread so it at least will keep the GUI responsive
			//Thread thread = new Thread(() =>
			//{
				string fname = Path.GetFileNameWithoutExtension(path);
				if (string.IsNullOrEmpty(fname))
				{
					throw new Exception("Missing file name in call to SaveIfcDocument");
				}

				IFCExportOptions ifcOptions = new IFCExportOptions {FileVersion = IFCVersion.IFC2x3};
				ifcOptions.WallAndColumnSplitting = false;
				ifcOptions.SpaceBoundaryLevel = 1;

				if (null != config)
				{
					config.UpdateOptions(ElementId.InvalidElementId, document, ref ifcOptions);
				}

			// get the revit form and set its cursor to busy
			System.Windows.Forms.Control form = System.Windows.Forms.Control.FromHandle(Process.GetCurrentProcess().MainWindowHandle);
			if (null != form) form.Cursor = Cursors.WaitCursor;
			RevitStatusText.Set("Exporting Revit Project to IFC");
			Cmd.ShowResult("Exporting Revit Project to IFC");
			Application.DoEvents();

			// revit doesn't allow the export to run in a different thread
			document.Export(folder, fname + ".ifc", ifcOptions);

			//});

			//thread.IsBackground = true;
			//thread.Priority = ThreadPriority.Highest;
			//thread.Start();

			//// get the revit form and set its cursor to busy
			//System.Windows.Forms.Control form = System.Windows.Forms.Control.FromHandle(Process.GetCurrentProcess().MainWindowHandle);
			//if (null != form) form.Cursor = Cursors.WaitCursor;
			//RevitStatusText.Set("Exporting Revit Project to IFC");

			//while (thread.IsAlive)
			//{
			//	// check for progress and display it
			//	Application.DoEvents();
			//}

			if (null != form) form.Cursor = Cursors.Default;
			//if (null != ctSource && ctSource.IsCancellationRequested)
			if (!File.Exists(Path.Combine(folder, fname + ".ifc")))
			{
				RevitStatusText.Set("Export to IFC cancelled");
			}
			else
			{
				RevitStatusText.Set("Export to IFC completed");
			}
		}

		/// <summary>
		/// Error report
		/// </summary>
		/// <returns>false</returns>
		internal bool ReportDocumentHasNoSavePath()
		{
			Cmd.ShowResult("The command cannnot be executed. The revit project must be saved first");

			return false;
		}

		internal List<IFCExporterLinkedFileData> GetAllLinkedFiles()
		{
			// this assumes that GetLinkInstances returns both found and missing instances otherwise we must write a different method to find the missing ones
			List<RevitLinkInstance> instances = GetLinkInstances();
			if (null == instances || instances.Count <= 0) return null;

			XYZ viewDirection = UIDoc.Document.ActiveView.ViewDirection;
			XYZ rightDirection = UIDoc.Document.ActiveView.RightDirection;

			List<IFCExporterLinkedFileData> res = new List<IFCExporterLinkedFileData>();
			foreach (RevitLinkInstance inst in instances)
			{
				// dit is een extension method?
				Document doc = inst.GetLinkDocument(UIDoc.Application);
				if (null == doc) continue;

				string path = doc.PathName;
				if (string.IsNullOrEmpty(path)) continue;
				IFCExporterLinkedFileData data = new IFCExporterLinkedFileData(path);

				res.Add(data);
				if (!data.Found) continue;

				List<BasePoint> points = ElementCollectionHelper.GetAllProjectElements(doc).OfType<BasePoint>().ToList();
				if (points.Count <= 0) continue;

				Transform toWcs = inst.GetTotalTransform();

				foreach (BasePoint point in points)
				{
					if (point.Category.Name != "Project Base Point") continue;

					XYZ pOrg;
					double hoek = toWcs.BasisX.AngleOnPlaneTo(rightDirection, viewDirection);
					if (Math.Abs(Math.Round(hoek, 2)) > 1e-6)
					{
						//voor hoeken gaan we de survey punten op elkaar leggen. Dat verrekenen we verder op
						pOrg = new XYZ(0, 0, 0);
					}
					else
					{
						//FamilyParametersHandler fph = new FamilyParametersHandler(point);

						//ParameterHandler ph = fph.Get(BuiltInParameter.BASEPOINT_EASTWEST_PARAM);
						//double x = ph?.AsRawDouble() ?? 0.0;
						Parameter ph = point.get_Parameter(BuiltInParameter.BASEPOINT_EASTWEST_PARAM);
						double x = ph?.AsDouble() ?? 0.0;

						//ph = fph.Get(BuiltInParameter.BASEPOINT_NORTHSOUTH_PARAM);
						//double y = ph?.AsRawDouble() ?? 0.0;
						ph = point.get_Parameter(BuiltInParameter.BASEPOINT_NORTHSOUTH_PARAM);
						double y = ph?.AsDouble() ?? 0.0;

						//ph = fph.Get(BuiltInParameter.BASEPOINT_ELEVATION_PARAM);
						//double z = ph?.AsRawDouble() ?? 0.0;
						ph = point.get_Parameter(BuiltInParameter.BASEPOINT_ELEVATION_PARAM);
						double z = ph?.AsDouble() ?? 0.0;

						pOrg = new XYZ(x, y, z);
					}

					XYZ p = toWcs.OfPoint(pOrg);
					XYZ pos = p - 2 * pOrg;

					data.Angle = hoek;
					data.Position = pos;
					break;
				}
			}

			return res;
		}

		internal void ReportResulToUser(string path, int failed)
		{
			string fname;
			if (string.IsNullOrEmpty(path))
			{
				fname = "Unknown project";
			}
			else
			{
				fname = Path.GetFileNameWithoutExtension(path);
				if (string.IsNullOrEmpty(fname)) fname = path;
			}
			if (failed > 0)
			{
				string meerv1 = failed != 1 ? "s" : "";
				//string meerv2 = failed != 1 ? "den" : "";
				Cmd.ShowResult($"{failed} linked file{meerv1} in project '{fname}' could not be exported to IFC");
			}
			else
			{
				Cmd.ShowResult($"Project '{fname}' is exported to IFC");
			}
		}
		#endregion application methods

		public string GetExportConfiguration()
		{
			return Exporter.ConfigurationName;
		}
	}
}
