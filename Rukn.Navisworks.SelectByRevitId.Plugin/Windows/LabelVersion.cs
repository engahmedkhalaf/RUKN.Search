namespace Rukn.Navisworks.SelectByRevitId.Plugin
{
    public class LabelVersion
    {
        public static void Update(MainWindowViewModel mainWindowViewModel)
        {
            string version = SettingsConfig.currentVersion;

            mainWindowViewModel.Version = version;
        }
    }
}
