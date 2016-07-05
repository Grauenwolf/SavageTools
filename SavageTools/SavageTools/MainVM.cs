using SavageTools.Characters;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Tortuga.Sails;

namespace SavageTools
{
    public class MainVM : ViewModelBase
    {

        public ICommand LoadSettingCommand
        {
            get { return GetCommand(LoadSetting); }
        }
        public ICommand CreateCharacterCommand
        {
            get { return GetCommand(CreateCharacter); }
        }

        void CreateCharacter()
        {
            Characters.Add(CharacterGenerator.GenerateCharacter());
        }

        void LoadSetting()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".setting";
            dlg.Filter = "Savage Worlds Setting (.savage-setting)|*.savage-setting|All Files (.*)|*.*";
            dlg.Multiselect = true;
            dlg.InitialDirectory = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Settings");

            if (dlg.ShowDialog() != true)
                return;

            foreach (var file in dlg.FileNames.Select(f => new FileInfo(f)))
                CharacterGenerator.LoadSetting(file);

            if (CharacterGenerator.SelectedArchetype == null)
                CharacterGenerator.SelectedArchetype = CharacterGenerator.Archetypes.FirstOrDefault();


        }

        public CharacterGenerator CharacterGenerator { get { return GetNew<CharacterGenerator>(); } }

        public ObservableCollection<Character> Characters { get { return GetNew<ObservableCollection<Character>>(); } }


    }
}


