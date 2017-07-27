using SavageTools.Characters;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Tortuga.Sails;

namespace SavageTools
{
    public class CharacterVM : ViewModelBase
    {
        public CharacterVM(Character character) { Character = character; }

        public Character Character { get; }

        public ICommand CopyCommand => GetCommand(CopyToClipboard);


        public void CopyToClipboard()
        {
            Clipboard.SetText(CopyToString());
        }

        public string CopyToString()
        {
            var c = Character;
            var result = new StringBuilder();
            result.AppendLine($"Name {c.Name}, Archetype {c.Archetype}, Race {c.Race}");
            result.AppendLine($"Agility {c.Agility}, Smarts {c.Smarts}, Strength {c.Strength}, Spirt {c.Spirit}, Vigor {c.Vigor}");

            result.AppendLine($"Charisma {c.Charisma}, Parry {c.ParryTotal}, Toughness {c.ToughnessTotal}, Pace {c.Pace}+{c.Running}, Strain {c.Strain}/{c.MaximumStrainTotal}");

            result.AppendLine(string.Join(", ", c.Skills.Select(s => s.LongName)));
            result.AppendLine(string.Join(", ", c.Edges.Select(e => e.Name + ": " + e.Description)));
            result.AppendLine(string.Join(", ", c.Hindrances.Select(h => h.Name + " " + h.LevelName + ": " + h.Description)));

            foreach (var group in c.PowerGroups)
                result.AppendLine($"{group.Skill}, Power Points {group.PowerPoints}, Powers: {string.Join(", ", group.Powers.Select(p => p.LongName))}");

            return result.ToString();
        }
    }
}
