using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Tortuga.Sails;

namespace SavageTools
{
    public class MainVM : ViewModelBase
    {
        public MainVM()
        {
            CharacterGenerator = new CharacterGenerator(new FileInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Settings", "Core.savage-setting")));
        }

        public CharacterGenerator CharacterGenerator { get => Get<CharacterGenerator>(); set => Set(value); }

        public ObservableCollection<CharacterVM> Characters => GetNew<ObservableCollection<CharacterVM>>();

        public ICommand CopyAllCommand => GetCommand(CopyAll);

        public ICommand CreateCharacterCommand => GetCommand(() => CreateCharacter());

        public ICommand LoadSettingCommand => GetCommand(LoadSetting);

        void CopyAll()
        {
            var result = new StringBuilder();
            foreach (var c in Characters)
            {
                result.AppendLine(c.CopyToString());
                result.AppendLine();
                result.AppendLine();
            }
            Clipboard.SetText(result.ToString());
        }

        void CreateCharacter()
        {
            var characterVM = new CharacterVM(CharacterGenerator.GenerateCharacter());
            characterVM.RemoveMe += (s, e) => Characters.Remove((CharacterVM)s);
            Characters.Add(characterVM);
        }


        void LoadSetting()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".setting",
                Filter = "Savage Worlds Setting (.savage-setting)|*.savage-setting|All Files (.*)|*.*",
                Multiselect = true,
                InitialDirectory = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Settings")
            };

            if (dlg.ShowDialog() != true)
                return;

            foreach (var file in dlg.FileNames.Select(f => new FileInfo(f)))
                CharacterGenerator.LoadSetting(file);

            if (CharacterGenerator.SelectedArchetype == null)
                CharacterGenerator.SelectedArchetype = CharacterGenerator.Archetypes.FirstOrDefault();


        }
    }
}


