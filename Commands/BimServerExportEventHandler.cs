using System;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using BimServerExchange.Runtime;

namespace BimServerExchange.Commands
{
	class BimServerExportEventHandler : IExternalEventHandler
	{
		#region types and enums
		public enum Commands
		{
			kNoCommand = 0,
			kExport,
			kOpenProject,
			kLogin
		}
		#endregion types and enums

		#region fields
		private Commander _cmd;
		private Commands _command;
		#endregion fields

		#region properties
		internal Commander Cmd
		{
			get
			{
				if (null == _cmd) throw new IcnException("No Commander assigned", 10, "BimServerExportEventHandler");
				return _cmd;
			}
			set
			{
				if (null == value) return;
				_cmd = value;
			}
		}
		internal Commands Command
		{
			get
			{
				if (false == Enum.IsDefined(typeof(Commands), _command)) return Commands.kNoCommand;
				return _command;
			}
			set
			{
				_command = Commands.kNoCommand;
				if (false == Enum.IsDefined(typeof(Commands), value)) return;
				_command = value;
			}
		}
		#endregion properties

		#region ctors, dtor
		internal BimServerExportEventHandler(Commander commander)
		{
			_cmd = commander;
			_command = Commands.kNoCommand;
		}
		#endregion ctors, dtor

		#region IExternalEventHandler
		/// <summary>
		/// return a short description for the event handler
		/// </summary>
		/// <returns>String with the name/description of the event handler</returns>
		public string GetName()
		{
			return "FamilyManager_ZoekTool_Commands_handler";
		}

		/// <summary>
		/// Function is called by Revit when an event is raised
		/// </summary>
		/// <param name="app">Application that was active when the event was raised</param>
		public void Execute(UIApplication app)
		{
			if (null == app) return;

			// make sure the familymanager refers to the currently active document
			UIDocument orig = Cmd.UIDoc;
			Cmd.UIDoc = app.ActiveUIDocument;

			try
			{
				// Use Cmd.ZoekForm to access the form commander directly
				// Use Cmd.RevitAccess to access the revit database
				// Use Cmd.MainFilterForm to access the filter settings (should no be necessary)
				// Use Cmd.WebServies to access the ftp handler and such
				// not implemented yet
				switch (Command)
				{
					case Commands.kExport:
						Cmd.BimServerExport.ExportEvent();
						break;
					case Commands.kOpenProject:
						Cmd.BimServerExport.OpenProjectEvent();
						break;
					case Commands.kLogin:
						Cmd.BimServerExport.LoginEvent();
						break;
					case Commands.kNoCommand:
					default:
						throw new IcnException("External EventHandler triggered with no or unhandled Command", 10, "BimServerExportEventHandler");
				}
			}
			catch (IcnException iex)
			{
				iex.Display(@"Exception in BIMserver Export");
			}
			catch (Exception ex)
			{
				string mssg = ex.Message;
				MessageBox.Show(mssg, @"Exception in BIMserver Export");
			}

			Cmd.UIDoc = orig;
		}
		#endregion IExternalEventHandler
	}
}
