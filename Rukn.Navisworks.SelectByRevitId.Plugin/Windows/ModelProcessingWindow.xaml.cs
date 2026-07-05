using System;
using System.Windows;
using System.Windows.Input;

namespace Rukn.Navisworks.SelectByRevitId.Plugin
{
    /// <summary>
    /// Interaction logic for ModelProcessingWindow.xaml
    /// </summary>
    public partial class ModelProcessingWindow : Window
    {
        public ModelProcessingWindow()
        {
            InitializeComponent();
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

        private void ReadModels_Click(object sender, RoutedEventArgs e)
        {
            TextBlockStatus.Text = "Reading models from source...";
            // TODO: Add model reading logic here
        }

        private void GenerateViewpoints_Click(object sender, RoutedEventArgs e)
        {
            TextBlockStatus.Text = "Generating viewpoints based on reference settings...";
            // TODO: Add viewpoint generation logic here
        }

        private void SaveViewpoints_Click(object sender, RoutedEventArgs e)
        {
            TextBlockStatus.Text = "Saving viewpoints to active document...";
            // TODO: Add save viewpoints logic here
        }

        private void ExportData_Click(object sender, RoutedEventArgs e)
        {
            TextBlockStatus.Text = "Exporting data...";
            // TODO: Add data export logic here
        }
    }
}
