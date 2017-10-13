using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.IFC;
using Autodesk.Revit.UI;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Local

namespace BimServerExchange.Runtime
{
	/// <summary>
	/// The map to store BuiltIn and Saved configurations.
	/// </summary>
	internal class IFCExporterConfigurations
	{
		#region types and enums
		private readonly Guid _schemaId = new Guid("A1E672E5-AC88-4933-A019-F9068402CFA7");
		private readonly Guid _mapSchemaId = new Guid("DCB88B13-594F-44F6-8F5D-AE9477305AC3");

		#region schema and schemafield related constants
		// The MapField is to defined the map<string,string> in schema. 
		// Please don't change the name values, it affects the schema.
		private const String s_configMapField = "MapField";
		// The following are the keys in the MapField in new schema. For old schema, they are simple fields.
		private const String s_setupName = "Name";
		private const String s_setupVersion = "Version";
		private const String s_setupFileFormat = "FileFormat";
		private const String s_setupSpaceBoundaries = "SpaceBoundaryLevel";
		private const String s_setupQTO = "ExportBaseQuantities";
		private const String s_splitWallsAndColumns = "SplitWallsAndColumns";
		private const String s_setupCurrentView = "VisibleElementsInCurrentView";
		private const String s_setupExport2D = "Export2DElements";
		private const String s_setupExportRevitProps = "ExportInternalRevitPropertySets";
		private const String s_setupExportIFCCommonProperty = "ExportIFCCommonPropertySets";
		private const String s_setupUse2DForRoomVolume = "Use2DBoundariesForRoomVolume";
		private const String s_setupUseFamilyAndTypeName = "UseFamilyAndTypeNameForReference";
		private const String s_setupExportPartsAsBuildingElements = "ExportPartsAsBuildingElements";
		private const String s_useActiveViewGeometry = "UseActiveViewGeometry";
		private const String s_setupExportSpecificSchedules = "ExportSpecificSchedules";
		private const String s_setupExportBoundingBox = "ExportBoundingBox";
		private const String s_setupExportSolidModelRep = "ExportSolidModelRep";
		private const String s_setupExportSchedulesAsPsets = "ExportSchedulesAsPsets";
		private const string s_setupExportUserDefinedPsets = "ExportUserDefinedPsets";
		private const string s_setupExportUserDefinedPsetsFileName = "ExportUserDefinedPsetsFileName";
		private const string s_setupExportUserDefinedParameterMapping = "ExportUserDefinedParameterMapping";
		private const string s_setupExportUserDefinedParameterMappingFileName = "ExportUserDefinedParameterMappingFileName";
		private const string s_setupExportLinkedFiles = "ExportLinkedFiles";
		private const String s_setupIncludeSiteElevation = "IncludeSiteElevation";
		private const String s_setupUseCoarseTessellation = "UseCoarseTessellation";
		private const String s_setupStoreIFCGUID = "StoreIFCGUID";
		private const String s_setupActivePhase = "ActivePhase";
		private const String s_setupExportRoomsInView = "ExportRoomsInView";
		#endregion
		#endregion types and enums

		#region fields
		private Dictionary<String, IFCExporterConfiguration> _data;
		private Schema _schema;
		private Schema _mapSchema;
		private UIDocument _uidoc;
		#endregion fields

		#region properties
		internal Dictionary<String, IFCExporterConfiguration> Dict
		{
			get
			{
				if (null != _data) return _data;

				_data = new Dictionary<String, IFCExporterConfiguration>();
				return _data;
			}
		}

		private UIDocument UIDoc {
			get
			{
				if (null != _uidoc) return _uidoc;

				throw new Exception("Property UIDoc not set to a value in IFCExporterConfigurations");
			}
			set
			{
				if (null == value) return;

				_uidoc = value;
			}
		}

		#endregion properties

		#region ctors, dtor
		/// <summary>
		/// Constructs a default map.
		/// </summary>
		internal IFCExporterConfigurations()
		{
		}

		/// <summary>
		/// Constructs a new map as a copy of an existing one.
		/// </summary>
		/// <param name="map">The specific map to copy.</param>
		internal IFCExporterConfigurations(IFCExporterConfigurations map)
		{
			// Deep copy
			foreach (IFCExporterConfiguration value in map.Values)
			{
				Add(value.Clone());
			}
		}
		#endregion ctors, dtor

		/// <summary>
		/// Adds the built-in configurations to the map.
		/// </summary>
		internal void AddBuiltInConfigurations()
		{
			// These are the built-in configurations.  Provide a more extensible means of storage.
			// Order of construction: name, version, space boundaries, QTO, split walls, internal sets, 2d elems, boundingBox
#if LIMITEDIFC
			// Revit2016 only supports the following five IFC versions, subsequent revits support eight.
			Add(IFCExporterConfiguration.CreateBuiltInConfiguration("IFC2x3 Coordination View 2.0", IFCVersion.IFC2x3CV2, 0, false, false, false, false, false, false, false, false, false));
			Add(IFCExporterConfiguration.CreateBuiltInConfiguration("IFC2x3 Coordination View", IFCVersion.IFC2x3, 1, false, false, true, false, false, false, true, false, false));
			Add(IFCExporterConfiguration.CreateBuiltInConfiguration("IFC2x3 GSA Concept Design BIM 2010", IFCVersion.IFCCOBIE, 2, true, true, true, false, false, false, true, true, false));
			Add(IFCExporterConfiguration.CreateBuiltInConfiguration("IFC2x2 Coordination View", IFCVersion.IFC2x2, 1, false, false, true, false, false, false, false, false, false));
			Add(IFCExporterConfiguration.CreateBuiltInConfiguration("IFC2x2 Singapore BCA e-Plan Check", IFCVersion.IFCBCA, 1, false, true, true, false, false, false, false, false, false));
#else
			Add(IFCExporterConfiguration.CreateBuiltInConfiguration("IFC4 Reference View", IFCVersion.IFC4RV, 0, false, false, false, false, false, false, false, false, false));
			Add(IFCExporterConfiguration.CreateBuiltInConfiguration("IFC4 Design Transfer View", IFCVersion.IFC4DTV, 0, false, false, false, false, false, false, false, false, false));
			Add(IFCExporterConfiguration.CreateBuiltInConfiguration("IFC2x3 Coordination View 2.0", IFCVersion.IFC2x3CV2, 0, false, false, false, false, false, false, false, false, false));
			Add(IFCExporterConfiguration.CreateBuiltInConfiguration("IFC2x3 Coordination View", IFCVersion.IFC2x3, 1, false, false, true, false, false, false, true, false, false));
			Add(IFCExporterConfiguration.CreateBuiltInConfiguration("IFC2x3 GSA Concept Design BIM 2010", IFCVersion.IFCCOBIE, 2, true, true, true, false, false, false, true, true, false));
			Add(IFCExporterConfiguration.CreateBuiltInConfiguration("IFC2x3 Basic FM Handover View", IFCVersion.IFC2x3BFM, 1, true, true, false, false, false, false, true, false, false));
			Add(IFCExporterConfiguration.CreateBuiltInConfiguration("IFC2x2 Coordination View", IFCVersion.IFC2x2, 1, false, false, true, false, false, false, false, false, false));
			Add(IFCExporterConfiguration.CreateBuiltInConfiguration("IFC2x2 Singapore BCA e-Plan Check", IFCVersion.IFCBCA, 1, false, true, true, false, false, false, false, false, false));
			Add(IFCExporterConfiguration.CreateBuiltInConfiguration("IFC2x3 Extended FM Handover View", IFCVersion.IFC2x3FM, 1, true, false, false, true, true, false, true, true, false));
#endif
		}

		/// <summary>
		/// Adds the saved configuration from document to the map.
		/// </summary>
		internal void AddProjectConfigurations()
		{
			try
			{
				if (_schema == null)
				{
					_schema = Schema.Lookup(_schemaId);
				}
				if (_mapSchema == null)
				{
					_mapSchema = Schema.Lookup(_mapSchemaId);
				}

				if (_mapSchema != null)
				{
					foreach (DataStorage storedSetup in GetProjectConfigurations(_mapSchema))
					{
						Entity configEntity = storedSetup.GetEntity(_mapSchema);
						IDictionary<string, string> configMap = configEntity.Get<IDictionary<string, string>>(s_configMapField);
						IFCExporterConfiguration configuration = IFCExporterConfiguration.CreateDefaultConfiguration();
						if (configMap.ContainsKey(s_setupName))
							configuration.Name = configMap[s_setupName];
						if (configMap.ContainsKey(s_setupVersion))
							configuration.IFCFVersion = (IFCVersion) Enum.Parse(typeof(IFCVersion), configMap[s_setupVersion]);
						if (configMap.ContainsKey(s_setupFileFormat))
							configuration.IFCFileType = (IFCFileFormat) Enum.Parse(typeof(IFCFileFormat), configMap[s_setupFileFormat]);
						if (configMap.ContainsKey(s_setupSpaceBoundaries))
							configuration.SpaceBoundaries = int.Parse(configMap[s_setupSpaceBoundaries]);
						if (configMap.ContainsKey(s_setupActivePhase))
							configuration.ActivePhaseId = new ElementId(int.Parse(configMap[s_setupActivePhase]));
						if (configMap.ContainsKey(s_setupQTO))
							configuration.ExportBaseQuantities = bool.Parse(configMap[s_setupQTO]);
						if (configMap.ContainsKey(s_setupCurrentView))
							configuration.VisibleElementsOfCurrentView = bool.Parse(configMap[s_setupCurrentView]);
						if (configMap.ContainsKey(s_splitWallsAndColumns))
							configuration.SplitWallsAndColumns = bool.Parse(configMap[s_splitWallsAndColumns]);
						if (configMap.ContainsKey(s_setupExport2D))
							configuration.Export2DElements = bool.Parse(configMap[s_setupExport2D]);
						if (configMap.ContainsKey(s_setupExportRevitProps))
							configuration.ExportInternalRevitPropertySets = bool.Parse(configMap[s_setupExportRevitProps]);
						if (configMap.ContainsKey(s_setupExportIFCCommonProperty))
							configuration.ExportIFCCommonPropertySets = bool.Parse(configMap[s_setupExportIFCCommonProperty]);
						if (configMap.ContainsKey(s_setupUse2DForRoomVolume))
							configuration.Use2DRoomBoundaryForVolume = bool.Parse(configMap[s_setupUse2DForRoomVolume]);
						if (configMap.ContainsKey(s_setupUseFamilyAndTypeName))
							configuration.UseFamilyAndTypeNameForReference = bool.Parse(configMap[s_setupUseFamilyAndTypeName]);
						if (configMap.ContainsKey(s_setupExportPartsAsBuildingElements))
							configuration.ExportPartsAsBuildingElements = bool.Parse(configMap[s_setupExportPartsAsBuildingElements]);
						if (configMap.ContainsKey(s_useActiveViewGeometry))
							configuration.UseActiveViewGeometry = bool.Parse(configMap[s_useActiveViewGeometry]);
						if (configMap.ContainsKey(s_setupExportSpecificSchedules))
							configuration.ExportSpecificSchedules = bool.Parse(configMap[s_setupExportSpecificSchedules]);
						if (configMap.ContainsKey(s_setupExportBoundingBox))
							configuration.ExportBoundingBox = bool.Parse(configMap[s_setupExportBoundingBox]);
						if (configMap.ContainsKey(s_setupExportSolidModelRep))
							configuration.ExportSolidModelRep = bool.Parse(configMap[s_setupExportSolidModelRep]);
						if (configMap.ContainsKey(s_setupExportSchedulesAsPsets))
							configuration.ExportSchedulesAsPsets = bool.Parse(configMap[s_setupExportSchedulesAsPsets]);
						if (configMap.ContainsKey(s_setupExportUserDefinedPsets))
							configuration.ExportUserDefinedPsets = bool.Parse(configMap[s_setupExportUserDefinedPsets]);
						if (configMap.ContainsKey(s_setupExportUserDefinedPsetsFileName))
							configuration.ExportUserDefinedPsetsFileName = configMap[s_setupExportUserDefinedPsetsFileName];
						if (configMap.ContainsKey(s_setupExportUserDefinedParameterMapping))
							configuration.ExportUserDefinedParameterMapping = bool.Parse(configMap[s_setupExportUserDefinedParameterMapping]);
						if (configMap.ContainsKey(s_setupExportUserDefinedParameterMappingFileName))
							configuration.ExportUserDefinedParameterMappingFileName = configMap[s_setupExportUserDefinedParameterMappingFileName];
						if (configMap.ContainsKey(s_setupExportLinkedFiles))
							configuration.ExportLinkedFiles = bool.Parse(configMap[s_setupExportLinkedFiles]);
						if (configMap.ContainsKey(s_setupIncludeSiteElevation))
							configuration.IncludeSiteElevation = bool.Parse(configMap[s_setupIncludeSiteElevation]);
						if (configMap.ContainsKey(s_setupStoreIFCGUID))
							configuration.StoreIFCGUID = bool.Parse(configMap[s_setupStoreIFCGUID]);
						if (configMap.ContainsKey(s_setupExportRoomsInView))
							configuration.ExportRoomsInView = bool.Parse(configMap[s_setupExportRoomsInView]);
						Add(configuration);
					}

					return; // if finds the config in map schema, return and skip finding the old schema.
				}

				// find the config in old schema.
				if (_schema != null)
				{
					foreach (DataStorage storedSetup in GetProjectConfigurations(_schema))
					{
						Entity configEntity = storedSetup.GetEntity(_schema);
						IFCExporterConfiguration configuration = IFCExporterConfiguration.CreateDefaultConfiguration();
						configuration.Name = configEntity.Get<String>(s_setupName);
						configuration.IFCFVersion = (IFCVersion) configEntity.Get<int>(s_setupVersion);
						configuration.IFCFileType = (IFCFileFormat) configEntity.Get<int>(s_setupFileFormat);
						configuration.SpaceBoundaries = configEntity.Get<int>(s_setupSpaceBoundaries);
						configuration.ExportBaseQuantities = configEntity.Get<bool>(s_setupQTO);
						configuration.SplitWallsAndColumns = configEntity.Get<bool>(s_splitWallsAndColumns);
						configuration.Export2DElements = configEntity.Get<bool>(s_setupExport2D);
						configuration.ExportInternalRevitPropertySets = configEntity.Get<bool>(s_setupExportRevitProps);
						Field fieldIFCCommonPropertySets = _schema.GetField(s_setupExportIFCCommonProperty);
						if (fieldIFCCommonPropertySets != null)
							configuration.ExportIFCCommonPropertySets = configEntity.Get<bool>(s_setupExportIFCCommonProperty);
						configuration.Use2DRoomBoundaryForVolume = configEntity.Get<bool>(s_setupUse2DForRoomVolume);
						configuration.UseFamilyAndTypeNameForReference = configEntity.Get<bool>(s_setupUseFamilyAndTypeName);
						Field fieldPartsAsBuildingElements = _schema.GetField(s_setupExportPartsAsBuildingElements);
						if (fieldPartsAsBuildingElements != null)
							configuration.ExportPartsAsBuildingElements = configEntity.Get<bool>(s_setupExportPartsAsBuildingElements);
						Field fieldExportBoundingBox = _schema.GetField(s_setupExportBoundingBox);
						if (fieldExportBoundingBox != null)
							configuration.ExportBoundingBox = configEntity.Get<bool>(s_setupExportBoundingBox);
						Field fieldExportSolidModelRep = _schema.GetField(s_setupExportSolidModelRep);
						if (fieldExportSolidModelRep != null)
							configuration.ExportSolidModelRep = configEntity.Get<bool>(s_setupExportSolidModelRep);
						Field fieldExportSchedulesAsPsets = _schema.GetField(s_setupExportSchedulesAsPsets);
						if (fieldExportSchedulesAsPsets != null)
							configuration.ExportSchedulesAsPsets = configEntity.Get<bool>(s_setupExportSchedulesAsPsets);
						Field fieldExportUserDefinedPsets = _schema.GetField(s_setupExportUserDefinedPsets);
						if (fieldExportUserDefinedPsets != null)
							configuration.ExportUserDefinedPsets = configEntity.Get<bool>(s_setupExportUserDefinedPsets);
						Field fieldExportUserDefinedPsetsFileName = _schema.GetField(s_setupExportUserDefinedPsetsFileName);
						if (fieldExportUserDefinedPsetsFileName != null)
							configuration.ExportUserDefinedPsetsFileName = configEntity.Get<string>(s_setupExportUserDefinedPsetsFileName);

						Field fieldExportUserDefinedParameterMapingTable = _schema.GetField(s_setupExportUserDefinedParameterMapping);
						if (fieldExportUserDefinedParameterMapingTable != null)
							configuration.ExportUserDefinedParameterMapping = configEntity.Get<bool>(s_setupExportUserDefinedParameterMapping);

						Field fieldExportUserDefinedParameterMappingFileName = _schema.GetField(s_setupExportUserDefinedParameterMappingFileName);
						if (fieldExportUserDefinedParameterMappingFileName != null)
							configuration.ExportUserDefinedParameterMappingFileName = configEntity.Get<string>(s_setupExportUserDefinedParameterMappingFileName);

						Field fieldExportLinkedFiles = _schema.GetField(s_setupExportLinkedFiles);
						if (fieldExportLinkedFiles != null)
							configuration.ExportLinkedFiles = configEntity.Get<bool>(s_setupExportLinkedFiles);
						Field fieldIncludeSiteElevation = _schema.GetField(s_setupIncludeSiteElevation);
						if (fieldIncludeSiteElevation != null)
							configuration.IncludeSiteElevation = configEntity.Get<bool>(s_setupIncludeSiteElevation);
						Field fieldStoreIFCGUID = _schema.GetField(s_setupStoreIFCGUID);
						if (fieldStoreIFCGUID != null)
							configuration.StoreIFCGUID = configEntity.Get<bool>(s_setupStoreIFCGUID);
						Field fieldActivePhase = _schema.GetField(s_setupActivePhase);
						if (fieldActivePhase != null)
							configuration.ActivePhaseId = new ElementId(int.Parse(configEntity.Get<string>(s_setupActivePhase)));
						Field fieldExportRoomsInView = _schema.GetField(s_setupExportRoomsInView);
						if (fieldExportRoomsInView != null)
							configuration.ExportRoomsInView = configEntity.Get<bool>(s_setupExportRoomsInView);
						Add(configuration);
					}
				}
			}
			catch (Exception)
			{
				// to avoid fail to show the dialog if any exception throws in reading schema.
			}
		}


		/// <summary>
		/// Updates the setups to save into the document.
		/// </summary>
		internal void UpdateProjectConfigurations()
		{
			// delete the old schema and the DataStorage.
			if (_schema == null)
			{
				_schema = Schema.Lookup(_schemaId);
			}
			if (_schema != null)
			{
				IList<DataStorage> oldSavedConfigurations = GetProjectConfigurations(_schema);
				if (oldSavedConfigurations.Count > 0)
				{
					Transaction deleteTransaction = new Transaction(UIDoc?.Document, "Delete old IFC export setups");
					try
					{
						deleteTransaction.Start();
						List<ElementId> dataStorageToDelete = new List<ElementId>();
						foreach (DataStorage dataStorage in oldSavedConfigurations)
						{
							dataStorageToDelete.Add(dataStorage.Id);
						}

						UIDoc?.Document?.Delete(dataStorageToDelete);
						deleteTransaction.Commit();
					}
					catch (Exception)
					{
						if (deleteTransaction.HasStarted())
							deleteTransaction.RollBack();
					}
				}
			}

			// update the configurations to new map schema.
			if (_mapSchema == null)
			{
				_mapSchema = Schema.Lookup(_mapSchemaId);
			}

			// Are there any setups to save or resave?
			List<IFCExporterConfiguration> setupsToSave = new List<IFCExporterConfiguration>();
			foreach (IFCExporterConfiguration configuration in Dict.Values)
			{
				if (configuration.IsBuiltIn) continue;

				setupsToSave.Add(configuration);
			}

			// If there are no setups to save, and if the schema is not present (which means there are no
			// previously existing setups which might have been deleted) we can skip the rest of this method.
			if (setupsToSave.Count <= 0 && _mapSchema == null)
				return;

			if (_mapSchema == null)
			{
				SchemaBuilder builder = new SchemaBuilder(_mapSchemaId);
				builder.SetSchemaName("IFCExporterConfigurationMap");
				builder.AddMapField(s_configMapField, typeof(String), typeof(String));
				_mapSchema = builder.Finish();
			}

			// Overwrite all saved configs with the new list
			Transaction transaction = new Transaction(UIDoc?.Document, "Update IFC export setups");
			try
			{
				transaction.Start();
				IList<DataStorage> savedConfigurations = GetProjectConfigurations(_mapSchema);
				int savedConfigurationCount = savedConfigurations.Count;
				int savedConfigurationIndex = 0;
				foreach (IFCExporterConfiguration configuration in setupsToSave)
				{
					DataStorage configStorage;
					if (savedConfigurationIndex >= savedConfigurationCount)
					{
						configStorage = DataStorage.Create(UIDoc?.Document);
					}
					else
					{
						configStorage = savedConfigurations[savedConfigurationIndex];
						savedConfigurationIndex++;
					}

					Entity mapEntity = new Entity(_mapSchema);
					IDictionary<string, string> mapData = new Dictionary<string, string>();
					mapData.Add(s_setupName, configuration.Name);
					mapData.Add(s_setupVersion, configuration.IFCFVersion.ToString());
					mapData.Add(s_setupFileFormat, configuration.IFCFileType.ToString());
					mapData.Add(s_setupSpaceBoundaries, configuration.SpaceBoundaries.ToString());
					mapData.Add(s_setupQTO, configuration.ExportBaseQuantities.ToString());
					mapData.Add(s_setupCurrentView, configuration.VisibleElementsOfCurrentView.ToString());
					mapData.Add(s_splitWallsAndColumns, configuration.SplitWallsAndColumns.ToString());
					mapData.Add(s_setupExport2D, configuration.Export2DElements.ToString());
					mapData.Add(s_setupExportRevitProps, configuration.ExportInternalRevitPropertySets.ToString());
					mapData.Add(s_setupExportIFCCommonProperty, configuration.ExportIFCCommonPropertySets.ToString());
					mapData.Add(s_setupUse2DForRoomVolume, configuration.Use2DRoomBoundaryForVolume.ToString());
					mapData.Add(s_setupUseFamilyAndTypeName, configuration.UseFamilyAndTypeNameForReference.ToString());
					mapData.Add(s_setupExportPartsAsBuildingElements, configuration.ExportPartsAsBuildingElements.ToString());
					mapData.Add(s_useActiveViewGeometry, configuration.UseActiveViewGeometry.ToString());
					mapData.Add(s_setupExportSpecificSchedules, configuration.ExportSpecificSchedules.ToString());
					mapData.Add(s_setupExportBoundingBox, configuration.ExportBoundingBox.ToString());
					mapData.Add(s_setupExportSolidModelRep, configuration.ExportSolidModelRep.ToString());
					mapData.Add(s_setupExportSchedulesAsPsets, configuration.ExportSchedulesAsPsets.ToString());
					mapData.Add(s_setupExportUserDefinedPsets, configuration.ExportUserDefinedPsets.ToString());
					mapData.Add(s_setupExportUserDefinedPsetsFileName, configuration.ExportUserDefinedPsetsFileName);
					mapData.Add(s_setupExportUserDefinedParameterMapping, configuration.ExportUserDefinedParameterMapping.ToString());
					mapData.Add(s_setupExportUserDefinedParameterMappingFileName, configuration.ExportUserDefinedParameterMappingFileName);
					mapData.Add(s_setupExportLinkedFiles, configuration.ExportLinkedFiles.ToString());
					mapData.Add(s_setupIncludeSiteElevation, configuration.IncludeSiteElevation.ToString());
					mapData.Add(s_setupStoreIFCGUID, configuration.StoreIFCGUID.ToString());
					mapData.Add(s_setupActivePhase, configuration.ActivePhaseId.ToString());
					mapData.Add(s_setupExportRoomsInView, configuration.ExportRoomsInView.ToString());
					mapEntity.Set(s_configMapField, mapData);

					configStorage.SetEntity(mapEntity);
				}

				List<ElementId> elementsToDelete = new List<ElementId>();
				for (; savedConfigurationIndex < savedConfigurationCount; savedConfigurationIndex++)
				{
					DataStorage configStorage = savedConfigurations[savedConfigurationIndex];
					elementsToDelete.Add(configStorage.Id);
				}

				if (elementsToDelete.Count > 0) UIDoc?.Document?.Delete(elementsToDelete);

				transaction.Commit();
			}
			catch (Exception)
			{
				if (transaction.HasStarted())
					transaction.RollBack();
			}
		}

		/// <summary>
		/// Gets the saved setups from the document.
		/// </summary>
		/// <returns>The saved configurations.</returns>
		private IList<DataStorage> GetProjectConfigurations(Schema schema)
		{
			FilteredElementCollector collector = new FilteredElementCollector(UIDoc?.Document);
			collector.OfClass(typeof(DataStorage));
			// ReSharper disable once ConvertToLocalFunction
			Func<DataStorage, bool> hasTargetData = ds => (ds.GetEntity(schema) != null && ds.GetEntity(schema).IsValid());

			return collector.Cast<DataStorage>().Where(hasTargetData).ToList();
		}

		/// <summary>
		/// Adds a configuration to the map.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		internal void Add(IFCExporterConfiguration configuration)
		{
			Dict.Add(configuration.Name, configuration);
		}

		/// <summary>
		/// Whether the map has the name of a configuration.
		/// </summary>
		/// <param name="cname">The configuration name.</param>
		/// <returns>True for having the name, false otherwise.</returns>
		internal bool HasName(String cname)
		{
			if (cname == null) return false;

			return Dict.ContainsKey(cname);
		}

		/// <summary>
		/// The configuration by name.
		/// </summary>
		/// <param name="name">The name of a configuration.</param>
		/// <returns>The configuration of looking by name.</returns>
		internal IFCExporterConfiguration this[String name]
		{
			get
			{
				if (string.IsNullOrEmpty(name)) return null;
				if (!Dict.ContainsKey(name)) return null;
				return Dict[name];
			}
		}

		/// <summary>
		/// The configurations in the map.
		/// </summary>
		internal IEnumerable<IFCExporterConfiguration> Values
		{
			get { return Dict.Values; }
		}

		public void AddSavedConfigurations(string fdir)
		{
			if (string.IsNullOrEmpty(fdir) || !Directory.Exists(fdir)) return;

			foreach (string fpath in Directory.EnumerateFiles(fdir, "*.3bic", SearchOption.AllDirectories))
			{
				// create a new IfcExporterConfiguration from fpath
				// add it to Dict using the name from either fpath or the one internal
			}
		}
	}
}
