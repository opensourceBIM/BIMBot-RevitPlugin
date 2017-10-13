using System.Windows.Forms;

namespace BimServerExchange.Forms
{
	public partial class ConfirmationForm : Form
	{
		public ConfirmationForm(string message)
		{
			InitializeComponent();
			InitialiseForm(message);
		}

		private void InitialiseForm(string message)
		{
			if (string.IsNullOrEmpty(message)) return;

			MessageMemo.Text = message.Replace('|', '\n');
		}
	}
}