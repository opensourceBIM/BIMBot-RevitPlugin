using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Runtime
{
	public class RevitStatusText
	{
		#region statustext
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern int SetWindowText(IntPtr hWnd, string lpString);

		[DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		/// <summary>
		/// Display the given text at the revit status line
		/// </summary>
		/// <param name="text">string with the text to display</param>
		public static void Set(string text)
		{
			IntPtr mMainWndFromHandle = IntPtr.Zero;
			Process[] processes = Process.GetProcessesByName("Revit");

			if (0 < processes.Length)
			{
				mMainWndFromHandle = processes[0].MainWindowHandle;
			}
			IntPtr revitStatusBar = FindWindowEx(mMainWndFromHandle, IntPtr.Zero, "msctls_statusbar32", "");

			if (revitStatusBar != IntPtr.Zero)
			{
				SetWindowText(revitStatusBar, text);
			}
		}
		#endregion statustext
	}
}
