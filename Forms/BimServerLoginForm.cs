using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BimServerExchange.Commands;
using BimServerExchange.Runtime;
using Autodesk.Revit.UI;
using BimServerExchange.Objects;
using TextBox = System.Windows.Forms.TextBox;
using ComboBox = System.Windows.Forms.ComboBox;
// ReSharper disable JoinNullCheckWithUsage

namespace BimServerExchange.Forms
{
	public partial class BimServerLoginForm : Form
	{
		#region fields
		private UIDocument _uidoc;
		private Commander _cmd;
		private BimServerExchangeData _data;
		private int _bitfields;
		#endregion fields

		#region properties
		internal UIDocument UIDoc
		{
			get
			{
				if (null != _uidoc) return _uidoc;
				throw new IcnException("Property UIDoc not set to a reference", 10, "BimServerExchangeForm");
			}
			set
			{
				if (null == value) throw new IcnException("Attempt to clear the value of Property UIDoc", 10, "BimServerExchangeForm");
				_uidoc = value;
			}
		}
		internal Commander Cmd
		{
			get
			{
				if (null != _cmd) return _cmd;
				throw new IcnException("Property Cmd no set", 10, "BimServerExchangeForm");
			}
		}
		internal BimServerExchangeData Data
		{
			get
			{
				if (null != _data) return _data;

				// this initialises from the settings
				_data = new BimServerExchangeData();
				// the quickexport (simpleform) has an override for the bimserver project name from an xml file
				if (SimpleForm)
				{
					string key = UIDoc.Document.PathName;
					if (!string.IsNullOrEmpty(key) && Cmd.BimServerLogin.Projects.ContainsKey(key))
					{
						_data.ProjectName = Cmd.BimServerLogin.Projects[key];
					}
				}
				return _data;
			}
		}
		internal bool Connected
		{
			get { return (_bitfields & 1) != 0; }
			set { _bitfields = (_bitfields & ~1) | (value ? 1 : 0); }
		}

		internal bool SimpleForm
		{
			get { return (_bitfields & 2) != 0; }
			set { _bitfields = (_bitfields & ~2) | (value ? 2 : 0); }
		}
		#endregion properties

		#region ctors, dtor
		internal BimServerLoginForm(UIDocument uidoc, BimServerExchangeData data, bool simpleForm = true)
		{
			InitializeComponent();
			InitialiseForm(uidoc, data, simpleForm);
		}

		#region ctor support
		private void InitialiseForm(UIDocument uidoc, BimServerExchangeData data, bool simpleForm)
		{
			UIDoc = uidoc;
			Connected = false;
			SimpleForm = simpleForm;
			// commander must be created last as it uses the SimpleForm property for its initialisation
			_cmd = new Commander(this, UIDoc);
			// if current settings are passed in we should use them
			if (null != data)
			{
				Data.CopyFrom(data);
			}

			Cmd.BimServerLogin.ActivateIfcConfiguration(Data.UseStandardRevitExporter);
			Cmd.BimServerLogin.FillConfigurationCombobox(Cmd.IfcExporter.Exporter.Configurations);
			Cmd.BimServerLogin.ActivateIfcConfiguration(!Data.UseStandardRevitExporter);

			if (simpleForm)
			{
				IFCSettingsPnl.Visible = false;
				ProjectPnl.Visible = false;
				MinimumSize = new Size(MinimumSize.Width, MinimumSize.Height - 66 - 34);
				Size = new Size(Size.Width, Size.Height - 66 - 34);
				MaximumSize = new Size(MaximumSize.Width, MaximumSize.Height - 66 - 34);

				// try to connect to the bimserver (since we want to query it for existing projects)
				Cmd.BimServerLogin.Connect();

				string token = Data.Token;
				if (!string.IsNullOrEmpty(token))
				{
					Data.Token = token;
					Connected = true;

					List<SProject> projects = Cmd.ServerInterface.GetAllProjects(Data);
					Cmd.BimServerLogin.FillProjectsComboBox(projects);
					Cmd.BimServerLogin.SelectDefaultProject(Data.ProjectName);
				}
				else
				{
					Cmd.BimServerLogin.FillProjectsComboBox(null);
				}

				// this initialises from the Data
				InitialiseFormFromData();

				return;
			}

			ProjectPnl.Visible = false;
			MinimumSize = new Size(MinimumSize.Width, MinimumSize.Height - 34);
			Size = new Size(Size.Width, Size.Height - 34);
			MaximumSize = new Size(MaximumSize.Width, MaximumSize.Height - 34);

			List<string> formats = new List<string>(2) { "Ifc2x3tc1", "Ifc4" };
			// retry to select the ifc format now the required list has been set
			Cmd.BimServerLogin.FillFormatsCombobox(formats, "Ifc2x3tc1");
			//ProtocolRadio.SelectedIndex = Data.BimServerStreaming ? 0 : 1;
			StreamingRadio.Checked = Data.BimServerStreaming;
			SequentialRadio.Checked = !Data.BimServerStreaming;

			// this initialises from the Data
			InitialiseFormFromData();
		}

		private void InitialiseFormFromData(bool projectsOnly = false)
		{
			if (!projectsOnly)
			{
				UrlEdt.Text = Data.Url;
				PortEdt.Text = Data.Port;
				LoginNameEdt.Text = Data.LoginName;
				PasswordEdt.Text = Data.Password;

				IfcExportAddinChk.Checked = !Data.UseStandardRevitExporter;
				if (!Data.UseStandardRevitExporter) Cmd.BimServerLogin.SetDefaultConfiguration(Data.IfcConfiguration);
			}

			if (SimpleForm)
			{
				if (!Connected) return;

				List<SProject> projects = Cmd.ServerInterface.GetAllProjects(Data);
				Cmd.BimServerLogin.FillProjectsComboBox(projects);
				SelectProject(Data.ProjectName);
			}
			else
			{
				SelectIfcFormat(Data.IfcFormat);
			}
		}
		#endregion ctor support
		#endregion ctors, dtor

		#region private methods
		private void SelectIfcFormat(string ifcFormat)
		{
			IFCFormatCbx.SelectedIndex = -1;
			if (string.IsNullOrEmpty(ifcFormat)) return;

			int idx = 1;
			if (ifcFormat.IndexOf("streaming", StringComparison.InvariantCultureIgnoreCase) > 0) idx = 0;
			//ProtocolRadio.SelectedIndex = idx;
			StreamingRadio.Checked = idx == 0;
			SequentialRadio.Checked = idx != 0;

			//for (idx = 0; idx < IFCFormatCbx.Properties.Items.Count; idx++)
			for (idx = 0; idx < IFCFormatCbx.Items.Count; idx++)
			{
				//string nm = IFCFormatCbx.Properties.Items[idx] as string;
				string nm = IFCFormatCbx.Items[idx] as string;
				if (string.IsNullOrEmpty(nm)) continue;

				if (!ifcFormat.StartsWith(nm, StringComparison.InvariantCultureIgnoreCase)) continue;

				IFCFormatCbx.SelectedIndex = idx;
				break;
			}
		}

		private void SelectProject(string projectName)
		{
			ProjectsCbx.SelectedIndex = -1;
			if (string.IsNullOrEmpty(projectName)) return;

			//for (int idx = 0; idx < ProjectsCbx.Properties.Items.Count; idx++)
			for (int idx = 0; idx < ProjectsCbx.Items.Count; idx++)
			{
				//if (0 != string.Compare(projectName, ProjectsCbx.Properties.Items[idx] as string, StringComparison.InvariantCultureIgnoreCase)) continue;
				if (0 != string.Compare(projectName, ProjectsCbx.Items[idx] as string, StringComparison.InvariantCultureIgnoreCase)) continue;

				ProjectsCbx.SelectedIndex = idx;
				break;
			}
		}
		#endregion private methods

		private void SimpleButton_Click(object sender, EventArgs e)
		{
			Button btn = sender as Button;
			if (null == btn) return;

			if (ConnectBtn == btn)
			{
				if (!Cmd.BimServerLogin.ValidateInput()) return;

				Cmd.BimServerLogin.Connect();
				if (!Connected)
				{
					MessageBox.Show($@"Could not connect to the BIMserver {UrlEdt} with this username and password");
					return;
				}

				Cmd.BimServerLogin.WriteLoginSettings();
				DialogResult = DialogResult.OK;
				Close();
				return;
			}

			throw new IcnException($"SimpleButton '{btn.Name}' activated that has no code", 20, "BimServerLoginForm");
		}

		private void TextEdit_Changed(object sender, EventArgs e)
		{
			TextBox edt = sender as TextBox;
			if (null == edt) return;

			if (UrlEdt == edt)
			{
				if (0 == string.Compare(Data.Url, edt.Text, StringComparison.InvariantCultureIgnoreCase)) return;
				Data.Url = edt.Text;
				if (Cmd.BimServerLogin.ConnectDataIncomplete()) return;

				Cmd.BimServerLogin.Disconnect();
				Cmd.BimServerLogin.Connect();
				if (!Connected) return;

				if (SimpleForm) InitialiseFormFromData(true);
				return;
			}

			if (PortEdt == edt)
			{
				if (0 == string.Compare(Data.Port, edt.Text, StringComparison.InvariantCultureIgnoreCase)) return;
				Data.Port = edt.Text;
				if (Cmd.BimServerLogin.ConnectDataIncomplete()) return;

				Cmd.BimServerLogin.Disconnect();
				Cmd.BimServerLogin.Connect();
				if (!Connected) return;

				if (SimpleForm) InitialiseFormFromData(true);
				return;
			}

			if (LoginNameEdt == edt)
			{
				if (0 == string.Compare(Data.LoginName, edt.Text, StringComparison.InvariantCultureIgnoreCase)) return;
				Data.LoginName = edt.Text;
				if (Cmd.BimServerLogin.ConnectDataIncomplete()) return;

				Cmd.BimServerLogin.Disconnect();
				Cmd.BimServerLogin.Connect();
				if (!Connected) return;

				if (SimpleForm) InitialiseFormFromData(true);
				return;
			}

			if (PasswordEdt == edt)
			{
				if (0 == string.Compare(Data.Password, edt.Text, StringComparison.InvariantCultureIgnoreCase)) return;
				Data.Password = edt.Text;
				if (Cmd.BimServerLogin.ConnectDataIncomplete()) return;

				Cmd.BimServerLogin.Disconnect();
				Cmd.BimServerLogin.Connect();
				if (!Connected) return;

				if (SimpleForm) InitialiseFormFromData(true);
				return;
			}

			throw new IcnException($"TextEdit '{edt.Name}' activated that has no code", 20, "BimServerLoginForm");
		}

		private void ComboBox_SelectionChanged(object sender, EventArgs e)
		{
			ComboBox cbx = sender as ComboBox;
			if (null == cbx) return;

			if (ProjectsCbx == cbx)
			{
				int idx = cbx.SelectedIndex;
				//if (idx < 0 || idx >= cbx.Properties.Items.Count) return;
				if (idx < 0 || idx >= cbx.Items.Count) return;

				//string val = cbx.Properties.Items[idx] as string;
				string val = cbx.Items[idx] as string;
				if (string.IsNullOrEmpty(val)) return;

				Data.ProjectName = val;
				return;
			}

			if (IFCFormatCbx == cbx)
			{
				int idx = cbx.SelectedIndex;
				//if (idx < 0 || idx >= cbx.Properties.Items.Count) return;
				if (idx < 0 || idx >= cbx.Items.Count) return;

				//string val = cbx.Properties.Items[idx] as string;
				string val = cbx.Items[idx] as string;
				if (string.IsNullOrEmpty(val)) return;

				//if (ProtocolRadio.SelectedIndex == 1) val += " (streaming)";
				if (StreamingRadio.Checked) val += " (streaming)";
				Data.IfcFormat = val;

				return;
			}

			if (ExportConfigurationsCbx == cbx)
			{
				int idx = cbx.SelectedIndex;
				//if (idx < 0 || idx >= cbx.Properties.Items.Count) return;
				if (idx < 0 || idx >= cbx.Items.Count) return;

				//string val = cbx.Properties.Items[idx] as string;
				string val = cbx.Items[idx] as string;
				if (string.IsNullOrEmpty(val)) return;

				Data.IfcConfiguration = val;

				return;
			}

			throw new IcnException($"ComboBox '{cbx.Name}' activated that has no code", 20, "BimServerLoginForm");
		}

		/*
		private void RadioGroup_SelectionChanged(object sender, EventArgs e)
		{
			RadioGroup radio = sender as RadioGroup;
			if (null == radio) return;

			if (ProtocolRadio == radio)
			{
				int idx = IFCFormatCbx.SelectedIndex;
				if (idx < 0 || idx >= IFCFormatCbx.Properties.Items.Count) return;

				string val = IFCFormatCbx.Properties.Items[idx] as string;
				if (string.IsNullOrEmpty(val)) return;

				bool useStreaming = ProtocolRadio.SelectedIndex == 0;
				Data.BimServerStreaming = useStreaming;

				if (useStreaming) val += " (streaming)";
				Data.IfcFormat = val;

				return;
			}

			throw new IcnException($"RadioGroup '{radio.Name}' activated that has no code", 20, "BimServerLoginForm");
		}

	*/
		private void CheckEdit_Changed(object sender, EventArgs e)
		{
			CheckBox chk = sender as CheckBox;
			if (null == chk) return;

			if (IfcExportAddinChk == chk)
			{
				// the check reads 'ifc exporter addin installed', so we have to set he UseStandardRevitExporter to the inverse of that check
				Data.UseStandardRevitExporter = !chk.Checked;
				// the combobox gets enabled with the exporter is enabled
				Cmd.BimServerLogin.ActivateIfcConfiguration(chk.Checked);
				return;
			}

			throw new IcnException($"Checkedit '{chk.Name}' activated that has no code", 20, "BimServerLoginForm");
		}
	}
}