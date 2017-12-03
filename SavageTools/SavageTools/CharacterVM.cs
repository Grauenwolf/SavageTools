using SavageTools.Characters;
using System;
using System.Windows;
using System.Windows.Input;
using Tortuga.Sails;

namespace SavageTools
{
    public class CharacterVM : ViewModelBase
    {
        public CharacterVM(Character character) { Character = character; }

        public event EventHandler RemoveMe;

        public Character Character { get; }

        public ICommand CopyCommand => GetCommand(CopyToClipboard);
        public ICommand RemoveCommand => GetCommand(() => RemoveMe?.Invoke(this, EventArgs.Empty));
        public void CopyToClipboard() => Clipboard.SetText(CopyToString());

        public string CopyToString()
        {
            //TOOD: Add HTML and Markdown options

            var result = new StoryBuilder();

            Character.CopyToStory(result);

            return result.ToString();
        }
    }
}
