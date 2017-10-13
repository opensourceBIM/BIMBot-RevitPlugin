#region
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

#endregion

namespace BimServerExchange.Runtime
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class ElementCollectionHelper
   {
      /// <summary>
      ///    Geef alle gelinkte files
      /// </summary>
      /// <param name="app"></param>
      /// <param name="addProj"></param>
      /// <returns></returns>
      public static List<string> GetLinkedFiles(UIApplication app, bool addProj)
      {
         List<string> linkedProjects = new List<string>();
         List<RevitLinkInstance> instantiekes =
            GetAllProjectElements(app.ActiveUIDocument.Document)
               .OfType<RevitLinkInstance>()
               .Where(c => c.Name.ToLower().Contains(".rvt"))
               .ToList();
         foreach (RevitLinkInstance rli in instantiekes)
         {
            if (rli.GetLinkDocument() == null) continue;
            string linknaam = rli.GetLinkDocument().PathName;
            if (!linkedProjects.Contains(linknaam))
            {
               linkedProjects.Add(linknaam);
            }
         }
         if (linkedProjects.Count == 0)
         {
            // er zijn waarschijnlijk worksets gebruikt
            ModelPath mdlPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(app.ActiveUIDocument.Document.PathName);
            TransmissionData transData = TransmissionData.ReadTransmissionData(mdlPath);
            if (transData != null)
            {
               ICollection<ElementId> externalReferences = transData.GetAllExternalFileReferenceIds();
               foreach (ElementId refId in externalReferences)
               {
                  ExternalFileReference curRef = transData.GetLastSavedReferenceData(refId);
                  string refType = curRef.ExternalFileReferenceType.ToString();
                  string refPath = ModelPathUtils.ConvertModelPathToUserVisiblePath(curRef.GetAbsolutePath());
                  if (refType == "RevitLink") linkedProjects.Add(refPath);
               }
            }
         }
         if (addProj)
         {
            linkedProjects.Add(app.ActiveUIDocument.Document.PathName);
         }
         //laatste check om te kijken of de links ook gevonden worden
         return linkedProjects.Where(File.Exists).ToList();
      }

      // bug683 fix: functie toegevoegd zodat IFC exporter selectief kan rapporteren welke files missen
      /// <summary>
      ///    Geef alle gelinkte files die niet langer gevonden kunnen worden
      /// </summary>
      /// <param name="app"></param>
      /// <param name="addProj"></param>
      /// <returns></returns>
      // ReSharper disable once UnusedMember.Global
      public static List<string> GetMissingLinkedFiles(UIApplication app, bool addProj)
      {
         List<string> linkedProjects = new List<string>();
         if (linkedProjects.Count == 0)
         {
            // er zijn waarschijnlijk worksets gebruikt
            ModelPath mdlPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(app.ActiveUIDocument.Document.PathName);
            TransmissionData transData = TransmissionData.ReadTransmissionData(mdlPath);
            if (transData != null)
            {
               ICollection<ElementId> externalReferences = transData.GetAllExternalFileReferenceIds();
               foreach (ElementId refId in externalReferences)
               {
                  ExternalFileReference curRef = transData.GetLastSavedReferenceData(refId);
                  string refType = curRef.ExternalFileReferenceType.ToString();
                  string refPath = ModelPathUtils.ConvertModelPathToUserVisiblePath(curRef.GetAbsolutePath());
                  if (refType == "RevitLink") linkedProjects.Add(refPath);
               }
            }
         }

         if (addProj)
         {
            linkedProjects.Add(app.ActiveUIDocument.Document.PathName);
         }

         //laatste check om te kijken of de links ook gevonden worden
         return linkedProjects.Where(p => (false == File.Exists(p))).ToList();
      }

		/// <summary>
      ///    Alle elementen bepalen, dus niet alleen de elementen uit de actieve view
      /// </summary>
      /// <param name="doc"></param>
      /// <returns></returns>
      public static List<Element> GetAllProjectElements(Document doc)
      {
         List<Element> elementList;
         FilteredElementCollector elemTypeCtor = (new FilteredElementCollector(doc)).WhereElementIsElementType();
         FilteredElementCollector notElemTypeCtor = (new FilteredElementCollector(doc)).WhereElementIsNotElementType();
         FilteredElementCollector allElementCtor = elemTypeCtor.UnionWith(notElemTypeCtor);
         try
         {
            elementList = !allElementCtor.Any() ? new List<Element>() : allElementCtor.ToList();
         }
         catch
         {
            elementList = new List<Element>();
         }
         return elementList;
      }
   }
}