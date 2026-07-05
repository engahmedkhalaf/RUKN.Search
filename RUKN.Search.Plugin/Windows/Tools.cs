using Autodesk.Navisworks.Api;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RUKN.Search.Plugin.Utils
{
    public class Tools
    {
        public static string SelectedIds;
        private static StringBuilder strBuilder;
        public static Document Doc { get; set; }
        public static ModelItemCollection CurrentSelection { get; set; }

        /// <summary>
        /// Convert input string to int
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<string> splitString(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return new List<string>();

            string trimmedInput = s.Trim();
            if (trimmedInput == "Insert Element Id")
                return new List<string>();

            char delimit = ',';
            return s.Split(delimit)
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();
        }

        internal static void GetIds(ModelItemCollection modelItemCollection)
        {
            foreach (var ModelItem in modelItemCollection)
            {
                if (ModelItem.HasGeometry || ModelItem.IsInsert)
                {
                    ModelItemCollection mic = new ModelItemCollection
                    {
                        ModelItem.Parent
                    };
                    Tools.GetIds(mic);
                    break;
                }
                if (ModelItem.IsCollection && ModelItem.IsLayer)
                {
                    ModelItemCollection mic = new ModelItemCollection();
                    mic.AddRange(ModelItem.Children);
                    Tools.GetIds(mic);
                }
                else
                {
                    var property = ModelItem.PropertyCategories.FindPropertyByName("LcRevitId", "LcOaNat64AttributeValue");
                    if (property != null && property.Value != null)
                    {
                        var id = property.Value.ToDisplayString();
                        strBuilder = new StringBuilder();
                        strBuilder.Append(SelectedIds);
                        strBuilder.Append(id);
                        strBuilder.Append(",");
                        SelectedIds = strBuilder.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Get selection from int Revit ID
        /// </summary>
        /// <param name="li"></param>
        /// <returns></returns>
        public static ModelItemCollection getElements(List<string> li)
        {
            if (li == null || li.Count == 0)
            {
                MessageWindow.Show("Warning", "Please insert one or more Revit Element IDs.");
                return new ModelItemCollection();
            }

            // Execute Search
            ModelItemCollection items = new ModelItemCollection();
            global::Autodesk.Navisworks.Api.Search search = new global::Autodesk.Navisworks.Api.Search();
            search.Selection.SelectAll();
            List<string> list = li.Distinct().ToList();
            for (int i = 0; i < list.Count(); i++)
            {
                string st = list[i];
                search.SearchConditions.Add(SearchCondition.HasPropertyByName("LcRevitId", "LcOaNat64AttributeValue").EqualValue(VariantData.FromDisplayString(st)));
                var item = search.FindFirst(Autodesk.Navisworks.Api.Application.ActiveDocument, false);
                if (item != null)
                    items.Add(item);
                search.Clear();
            }
            if (items.Count != list.Count) 
            {
                if (items.Count == 0)
                {
                    MessageWindow.Show("Warning", "No matching Revit Element IDs were found in the model.");
                }
                else
                {
                    MessageWindow.Show("Warning", $"Only {items.Count} out of {list.Count} Revit Element IDs were found.");
                }
            }

            Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.CopyFrom(items);

            return items;
        }
        public static bool IsRevitModelLoaded()
        {
            var activeDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            return activeDoc.ActiveSheet != null;
        }
    }
}