using SavageTools.Settings;
using System;
using System.IO;
using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class CharacterOptions : ModelBase
    {
        public CharacterOptions(CharacterGenerator characterGenerator)
        {
            CharacterGenerator = characterGenerator ?? throw new ArgumentNullException(nameof(characterGenerator));
        }

        public bool BornAHero { get => GetDefault(false); set => Set(value); }
        public CharacterGenerator CharacterGenerator { get; }
        public bool RandomArchetype { get => GetDefault(false); set => Set(value); }
        public bool RandomRace { get => GetDefault(false); set => Set(value); }
        public bool RandomRank { get => GetDefault(false); set => Set(value); }
        public SettingArchetype SelectedArchetype
        {
            get { return Get<SettingArchetype>(); }
            set
            {
                if (Set(value) && value != null && !string.IsNullOrEmpty(value.Race))
                {
                    var newRace = CharacterGenerator.Races.SingleOrDefault(r => r.Name == value.Race);
                    if (newRace != null)
                        SelectedRace = newRace;
                }
            }
        }

        public SettingRace SelectedRace { get { return Get<SettingRace>(); } set { Set(value); } }
        public SettingRank SelectedRank { get { return Get<SettingRank>(); } set { Set(value); } }

        public bool WildCard { get => GetDefault(false); set => Set(value); }
        public Character GenerateCharacter()
        {
            return CharacterGenerator.GenerateCharacter(this);
        }

        public void LoadSetting(FileInfo file)
        {
            var currentArchetype = SelectedArchetype?.Name;
            var currentRace = SelectedRace?.Name;
            var currentRank = SelectedRank?.Name;

            CharacterGenerator.LoadSetting(file);


            if (currentArchetype != null && SelectedArchetype == null) //selected archetype was replaced so we need to reselect it
                SelectedArchetype = CharacterGenerator.Archetypes.Single(a => a.Name == currentArchetype);

            if (currentRace != null && SelectedRace == null) //selected race was replaced so we need to reselect it
                SelectedRace = CharacterGenerator.Races.Single(a => a.Name == currentRace);

            if (currentRank != null && SelectedRank == null) //selected rank was replaced so we need to reselect it
                SelectedRank = CharacterGenerator.Ranks.Single(a => a.Name == currentRank);

            if (CharacterGenerator.BornAHeroSetting)
                BornAHero = true;

        }
    }


}
