using System;
using System.IO;
using System.Windows;

namespace SavageTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (MainWindow.DataContext == null)
            {
                var mainVM = new MainVM();
                var mainBook = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), @"Settings\Core.setting");
                mainVM.CharacterGenerator.LoadSetting(new FileInfo(mainBook));
                mainVM.CharacterGenerator.SelectedArchetype = mainVM.CharacterGenerator.Archetypes[0];

                MainWindow.DataContext = mainVM;
            }
        }
    }
}
