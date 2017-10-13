using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.IFC;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Local

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

//using BIM.IFC.Export.UI.Properties;

namespace BimServerExchange.Runtime
{
	public class IFCExporterConfiguration
	{
		#region types and enums
		#endregion types and enums

		#region fields
		private bool _isBuiltInConfiguration;
		#endregion fields

		#region properties
		/// <summary>
		/// The name of the configuration.
		/// </summary>
		internal string Name { get; set; }

		/// <summary>
		/// The IFCVersion of the configuration.
		/// </summary>
		internal IFCVersion IFCFVersion { get; set; }

		/// <summary>
		/// The IFCFileFormat of the configuration.
		/// </summary>
		internal IFCFileFormat IFCFileType { get; set; }

		/// <summary>
		/// The level of space boundaries of the configuration.
		/// </summary>
		internal int SpaceBoundaries { get; set; }

		/// <summary>
		/// The phase of the document to export.
		/// </summary>
		internal ElementId ActivePhaseId { get; set; }

		/// <summary>
		/// Whether or not to include base quantities for model elements in the export data. 
		/// Base quantities are generated from model geometry to reflect actual physical quantity values, independent of measurement rules or methods.
		/// </summary>
		internal bool ExportBaseQuantities { get; set; }

		/// <summary>
		/// Whether or not to split walls and columns by building stories.
		/// </summary>
		internal bool SplitWallsAndColumns { get; set; }

		/// <summary>
		/// True to include the Revit-specific property sets based on parameter groups. 
		/// False to exclude them.
		/// </summary>
		internal bool ExportInternalRevitPropertySets { get; set; }

		/// <summary>
		/// True to include the IFC common property sets. 
		/// False to exclude them.
		/// </summary>
		internal bool ExportIFCCommonPropertySets { get; set; }

		/// <summary>
		/// True to include 2D elements supported by IFC export (notes and filled regions). 
		/// False to exclude them.
		/// </summary>
		internal bool Export2DElements { get; set; }

		/// <summary>
		/// True to export only the visible elements of the current view (based on filtering and/or element and category hiding). 
		/// False to export the entire model.  
		/// </summary>
		internal bool VisibleElementsOfCurrentView { get; set; }

		/// <summary>
		/// True to use a simplified approach to calculation of room volumes (based on extrusion of 2D room boundaries) which is also the default when exporting to IFC 2x2.   
		/// False to use the Revit calculated room geometry to represent the room volumes (which is the default when exporting to IFC 2x3).
		/// </summary>
		internal bool Use2DRoomBoundaryForVolume { get; set; }

		/// <summary>
		/// True to use the family and type name for references. 
		/// False to use the type name only.
		/// </summary>
		internal bool UseFamilyAndTypeNameForReference { get; set; }

		/// <summary>
		/// True to export the parts as independent building elements
		/// False to export the parts with host element.
		/// </summary>
		internal bool ExportPartsAsBuildingElements { get; set; }

		/// <summary>
		/// True to allow exports of solid models when possible.
		/// False to exclude them.
		/// </summary>
		internal bool ExportSolidModelRep { get; set; }

		/// <summary>
		/// True to allow exports of schedules as custom property sets.
		/// False to exclude them.
		/// </summary>
		internal bool ExportSchedulesAsPsets { get; set; }

		/// <summary>
		/// True to allow user defined property sets to be exported
		/// False to ignore them
		/// </summary>
		internal bool ExportUserDefinedPsets { get; set; }

		/// <summary>
		/// The name of the file containing the user defined property sets to be exported.
		/// </summary>
		internal string ExportUserDefinedPsetsFileName { get; set; }

		/// <summary>
		/// True if the User decides to use the Parameter Mapping Table
		/// False if the user decides to ignore it
		/// </summary>
		internal bool ExportUserDefinedParameterMapping { get; set; }

		/// <summary>
		/// The name of the file containing the user defined Parameter Mapping Table to be exported.
		/// </summary>
		internal string ExportUserDefinedParameterMappingFileName { get; set; }

		/// <summary>
		/// True to export bounding box.
		/// False to exclude them.
		/// </summary>
		internal bool ExportBoundingBox { get; set; }

		/// <summary>
		/// True to include IFCSITE elevation in the site local placement origin.
		/// </summary>
		internal bool IncludeSiteElevation { get; set; }

		/// <summary>
		/// True to use the active view when generating geometry.
		/// False to use default export options.
		/// </summary>
		internal bool UseActiveViewGeometry { get; set; }

		/// <summary>
		/// True to export specific schedules.
		/// </summary>
		internal bool? ExportSpecificSchedules { get; set; }

		/// <summary>
		/// Value indicating the level of detail to be used by tessellation. Valid valus is between 0 to 1
		/// </summary>
		internal double TessellationLevelOfDetail { get; set; }

		/// <summary>
		/// True to store the IFC GUID in the file after the export.  This will require manually saving the file to keep the parameter.
		/// </summary>
		internal bool StoreIFCGUID { get; set; }

		/// <summary>
		/// True to export rooms if their bounding box intersect with the section box.
		/// </summary>
		/// <remarks>
		/// If the section box isn't visible, then all the rooms are exported if this option is set.
		/// </remarks>
		internal bool ExportRoomsInView { get; set; }

		/// <summary>
		/// 
		/// </summary>
		internal bool ExportLinkedFiles { get; set; }

		/// <summary>
		/// Id of the active view.
		/// </summary>
		internal int ActiveViewId { get; set; }

		/// <summary>
		/// Whether the configuration is builtIn or not.
		/// </summary>
		internal bool IsBuiltIn
		{
			get { return _isBuiltInConfiguration; }
		}

		/// <summary>
		/// Whether the configuration is in-session or not.
		/// </summary>
		//internal bool IsInSession
		//{
		//	get { return m_isInSession; }
		//}

		/// <summary>
		/// Identifies the version selected by the user.
		/// </summary>
		internal String FileVersionDescription
		{
			get
			{
				string res;
				switch (IFCFVersion)
				{
					case IFCVersion.IFC2x2:
						res = "IFC 2x2 Coordination View";
						break;
					case IFCVersion.IFC2x3:
						res = "IFC 2x3 Coordination View";
						break;
					case IFCVersion.IFC2x3CV2:
						res = "IFC 2x3 Coordination View 2.0";
						break;
					case IFCVersion.IFC4:
						res = "IFC 4 Coordination View 2.0";
						break;
					case IFCVersion.IFCBCA:
						res = "IFC 2x2 Singapore BCA e-Plan Check";
						break;
					case IFCVersion.IFCCOBIE:
						res = "IFC 2x3 GSA Concept Design BIM 2010";
						break;
#if (!LIMITEDIFC)
					case IFCVersion.IFC2x3FM:
						res = "IFC2x3 Extended FM Handover View";
						break;
					case IFCVersion.IFC4DTV:
						res = "IFC4 Design Transfer View";
						break;
					case IFCVersion.IFC4RV:
						res = "IFC4 Reference View";
						break;
					case IFCVersion.IFC2x3BFM:
						res = "IFC 2x3 Basic FM Handover View";
						break;
#endif
					default:
						res = "Unrecognized IFC version";
						break;
				}

				return res;
			}
		}
#endregion properties


#region ctors, dtor
		/// <summary>
		/// Constructs a default configuration.
		/// </summary>
		private IFCExporterConfiguration()
		{
			Name = "<<Default>>";
			IFCFVersion = IFCVersion.IFC2x3CV2;
			IFCFileType = IFCFileFormat.Ifc;
			SpaceBoundaries = 0;
			ActivePhaseId = ElementId.InvalidElementId;
			ExportBaseQuantities = false;
			SplitWallsAndColumns = false;
			VisibleElementsOfCurrentView = false;
			Use2DRoomBoundaryForVolume = false;
			UseFamilyAndTypeNameForReference = false;
			ExportInternalRevitPropertySets = false;
			ExportIFCCommonPropertySets = true;
			Export2DElements = false;
			ExportPartsAsBuildingElements = false;
			ExportBoundingBox = false;
			ExportSolidModelRep = false;
			ExportSchedulesAsPsets = false;
			ExportUserDefinedPsets = false;
			ExportUserDefinedPsetsFileName = "";
			ExportUserDefinedParameterMapping = false;
			ExportUserDefinedParameterMappingFileName = "";
			ExportLinkedFiles = false;
			IncludeSiteElevation = false;
			UseActiveViewGeometry = false;
			ExportSpecificSchedules = false;
			TessellationLevelOfDetail = 0.5;
			StoreIFCGUID = false;
			_isBuiltInConfiguration = false;
			//this.m_isInSession = false;
			ExportRoomsInView = false;
		}

		/// <summary>
		/// Constructs a copy configuration by providing name and defined configuration. 
		/// </summary>
		/// <param name="name">The name of copy configuration.</param>
		/// <param name="other">The defined configuration to copy.</param>
		private IFCExporterConfiguration(String name, IFCExporterConfiguration other)
		{
			Name = name;
			IFCFVersion = other.IFCFVersion;
			IFCFileType = other.IFCFileType;
			SpaceBoundaries = other.SpaceBoundaries;
			ExportBaseQuantities = other.ExportBaseQuantities;
			SplitWallsAndColumns = other.SplitWallsAndColumns;
			ExportInternalRevitPropertySets = other.ExportInternalRevitPropertySets;
			ExportIFCCommonPropertySets = other.ExportIFCCommonPropertySets;
			Export2DElements = other.Export2DElements;
			VisibleElementsOfCurrentView = other.VisibleElementsOfCurrentView;
			Use2DRoomBoundaryForVolume = other.Use2DRoomBoundaryForVolume;
			UseFamilyAndTypeNameForReference = other.UseFamilyAndTypeNameForReference;
			ExportPartsAsBuildingElements = other.ExportPartsAsBuildingElements;
			UseActiveViewGeometry = other.UseActiveViewGeometry;
			ExportSpecificSchedules = other.ExportSpecificSchedules;
			ExportBoundingBox = other.ExportBoundingBox;
			ExportSolidModelRep = other.ExportSolidModelRep;
			ExportSchedulesAsPsets = other.ExportSchedulesAsPsets;
			ExportUserDefinedPsets = other.ExportUserDefinedPsets;
			ExportUserDefinedPsetsFileName = other.ExportUserDefinedPsetsFileName;
			ExportUserDefinedParameterMapping = other.ExportUserDefinedParameterMapping;
			ExportUserDefinedParameterMappingFileName = other.ExportUserDefinedParameterMappingFileName;
			ExportLinkedFiles = other.ExportLinkedFiles;
			IncludeSiteElevation = other.IncludeSiteElevation;
			TessellationLevelOfDetail = other.TessellationLevelOfDetail;
			ActivePhaseId = other.ActivePhaseId;
			ExportRoomsInView = other.ExportRoomsInView;
			_isBuiltInConfiguration = false;
			//this.m_isInSession = false;
		}

#region ctor support
		/// <summary>
		/// Creates a new default configuration.
		/// </summary>
		/// <returns>The new default configuration.</returns>
		internal static IFCExporterConfiguration CreateDefaultConfiguration()
		{
			return new IFCExporterConfiguration();
		}

		public IFCExporterConfiguration Clone()
		{
			return new IFCExporterConfiguration(this);
		}
#endregion ctor support


		/// <summary>
		/// Creates a builtIn configuration by particular options.
		/// </summary>
		/// <param name="name">The configuration name.</param>
		/// <param name="ifcVersion">The IFCVersion.</param>
		/// <param name="spaceBoundaries">The space boundary level.</param>
		/// <param name="exportBaseQuantities">The ExportBaseQuantities.</param>
		/// <param name="splitWalls">bool indicating if walls and columns should be split by level</param>
		/// <param name="internalSets">bool indicating if the export should use the internal propertysets</param>
		/// <param name="schedulesAsPSets">bool indicating if schedules shoudl be exported as propertysets</param>
		/// <param name="userDefinedPSets">bool indicating if userdefined propertysets should be used</param>
		/// <param name="userDefinedParameterMapping">bool indicating if userdefind parametermappings should be used</param>
		/// <param name="PlanElems2D">The Export2DElements option.</param>
		/// <param name="exportBoundingBox">The exportBoundingBox option.</param>
		/// <param name="exportLinkedFiles">The exportLinkedFiles option.</param>
		/// <returns>The builtIn configuration.</returns>
		internal static IFCExporterConfiguration CreateBuiltInConfiguration(string name,
																								IFCVersion ifcVersion,
																								int spaceBoundaries,
																								bool exportBaseQuantities,
																								bool splitWalls,
																								bool internalSets,
																								bool schedulesAsPSets,
																								bool userDefinedPSets,
																								bool userDefinedParameterMapping,
																								bool PlanElems2D,
																								bool exportBoundingBox,
																								bool exportLinkedFiles)
		{
			IFCExporterConfiguration configuration = new IFCExporterConfiguration();
			configuration.Name = name;
			configuration.IFCFVersion = ifcVersion;
			configuration.IFCFileType = IFCFileFormat.Ifc;
			configuration.SpaceBoundaries = spaceBoundaries;
			configuration.ExportBaseQuantities = exportBaseQuantities;
			configuration.SplitWallsAndColumns = splitWalls;
			configuration.ExportInternalRevitPropertySets = internalSets;
			configuration.ExportIFCCommonPropertySets = true;
			configuration.Export2DElements = PlanElems2D;
			configuration.VisibleElementsOfCurrentView = false;
			configuration.Use2DRoomBoundaryForVolume = false;
			configuration.UseFamilyAndTypeNameForReference = false;
			configuration.ExportPartsAsBuildingElements = false;
			configuration.UseActiveViewGeometry = false;
			configuration.ExportSpecificSchedules = false;
			configuration.ExportBoundingBox = exportBoundingBox;
			configuration.ExportSolidModelRep = false;
			configuration.ExportSchedulesAsPsets = schedulesAsPSets;
			configuration.ExportUserDefinedPsets = userDefinedPSets;
			configuration.ExportUserDefinedPsetsFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" + name + @".txt";
			configuration.ExportUserDefinedParameterMapping = userDefinedParameterMapping;
			configuration.ExportUserDefinedParameterMappingFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\ParameterMappingTable.txt";
			configuration.ExportLinkedFiles = exportLinkedFiles;
			configuration.IncludeSiteElevation = false;
			// The default tesselationLevelOfDetail will be low
			configuration.TessellationLevelOfDetail = 0.5;
			configuration.StoreIFCGUID = false;
			configuration._isBuiltInConfiguration = true;
			//configuration.m_isInSession = false;
			configuration.ActivePhaseId = ElementId.InvalidElementId;
			configuration.ExportRoomsInView = false;
			return configuration;
		}

		/// <summary>
		/// Constructs a copy from a defined configuration.
		/// </summary>
		/// <param name="other">The configuration to copy.</param>
		private IFCExporterConfiguration(IFCExporterConfiguration other)
		{
			Name = other.Name;
			IFCFVersion = other.IFCFVersion;
			IFCFileType = other.IFCFileType;
			SpaceBoundaries = other.SpaceBoundaries;
			ExportBaseQuantities = other.ExportBaseQuantities;
			SplitWallsAndColumns = other.SplitWallsAndColumns;
			ExportInternalRevitPropertySets = other.ExportInternalRevitPropertySets;
			ExportIFCCommonPropertySets = other.ExportIFCCommonPropertySets;
			Export2DElements = other.Export2DElements;
			VisibleElementsOfCurrentView = other.VisibleElementsOfCurrentView;
			Use2DRoomBoundaryForVolume = other.Use2DRoomBoundaryForVolume;
			UseFamilyAndTypeNameForReference = other.UseFamilyAndTypeNameForReference;
			ExportPartsAsBuildingElements = other.ExportPartsAsBuildingElements;
			UseActiveViewGeometry = other.UseActiveViewGeometry;
			ExportSpecificSchedules = other.ExportSpecificSchedules;
			ExportBoundingBox = other.ExportBoundingBox;
			ExportSolidModelRep = other.ExportSolidModelRep;
			ExportSchedulesAsPsets = other.ExportSchedulesAsPsets;
			ExportUserDefinedPsets = other.ExportUserDefinedPsets;
			ExportUserDefinedPsetsFileName = other.ExportUserDefinedPsetsFileName;
			ExportUserDefinedParameterMapping = other.ExportUserDefinedParameterMapping;
			ExportUserDefinedParameterMappingFileName = other.ExportUserDefinedParameterMappingFileName;
			ExportLinkedFiles = other.ExportLinkedFiles;
			IncludeSiteElevation = other.IncludeSiteElevation;
			TessellationLevelOfDetail = other.TessellationLevelOfDetail;
			StoreIFCGUID = other.StoreIFCGUID;
			_isBuiltInConfiguration = other._isBuiltInConfiguration;
			//this.m_isInSession = other.m_isInSession;
			ActivePhaseId = other.ActivePhaseId;
			ExportRoomsInView = other.ExportRoomsInView;
		}

		/// <summary>
		/// Duplicates this configuration by giving a new name.
		/// </summary>
		/// <param name="newName">The new name of the copy configuration.</param>
		/// <returns>The duplicated configuration.</returns>
		internal IFCExporterConfiguration Duplicate(String newName)
		{
			return new IFCExporterConfiguration(newName, this);
		}
#endregion ctors, dtor

		/// <summary>
		/// Updates the IFCExportOptions with the settings in this configuration.
		/// </summary>
		/// <param name="uidoc">UIDocument from which the options are created (some settings are document dependent)</param>
		/// <param name="options">The IFCExportOptions to update.</param>
		/// <param name="filterViewId">The id of the view that will be used to select which elements to export.</param>
		internal void UpdateOptions(ElementId filterViewId, Document doc, ref IFCExportOptions options)
		{
			if (null == options || null == doc || null == options) return;

			options.FileVersion = IFCFVersion;
			options.SpaceBoundaryLevel = SpaceBoundaries;
			options.ExportBaseQuantities = ExportBaseQuantities;
			options.WallAndColumnSplitting = SplitWallsAndColumns;
			options.FilterViewId = VisibleElementsOfCurrentView ? filterViewId : ElementId.InvalidElementId;
			options.AddOption("ExportInternalRevitPropertySets", ExportInternalRevitPropertySets.ToString());
			options.AddOption("ExportIFCCommonPropertySets", ExportIFCCommonPropertySets.ToString());
			options.AddOption("ExportAnnotations", Export2DElements.ToString());
			options.AddOption("Use2DRoomBoundaryForVolume", Use2DRoomBoundaryForVolume.ToString());
			options.AddOption("UseFamilyAndTypeNameForReference", UseFamilyAndTypeNameForReference.ToString());
			options.AddOption("ExportVisibleElementsInView", VisibleElementsOfCurrentView.ToString());
			options.AddOption("ExportPartsAsBuildingElements", ExportPartsAsBuildingElements.ToString());
			options.AddOption("UseActiveViewGeometry", UseActiveViewGeometry.ToString());
			options.AddOption("ExportSpecificSchedules", ExportSpecificSchedules.ToString());
			options.AddOption("ExportBoundingBox", ExportBoundingBox.ToString());
			options.AddOption("ExportSolidModelRep", ExportSolidModelRep.ToString());
			options.AddOption("ExportSchedulesAsPsets", ExportSchedulesAsPsets.ToString());
			options.AddOption("ExportUserDefinedPsets", ExportUserDefinedPsets.ToString());
			options.AddOption("ExportUserDefinedParameterMapping", ExportUserDefinedParameterMapping.ToString());
			options.AddOption("ExportLinkedFiles", ExportLinkedFiles.ToString());
			options.AddOption("IncludeSiteElevation", IncludeSiteElevation.ToString());
			options.AddOption("TessellationLevelOfDetail", TessellationLevelOfDetail.ToString(CultureInfo.InvariantCulture));
			options.AddOption("ActiveViewId", ActiveViewId.ToString());
			options.AddOption("StoreIFCGUID", StoreIFCGUID.ToString());

			// The active phase may not be valid if we are exporting multiple projects. However, if projects share a template that defines the phases,
			// then the ActivePhaseId would likely be valid for all.  There is some small chance that the ActivePhaseId would be a valid, but different, phase
			// in different projects, but that is unlikely enough that it seems worth warning against it but allowing the better functionality in general.
			if (ValidatePhase(ActivePhaseId, doc)) options.AddOption("ActivePhase", ActivePhaseId.ToString());

			options.AddOption("FileType", IFCFileType.ToString());
			//string uiVersion = IFCUISettings.GetAssemblyVersion();
			//options.AddOption("AlternateUIVersion", uiVersion);

			options.AddOption("ConfigName", Name); // Add config name into the option for use in the exporter
			options.AddOption("ExportUserDefinedPsetsFileName", ExportUserDefinedPsetsFileName);
			options.AddOption("ExportUserDefinedParameterMappingFileName", ExportUserDefinedParameterMappingFileName);
			options.AddOption("ExportRoomsInView", ExportRoomsInView.ToString());
		}

		private bool ValidatePhase(ElementId activePhaseId, Document doc)
		{
			if (null == doc || activePhaseId == ElementId.InvalidElementId) return false;
			// using the active document here may cause problems when exporting linked files
			Element elem = doc.GetElement(ActivePhaseId);
			return (elem is Phase);
		}

		/// <summary>
		/// Converts to the string to identify the configuration.
		/// </summary>
		/// <returns>The string to identify the configuration.</returns>
		public override String ToString()
		{
			if (IsBuiltIn)
			{
				//return "<" + Name + " " + Properties.Resources.Setup + ">;
				return $"<{Name}>";
			}

			return Name;
		}

		public bool ReadFromFile(string path)
		{
			if (string.IsNullOrEmpty(path) || !File.Exists(path)) return false;

			try
			{
				XmlDocument xdoc = new XmlDocument();
				xdoc.Load(path);

				XmlElement root = xdoc.DocumentElement;
				if (null == root || 0 != string.Compare(root.Name, "IFCExporterConfiguration", StringComparison.InvariantCultureIgnoreCase))
				{
					throw new Exception("Missing root element in Xml");
				}

				foreach (XmlNode node in root)
				{
					XmlElement elem = node as XmlElement;
					if (null == elem) continue;

					String val = elem.GetAttribute("Value");
					if (String.IsNullOrEmpty(val)) continue;

					// try to get the property associated with this element
					PropertyInfo prop = typeof(IFCExporterConfiguration).GetProperty(elem.Name);
					if (null == prop) continue;

					// implicitly call the set accessor on this property (we may have to convert the non-string types)
					try
					{
						TypeConverter conv = TypeDescriptor.GetConverter(prop.PropertyType);
						object oval = conv.ConvertFromInvariantString(val);
						if (null != oval) prop.SetValue(this, oval, null);
					}
					catch (Exception ex)
					{
						//DrieBLogger.AppendLogMessage($"IFCExporterConfiguration could not convert property '{prop.Name}' from value string '{val}' ({ex.Message})");
						throw new IcnException($"IFCExporterConfiguration could not convert property '{prop.Name}' from value string '{val}' ({ex.Message})", 10, "IFCExporterConfiguration");
					}
				}
			}
			catch (Exception ex)
			{
				//DrieBLogger.AppendLogMessage($"IFC configuration file '{path}' could not be read ({ex.Message})");
				throw new IcnException($"IFC configuration file '{path}' could not be read ({ex.Message})", 10, "IFCExporterConfiguration");
			}

			return true;
		}

		public bool SaveToFile(string path)
		{
			if (string.IsNullOrEmpty(path) || !File.Exists(path)) return false;

			XmlDocument xdoc = new XmlDocument();
			XmlElement root = xdoc.CreateElement("IFCExporterConfiguration");
			xdoc.AppendChild(root);

			PropertyInfo[] properties = typeof(IFCExporterConfiguration).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo prop in properties)
			{
				String val = prop.GetValue(this) as String;
				XmlElement data = xdoc.CreateElement(prop.Name);
				root.AppendChild(data);
				data.SetAttribute("Value", val);
			}

			xdoc.Save(path);
			return true;
		}
	}
}
