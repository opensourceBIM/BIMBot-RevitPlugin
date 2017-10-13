using System;
using System.IO;
using Autodesk.Revit.DB;
// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Runtime
{
	internal class IFCExporterLinkedFileData
	{
		#region fields
		private string _path;
		private int _found;
		private double _angle;
		private XYZ _position;
		#endregion fields

		#region properties
		internal string Path
		{
			get
			{
				if (!string.IsNullOrEmpty(_path)) return _path;
				throw new Exception("No property Path set for IFCExporterLinkedFileData");
			}
			private set
			{
				if (string.IsNullOrEmpty(value)) throw new Exception("Attempt to clear property Path for IFCExporterLinkedFileData");
				_path = value;
			}
		}
		internal bool Found
		{
			get
			{
				return _found != 0;
			}
			private set
			{
				_found = value ? 1 : 0;
			}
		}
		internal double Angle
		{
			get
			{
				return _angle;
			}
			set
			{
				_angle = value;
			}
		}
		internal XYZ Position
		{
			get
			{
				if (null != _position) return _position;
				_position = new XYZ(0.0, 0.0, 0.0);
				return _position;
			}
			set
			{
				if (null == value)
				{
					_position = new XYZ(0.0, 0.0, 0.0);
					return;
				}
				_position = new XYZ(value.X, value.Y, value.Z);
			}
		}
		#endregion properties

		#region ctors, dtor
		internal IFCExporterLinkedFileData(string path)
		{
			Path = path;
			Found = File.Exists(path);
		}
		#endregion ctors, dtor
	}
}
