using Autodesk.Navisworks.Api.Plugins;
using Rukn.Navisworks.SelectByRevitId.Common.Application;

namespace Rukn.Navisworks.SelectByRevitId.Plugin
{
    [Plugin("ModelProcessing", IdentityInformation.DeveloperID, ToolTip = "Model Processing & Viewpoints Generator", DisplayName = "RUKNBIM")]
    public class ModelProcessingPlugin : CustomPlugin
    {
        public int Execute(params string[] parameters)
        {
            ModelProcessingWindow window = new ModelProcessingWindow();
            window.ShowDialog();
            return 0;
        }
    }
}
