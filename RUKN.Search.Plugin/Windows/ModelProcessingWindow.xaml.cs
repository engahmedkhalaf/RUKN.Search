using System;
using System.Windows;
using System.Windows.Input;

namespace RUKN.Search.Plugin
{
    /// <summary>
    /// Interaction logic for ModelProcessingWindow.xaml
    /// </summary>
    public partial class ModelProcessingWindow : Window
    {
        public ModelProcessingWindow()
        {
            InitializeComponent();
            PopulateModels();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Close_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Ruknbim_Link(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://www.ruknbim.com/");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link: " + ex.Message);
            }
        }

        private bool _sidePlanesEnabled = false;

        private void TogglePlanes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var state = Autodesk.Navisworks.Api.ComApi.ComApiBridge.State;
                var curView = state.CurrentView;
                var clipColl = (Autodesk.Navisworks.Api.Interop.ComApi.InwClippingPlaneColl2)curView.ClippingPlanes();

                _sidePlanesEnabled = !_sidePlanesEnabled;

                if (_sidePlanesEnabled)
                {
                    var doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
                    Autodesk.Navisworks.Api.BoundingBox3D bbox = null;

                    if (!doc.CurrentSelection.IsEmpty)
                    {
                        foreach (var item in doc.CurrentSelection.SelectedItems)
                        {
                            var box = item.BoundingBox();
                            if (box != null)
                            {
                                if (bbox == null)
                                {
                                    bbox = box;
                                }
                                else
                                {
                                    double minX = System.Math.Min(bbox.Min.X, box.Min.X);
                                    double minY = System.Math.Min(bbox.Min.Y, box.Min.Y);
                                    double minZ = System.Math.Min(bbox.Min.Z, box.Min.Z);
                                    double maxX = System.Math.Max(bbox.Max.X, box.Max.X);
                                    double maxY = System.Math.Max(bbox.Max.Y, box.Max.Y);
                                    double maxZ = System.Math.Max(bbox.Max.Z, box.Max.Z);
                                    bbox = new Autodesk.Navisworks.Api.BoundingBox3D(
                                        new Autodesk.Navisworks.Api.Point3D(minX, minY, minZ),
                                        new Autodesk.Navisworks.Api.Point3D(maxX, maxY, maxZ)
                                    );
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ComboModel.SelectedItem != null)
                        {
                            string selectedModel = ComboModel.SelectedItem.ToString();
                            foreach (Autodesk.Navisworks.Api.Model model in doc.Models)
                            {
                                string name = model.RootItem != null ? model.RootItem.DisplayName : System.IO.Path.GetFileNameWithoutExtension(model.SourceFileName);
                                if (name == selectedModel && model.RootItem != null)
                                {
                                    bbox = model.RootItem.BoundingBox();
                                    break;
                                }
                            }
                        }
                    }

                    if (bbox != null)
                    {
                        if (clipColl.Count < 6) clipColl.CreatePlane(6);

                        // Plane 3: Left (normal 1, 0, 0)
                        var p3 = (Autodesk.Navisworks.Api.Interop.ComApi.InwOaClipPlane)clipColl[3];
                        var n3 = (Autodesk.Navisworks.Api.Interop.ComApi.InwLUnitVec3f)state.ObjectFactory(Autodesk.Navisworks.Api.Interop.ComApi.nwEObjectType.eObjectType_nwLUnitVec3f, null, null);
                        n3.SetValue(1, 0, 0);
                        var plane3 = (Autodesk.Navisworks.Api.Interop.ComApi.InwLPlane3f)state.ObjectFactory(Autodesk.Navisworks.Api.Interop.ComApi.nwEObjectType.eObjectType_nwLPlane3f, null, null);
                        plane3.SetValue(n3, bbox.Min.X);
                        p3.Plane = plane3;
                        p3.Enabled = true;

                        // Plane 4: Right (normal -1, 0, 0)
                        var p4 = (Autodesk.Navisworks.Api.Interop.ComApi.InwOaClipPlane)clipColl[4];
                        var n4 = (Autodesk.Navisworks.Api.Interop.ComApi.InwLUnitVec3f)state.ObjectFactory(Autodesk.Navisworks.Api.Interop.ComApi.nwEObjectType.eObjectType_nwLUnitVec3f, null, null);
                        n4.SetValue(-1, 0, 0);
                        var plane4 = (Autodesk.Navisworks.Api.Interop.ComApi.InwLPlane3f)state.ObjectFactory(Autodesk.Navisworks.Api.Interop.ComApi.nwEObjectType.eObjectType_nwLPlane3f, null, null);
                        plane4.SetValue(n4, -bbox.Max.X);
                        p4.Plane = plane4;
                        p4.Enabled = true;

                        // Plane 5: Front (normal 0, 1, 0)
                        var p5 = (Autodesk.Navisworks.Api.Interop.ComApi.InwOaClipPlane)clipColl[5];
                        var n5 = (Autodesk.Navisworks.Api.Interop.ComApi.InwLUnitVec3f)state.ObjectFactory(Autodesk.Navisworks.Api.Interop.ComApi.nwEObjectType.eObjectType_nwLUnitVec3f, null, null);
                        n5.SetValue(0, 1, 0);
                        var plane5 = (Autodesk.Navisworks.Api.Interop.ComApi.InwLPlane3f)state.ObjectFactory(Autodesk.Navisworks.Api.Interop.ComApi.nwEObjectType.eObjectType_nwLPlane3f, null, null);
                        plane5.SetValue(n5, bbox.Min.Y);
                        p5.Plane = plane5;
                        p5.Enabled = true;

                        // Plane 6: Back (normal 0, -1, 0)
                        var p6 = (Autodesk.Navisworks.Api.Interop.ComApi.InwOaClipPlane)clipColl[6];
                        var n6 = (Autodesk.Navisworks.Api.Interop.ComApi.InwLUnitVec3f)state.ObjectFactory(Autodesk.Navisworks.Api.Interop.ComApi.nwEObjectType.eObjectType_nwLUnitVec3f, null, null);
                        n6.SetValue(0, -1, 0);
                        var plane6 = (Autodesk.Navisworks.Api.Interop.ComApi.InwLPlane3f)state.ObjectFactory(Autodesk.Navisworks.Api.Interop.ComApi.nwEObjectType.eObjectType_nwLPlane3f, null, null);
                        plane6.SetValue(n6, -bbox.Max.Y);
                        p6.Plane = plane6;
                        p6.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 3; i <= System.Math.Min(6, clipColl.Count); i++)
                    {
                        var plane = (Autodesk.Navisworks.Api.Interop.ComApi.InwOaClipPlane)clipColl[i];
                        plane.Enabled = false;
                    }
                }

                state.CurrentView = curView;
                TextBlockStatus.Text = _sidePlanesEnabled ? "Side planes enabled." : "Side planes disabled.";
            }
            catch (System.Exception ex)
            {
                TextBlockStatus.Text = "Error: " + ex.Message;
            }
        }

        private void GenerateViewpoints_Click(object sender, RoutedEventArgs e)
        {
            if (ComboModel.SelectedItem == null)
            {
                TextBlockStatus.Text = "Error: No model selected.";
                return;
            }

            string selectedModel = ComboModel.SelectedItem.ToString();
            if (selectedModel == "No models loaded")
            {
                TextBlockStatus.Text = "Error: No models loaded.";
                return;
            }

            // Read Offset values
            double? offsetTop = null;
            if (CheckOffsetTop.IsChecked == true)
            {
                double val;
                if (double.TryParse(TextOffsetTop.Text, out val))
                    offsetTop = val;
            }

            double? offsetBottom = null;
            if (CheckOffsetBottom.IsChecked == true)
            {
                double val;
                if (double.TryParse(TextOffsetBottom.Text, out val))
                    offsetBottom = val;
            }

            // Find checked levels
            var checkedLevels = new System.Collections.Generic.List<string>();
            foreach (var child in PanelLevels.Children)
            {
                if (child is System.Windows.Controls.StackPanel sp && sp.Tag is System.Windows.Controls.CheckBox cb && cb.IsChecked == true)
                {
                    foreach (var subChild in sp.Children)
                    {
                        if (subChild is System.Windows.Controls.TextBlock tb)
                        {
                            checkedLevels.Add(tb.Text);
                            break;
                        }
                    }
                }
            }

            if (checkedLevels.Count == 0)
            {
                TextBlockStatus.Text = "Error: No levels selected.";
                return;
            }

            TextBlockStatus.Text = $"Generating {checkedLevels.Count} viewpoint(s)...";

            int generatedCount = 0;
            try
            {
                var doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
                
                foreach (string levelName in checkedLevels)
                {
                    double? elevation = GetLevelElevation(selectedModel, levelName);
                    if (elevation.HasValue)
                    {
                        double? cutTopZ = offsetTop.HasValue ? (double?)(elevation.Value + offsetTop.Value) : null;
                        double? cutBottomZ = offsetBottom.HasValue ? (double?)(elevation.Value + offsetBottom.Value) : null;

                        // Apply the section cut using COM API
                        ApplySectionCut(cutTopZ, cutBottomZ);

                        // Capture current view state into a new viewpoint
                        Autodesk.Navisworks.Api.Viewpoint vp = doc.CurrentViewpoint.CreateCopy();
                        
                        // Create a SavedViewpoint
                        Autodesk.Navisworks.Api.SavedViewpoint savedVp = new Autodesk.Navisworks.Api.SavedViewpoint(vp);
                        string suffix = "";
                        if (offsetTop.HasValue || offsetBottom.HasValue)
                        {
                            suffix = $" (Top:{TextOffsetTop.Text}m, Bot:{TextOffsetBottom.Text}m)";
                        }
                        savedVp.DisplayName = $"{selectedModel} - {levelName}{suffix}";

                        // Save to document
                        doc.SavedViewpoints.AddCopy(savedVp);
                        generatedCount++;
                    }
                }

                // Reset sectioning on the current live view to avoid leaving the screen cut
                ClearSectioning();

                TextBlockStatus.Text = $"Successfully generated {generatedCount} viewpoint(s)!";
            }
            catch (System.Exception ex)
            {
                TextBlockStatus.Text = "Error: " + ex.Message;
                System.Windows.MessageBox.Show("Viewpoint generation failed: " + ex.Message);
            }
        }

        private double? GetLevelElevation(string selectedModelName, string levelNameName)
        {
            try
            {
                if (Autodesk.Navisworks.Api.Application.ActiveDocument != null)
                {
                    foreach (Autodesk.Navisworks.Api.Model model in Autodesk.Navisworks.Api.Application.ActiveDocument.Models)
                    {
                        string modelName = model.RootItem != null ? model.RootItem.DisplayName : System.IO.Path.GetFileNameWithoutExtension(model.SourceFileName);
                        if (modelName == selectedModelName)
                        {
                            if (model.RootItem != null)
                            {
                                foreach (Autodesk.Navisworks.Api.ModelItem child in model.RootItem.Children)
                                {
                                    if (child.DisplayName == levelNameName)
                                    {
                                        var bbox = child.BoundingBox();
                                        if (bbox != null)
                                        {
                                            return bbox.Min.Z;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch (System.Exception) { }
            return null;
        }

        private void ApplySectionCut(double? topZ, double? bottomZ)
        {
            try
            {
                var state = Autodesk.Navisworks.Api.ComApi.ComApiBridge.State;
                var curView = state.CurrentView;
                var clipColl = (Autodesk.Navisworks.Api.Interop.ComApi.InwClippingPlaneColl2)curView.ClippingPlanes();

                // Ensure we have at least 2 planes
                if (clipColl.Count < 2)
                {
                    clipColl.CreatePlane(2);
                }

                // Plane 1: Top cut (normal 0, 0, -1)
                var plane1 = (Autodesk.Navisworks.Api.Interop.ComApi.InwOaClipPlane)clipColl[1];
                if (topZ.HasValue)
                {
                    var normal = (Autodesk.Navisworks.Api.Interop.ComApi.InwLUnitVec3f)state.ObjectFactory(
                        Autodesk.Navisworks.Api.Interop.ComApi.nwEObjectType.eObjectType_nwLUnitVec3f, null, null);
                    normal.SetValue(0, 0, -1);

                    var plane = (Autodesk.Navisworks.Api.Interop.ComApi.InwLPlane3f)state.ObjectFactory(
                        Autodesk.Navisworks.Api.Interop.ComApi.nwEObjectType.eObjectType_nwLPlane3f, null, null);
                    
                    plane.SetValue(normal, -topZ.Value);
                    plane1.Plane = plane;
                    plane1.Enabled = true;
                }
                else
                {
                    plane1.Enabled = false;
                }

                // Plane 2: Bottom cut (normal 0, 0, 1)
                var plane2 = (Autodesk.Navisworks.Api.Interop.ComApi.InwOaClipPlane)clipColl[2];
                if (bottomZ.HasValue)
                {
                    var normal = (Autodesk.Navisworks.Api.Interop.ComApi.InwLUnitVec3f)state.ObjectFactory(
                        Autodesk.Navisworks.Api.Interop.ComApi.nwEObjectType.eObjectType_nwLUnitVec3f, null, null);
                    normal.SetValue(0, 0, 1);

                    var plane = (Autodesk.Navisworks.Api.Interop.ComApi.InwLPlane3f)state.ObjectFactory(
                        Autodesk.Navisworks.Api.Interop.ComApi.nwEObjectType.eObjectType_nwLPlane3f, null, null);
                    
                    plane.SetValue(normal, bottomZ.Value);
                    plane2.Plane = plane;
                    plane2.Enabled = true;
                }
                else
                {
                    plane2.Enabled = false;
                }

                state.CurrentView = curView;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show("Error applying section cut: " + ex.Message);
            }
        }

        private void ClearSectioning()
        {
            try
            {
                var state = Autodesk.Navisworks.Api.ComApi.ComApiBridge.State;
                var curView = state.CurrentView;
                var clipColl = (Autodesk.Navisworks.Api.Interop.ComApi.InwClippingPlaneColl2)curView.ClippingPlanes();
                for (int i = 1; i <= clipColl.Count; i++)
                {
                    var plane = (Autodesk.Navisworks.Api.Interop.ComApi.InwOaClipPlane)clipColl[i];
                    plane.Enabled = false;
                }
                state.CurrentView = curView;
                _sidePlanesEnabled = false;
            }
            catch (System.Exception) { }
        }

        private void PopulateModels()
        {
            ComboModel.Items.Clear();
            try
            {
                if (Autodesk.Navisworks.Api.Application.ActiveDocument != null)
                {
                    foreach (Autodesk.Navisworks.Api.Model model in Autodesk.Navisworks.Api.Application.ActiveDocument.Models)
                    {
                        string name = model.RootItem != null ? model.RootItem.DisplayName : System.IO.Path.GetFileNameWithoutExtension(model.SourceFileName);
                        if (!string.IsNullOrEmpty(name))
                        {
                            ComboModel.Items.Add(name);
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                // Silent catch
            }

            if (ComboModel.Items.Count > 0)
            {
                ComboModel.SelectedIndex = 0;
            }
            else
            {
                ComboModel.Items.Add("No models loaded");
                ComboModel.SelectedIndex = 0;
            }
        }

        private void PopulateLevels(string selectedModelName)
        {
            PanelLevels.Children.Clear();
            if (string.IsNullOrEmpty(selectedModelName) || selectedModelName == "No models loaded")
            {
                return;
            }

            try
            {
                if (Autodesk.Navisworks.Api.Application.ActiveDocument != null)
                {
                    foreach (Autodesk.Navisworks.Api.Model model in Autodesk.Navisworks.Api.Application.ActiveDocument.Models)
                    {
                        string modelName = model.RootItem != null ? model.RootItem.DisplayName : System.IO.Path.GetFileNameWithoutExtension(model.SourceFileName);
                        if (modelName == selectedModelName)
                        {
                            if (model.RootItem != null)
                            {
                                foreach (Autodesk.Navisworks.Api.ModelItem child in model.RootItem.Children)
                                {
                                    string levelName = child.DisplayName;
                                    if (!string.IsNullOrEmpty(levelName))
                                    {
                                        var itemPanel = new System.Windows.Controls.StackPanel();
                                        itemPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
                                        itemPanel.Margin = new Thickness(0, 0, 0, 8);

                                        // 1. Checkbox
                                        var cb = new System.Windows.Controls.CheckBox();
                                        cb.IsChecked = true;
                                        cb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                                        cb.Margin = new Thickness(0, 0, 6, 0);

                                        // 2. Revit Level symbol (WPF vector geometry)
                                        var symbolGrid = new System.Windows.Controls.Grid();
                                        symbolGrid.Width = 14;
                                        symbolGrid.Height = 14;
                                        symbolGrid.Margin = new Thickness(0, 0, 8, 0);
                                        symbolGrid.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                                        // Outer circle
                                        var circle = new System.Windows.Shapes.Ellipse();
                                        circle.Stroke = System.Windows.Media.Brushes.Black;
                                        circle.StrokeThickness = 1;
                                        symbolGrid.Children.Add(circle);

                                        // Top-Right filled quadrant
                                        var path1 = new System.Windows.Shapes.Path();
                                        path1.Fill = System.Windows.Media.Brushes.Black;
                                        path1.Data = System.Windows.Media.Geometry.Parse("M 7,7 L 7,0 A 7,7 0 0,1 14,7 Z");
                                        symbolGrid.Children.Add(path1);

                                        // Bottom-Left filled quadrant
                                        var path2 = new System.Windows.Shapes.Path();
                                        path2.Fill = System.Windows.Media.Brushes.Black;
                                        path2.Data = System.Windows.Media.Geometry.Parse("M 7,7 L 0,7 A 7,7 0 0,1 7,14 Z");
                                        symbolGrid.Children.Add(path2);

                                        itemPanel.Children.Add(cb);
                                        itemPanel.Children.Add(symbolGrid);

                                        // 3. Text label
                                        var tb = new System.Windows.Controls.TextBlock();
                                        tb.Text = levelName;
                                        tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                                        tb.Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#18263c");
                                        tb.FontSize = 12;
                                        itemPanel.Children.Add(tb);

                                        // Store a reference to checkbox on the itemPanel tag
                                        itemPanel.Tag = cb;

                                        PanelLevels.Children.Add(itemPanel);
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                // Silent catch
            }
        }

        private void ComboModel_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ComboModel.SelectedItem != null)
            {
                string selectedModel = ComboModel.SelectedItem.ToString();
                PopulateLevels(selectedModel);
            }
        }
    }
}
