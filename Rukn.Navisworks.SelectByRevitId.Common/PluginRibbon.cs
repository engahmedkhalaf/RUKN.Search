using Autodesk.Navisworks.Api.Plugins;
using Rukn.Navisworks.SelectByRevitId.Common.Application;
using Rukn.Navisworks.SelectByRevitId.Common.Utils;
using Rukn.Navisworks.SelectByRevitId.Plugin;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Rukn.Navisworks.Plugin.Common
{
    [Plugin("SelectByRevitIdRibbon", IdentityInformation.DeveloperID, DisplayName = "RUKNBIM")]
    [RibbonLayout("PluginRibbon.xaml")]
    [RibbonTab("RUKNBIM", DisplayName = "RUKNBIM")]
    [Command("SelectByRevitId", Icon = "ElementID_16.ico", LargeIcon = "ElementID_32.png", ToolTip = "Select elements using their original Revit ID", DisplayName = "SelectByRevitId")]
    public class PluginRibbon : CommonCommandHandlerPlugin
    {
        public override int ExecuteCommand(string name, params string[] parameters)
        {
            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pluginFileName = directoryName + $"\\{Assembly.GetExecutingAssembly().GetName().Name}.dll";
            Autodesk.Navisworks.Api.Application.Plugins.AddPluginAssembly(pluginFileName);
            switch (name)
            {
                case "SelectByRevitId":
                    try
                    {
                        if (!Autodesk.Navisworks.Api.Application.IsAutomated)
                        {
                            PluginBuilder pluginBuilder = new PluginBuilder("SelectByRevitId");
                            if (pluginBuilder.pluginRecord is CustomPluginRecord && pluginBuilder.pluginRecord.IsEnabled)
                            {
                                SelectByIdPlugin selectByIdPlugin = (SelectByIdPlugin)(pluginBuilder.pluginRecord.LoadedPlugin ?? pluginBuilder.pluginRecord.LoadPlugin());
                                selectByIdPlugin.Execute();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ups, something went wrong" + Environment.NewLine + ex.Message);
                    }
                    break;
            }
            return 0;
        }

        public override bool TryShowCommandHelp(string name)
        {
            bool result = base.TryShowCommandHelp("https://www.ruknbim.com/");
            return result;
        }
    }
}