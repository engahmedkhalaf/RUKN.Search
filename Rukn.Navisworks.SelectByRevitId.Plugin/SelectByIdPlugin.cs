using Autodesk.Navisworks.Api.Plugins;
using Rukn.Navisworks.SelectByRevitId.Common.Application;
using Rukn.Navisworks.SelectByRevitId.Plugin.Utils;


namespace Rukn.Navisworks.SelectByRevitId.Plugin
{
    [Plugin("SelectByRevitId", IdentityInformation.DeveloperID, ToolTip = "Select by revit element ID", DisplayName = "RUKNBIM")]
    public class SelectByIdPlugin : CustomPlugin
    {
        public int Execute(params string[] parameters)
        {
            Tools.SelectedIds = "";

            if (!Tools.IsRevitModelLoaded())
            {
                MessageWindow.Show("Warning", "You don't have a Revit model loaded, please load one");
            }
            else
            {
                SelectByIdWindow selectByIdWindow = new SelectByIdWindow();
                selectByIdWindow.ShowDialog();
            }

            return 0;
        }
    }
}