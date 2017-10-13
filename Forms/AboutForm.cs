using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace BimServerExchange.Forms
{
	public partial class AboutForm : Form
	{
		public AboutForm(string revitVersion, string addinVersion)
		{
			InitializeComponent();
			InitialiseForm(revitVersion, addinVersion);
		}

		private void InitialiseForm(string revitVersion, string addinVersion)
		{
			if (string.IsNullOrEmpty(revitVersion)) revitVersion = "Unknown";
			if (string.IsNullOrEmpty(addinVersion)) addinVersion = "1.0.0.0002";

			String txt = DescriptionEdt.Text;
			DescriptionEdt.Text = string.Format(txt, revitVersion);

			VersionEdt.Text = addinVersion;

			string year = DateTime.Now.Year.ToString("0000");
			CopyrightEdt.Text = $@"©ICN Solutions b.v. {year}";

			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BimServerExchange.Runtime.LicenseAgreement.rtf"))
			{
				if (null != stream)
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						LicenseEdt.Rtf = reader.ReadToEnd();
					}
				}
			}
		}

	}
}
