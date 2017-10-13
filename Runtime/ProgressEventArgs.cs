using System;

namespace BimServerExchange.Runtime
{
	class ProgressEventArgs : EventArgs
	{
		public int Value { get; set; }

		public ProgressEventArgs(int val = 0)
		{
			Value = val;
		}
	}
}
