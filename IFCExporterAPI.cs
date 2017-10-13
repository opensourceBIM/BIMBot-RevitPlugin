using System;
using System.Collections.Generic;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BimServerExchange.Commands;
using BimServerExchange.Runtime;
// ReSharper disable UnusedMethodReturnValue.Local
// ReSharper disable UnusedMethodReturnValue.Global

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace BimServerExchange
{
	/// <summary>
	/// class is meant to be an internal API for exporting to IFC without requiring user interaction (unlike the command IfCExporter)
	/// </summary>
	public class IFCExporterAPI
	{
		#region fields
		private UIDocument _uidoc;
		private Transaction _trans;
		private Transaction _transactionOverride;
		private Commander _cmd;
		// configuration options
		private string _savePath;
		private List<string> _configurations;
		private int _bitfields;
		//private string _dummy;
		#endregion fields

		#region properties
		internal UIDocument UIDoc
		{
			get
			{
				if (null != _uidoc) return _uidoc;
				//_uidoc = Cmd.BimServerExchange.Form.UIDoc;

				//if (null != _uidoc) return _uidoc;
				throw new Exception("No property UIDoc set for IFCExporterAPI");
			}
			set
			{
				if (null != value) _uidoc = value;
			}
		}
		private Transaction Trans
		{
			get
			{
				if (null != _trans) return _trans;
				_trans = new Transaction(UIDoc.Document);
				return _trans;
			}
		}
		private Commander Cmd
		{
			get
			{
				if (null != _cmd) return _cmd;
				throw new IcnException("Property Cmd not set to a reference", 10, "IFCExporterAPI");
			}
			set
			{
				_cmd = value;
			}
		}

		// configuration options
		internal string SavePath
		{
			get
			{
				if (!string.IsNullOrEmpty(_savePath)) return _savePath;
				return Path.GetDirectoryName(UIDoc.Document.PathName) ?? string.Empty;
			}
			set
			{
				_savePath = value;
			}
		}

		internal string ConfigurationName
		{
			get { return Cmd.Data.IfcConfiguration; }
		}
		internal List<string> Configurations
		{
			get
			{
				if (null != _configurations) return _configurations;

				_configurations = new List<string>();
				IFCConfigurationManager configs = new IFCConfigurationManager();
				_configurations.AddRange(configs.GetConfigurationNames());
				return _configurations;
			}
		}
		public bool UseStandardRevitExporter
		{
			get
			{
				return Cmd.Data.UseStandardRevitExporter;
			}
		}
		#endregion properties

		#region ctors, dtor
		internal IFCExporterAPI(Commander cmd, UIDocument uidoc)
		{
			Cmd = cmd;
			UIDoc = uidoc;
		}
		#endregion ctors, dtor

		#region application methods
		internal bool Run()
		{
			// check if there is a UIDoc.Document
			if (UIDoc?.Document == null) return false;

			// check if there is a path to save to
			string path = Cmd.IfcExporter.GetSavePath();
			if (string.IsNullOrEmpty(path)) return Cmd.IfcExporter.ReportDocumentHasNoSavePath();

			string exportConfigName = Cmd.IfcExporter.GetExportConfiguration();

			StartTransaction("IFC Export");  // run the transaction in UIDoc.Document

			// write the IFC for the document
			Cmd.IfcExporter.SaveIfcDocument(path, exportConfigName??string.Empty, UIDoc.Document);

			CommitTransaction();

			// get a list of linked files and if they are found or not
			List<IFCExporterLinkedFileData> files = Cmd.IfcExporter.GetAllLinkedFiles();

			// if there are linked files that are also found force save the document then close it
			//string current = UIDoc.Document.PathName; <-- needed if we have to close the current document after all
			int failed = 0;
			bool res = true;
			if (null != files && files.Count > 0) res = RunLinkedFiles(files, ref failed);

			Cmd.IfcExporter.ReportResulToUser(path, failed);
			return res;
		}
		#endregion application methods

		#region private methods
		#region transactions
		private TransactionStatus StartTransaction(string label, Document ovr = null)
		{
			if (string.IsNullOrEmpty(label)) label = "IFCExporterAPI";

			if (null != ovr)
			{
				if (_transactionOverride != null || ovr.IsModifiable)
				{
					Cmd.ShowResult("IFCExporterAPI attempts to start a Transaction but one is already active");
					return TransactionStatus.Started;
				}

				_transactionOverride = new Transaction(ovr, label);
				return _transactionOverride.Start();
			}

			if (UIDoc.Document.IsModifiable)
			{
				Cmd.ShowResult("IFCExporterAPI attempts to start a Transaction but one is already active");
				return TransactionStatus.Started;
			}

			// if we are given a document it overrides the current one in UIDoc for the duration of this transaction
			return Trans.Start(label);
		}

		private void CommitTransaction()
		{
			if (null != _transactionOverride)
			{
				if (!_transactionOverride.HasStarted())
				{
					Cmd.ShowResult("IFCExporterAPI attempts to commit a Transaction but none is active");
					return;
				}
				_transactionOverride.Commit();
				_transactionOverride = null;
				return;
			}

			if (null == _trans || !UIDoc.Document.IsModifiable)
			{
				Cmd.ShowResult("IFCExporterAPI attempts to commit a Transaction but none is active");
				return;
			}

			Trans.Commit();
			_trans = null;
		}

		private void RollbackTransaction()
		{
			if (null != _transactionOverride)
			{
				if (!_transactionOverride.HasStarted())
				{
					Cmd.ShowResult("IFCExporterAPI attempts to roll back a Transaction but none is active");
					return;
				}
				_transactionOverride.Commit();
				_transactionOverride = null;
				return;
			}

			if (null == _trans || !UIDoc.Document.IsModifiable)
			{
				Cmd.ShowResult("IFCExporterAPI attempts to roll back a Transaction but none is active");
				return;
			}

			Trans.RollBack();
			_trans = null;
		}
		#endregion transactions

		/// <summary>
		/// Helper method to handle all linked files
		/// </summary>
		/// <param name="files">List of linked file data</param>
		/// <param name="ctSource">CancellationToken allows the exporter to detect when the user cancelled the export</param>
		/// <param name="failed">reference parameter for the failed state</param>
		/// <returns></returns>
		private bool RunLinkedFiles(List<IFCExporterLinkedFileData> files, ref int failed)
		{
			failed = 0;
			if (null == files || files.Count <= 0) return true;

			// the following part is needed only when we close the active document (which we are not currently doing)
			//if (files.Any(x => x.Found))
			//{
			//	// there is at least one linked file
			//	UIDoc.Document.Save();
			//}

			// loop through the list of linked files and save them (if found) or log them (if not found)
			for (int count = 0; count < files.Count; count++)
			{
				IFCExporterLinkedFileData data = files[count];
				if (!data.Found)
				{
					Cmd.ShowResult($"Linked file '{data.Path}' could not be found and is not exported to IFC");
					failed += 1;
					continue;
				}

				// we have a linked file. It should not be necessary to close the current project to open a new one
				//Cmd.IfcExporterApi.CloseDocument()
				UIDocument uidoc = UIDoc.Application.OpenAndActivateDocument(data.Path);
				if (null == uidoc?.Document)
				{
					Cmd.ShowResult($"Linked file '{data.Path}' could not be activated for export");
					failed += 1;
					continue;
				}

				// this saves the IFC in the same location as the main file with the name of the linked file
				// this will overwrite existing files, especially if the linked file appears more than once in the list
				// but automatically renumbering is not a solution either as that creates new files with each run of this exporter
				try
				{
					StartTransaction($"IFC Export {count + 1}", uidoc.Document);   // run this transaction in the new document

					string lpath = Path.Combine(SavePath, Path.GetFileName(data.Path) ?? $"LinkedFile_{count}");
					Cmd.IfcExporter.SaveIfcDocument(lpath, ConfigurationName, uidoc.Document);

					CommitTransaction();
				}
				catch (Exception ex)
				{
					failed += 1;
					RollbackTransaction();
					Cmd.ShowResult($"Failed to export linked file '{data.Path}' to IFC ({ex.Message})");
				}
			}

			// if the docuemnt was closed open it again
			return false;
		}
		#endregion private methods
	}
}
