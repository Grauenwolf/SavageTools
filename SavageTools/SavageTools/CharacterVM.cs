using SavageTools.Characters;
using System;
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
        public ICommand RemoveCommand => GetCommand(() => RemoveMe?.Invoke(this, EventArgs.Empty));


        public event EventHandler RemoveMe;

        public void CopyToClipboard()
        {
            Clipboard.SetText(CopyToString());
        }

        public string CopyToString()
        {
            var c = Character;
            var result = new StringBuilder();

            result.Append($"{c.Name} ({c.Gender})");
            if (!string.IsNullOrWhiteSpace(c.Archetype) && c.Archetype != "(None)")
                result.Append($", {c.Archetype}");
            if (c.Race != "Human")
                result.Append($", {c.Race}");
            if (c.Rank != "Novice")
                result.Append($", Rank {c.Rank}");
            result.AppendLine();

            result.AppendLine($"Agility {c.Agility}, Smarts {c.Smarts}, Strength {c.Strength}, Spirt {c.Spirit}, Vigor {c.Vigor}");

            result.Append($"Charisma {c.Charisma}, Parry {c.ParryTotal}, Toughness {c.ToughnessTotal}, Pace {c.Pace}+{c.Running}, Size {c.Size}, ");

            //TODO: change what is printed depending on setting
            result.AppendLine($"Reason {c.ReasonTotal}, Status {c.Status}");
            //result.AppendLine($"Strain {c.Strain}/{c.MaximumStrainTotal}");

            result.AppendLine(string.Join(", ", c.Skills.Select(s => s.ShortName)));
            result.AppendLine(string.Join(", ", c.Edges.Select(e => e.Name + ": " + e.Description)));
            result.AppendLine(string.Join(", ", c.Hindrances.Select(h => h.Name + " " + h.LevelName + ": " + h.Description)));
            result.AppendLine(string.Join(", ", c.Features.Select(h => h.Name)));
            result.AppendLine(string.Join(", ", c.Personality.Select(h => h.Name)));

            foreach (var group in c.PowerGroups)
                result.AppendLine($"{group.Skill}, Power Points {group.PowerPoints}, Powers: {string.Join(", ", group.Powers.Select(p => p.LongName))}");

            result.AppendLine(string.Join(", ", c.Gear.Select(h => h.Name + (string.IsNullOrEmpty(h.Description) ? "" : ": " + h.Description))));

            return result.ToString();
        }
    }
}
