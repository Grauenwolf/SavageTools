using System;
using System.IO;
using System.Linq;
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
                var mainBook = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), @"Settings\Core.savage-setting");
                mainVM.CharacterGenerator.LoadSetting(new FileInfo(mainBook));

                mainVM.CharacterGeneratorSettings.SelectedArchetype = mainVM.CharacterGenerator.Archetypes[0];
                mainVM.CharacterGeneratorSettings.SelectedRace = mainVM.CharacterGenerator.Races.First(r => r.Name == "Human");
                mainVM.CharacterGeneratorSettings.SelectedRank = mainVM.CharacterGenerator.Ranks[0];

                MainWindow.DataContext = mainVM;
            }
        }
    }
}
