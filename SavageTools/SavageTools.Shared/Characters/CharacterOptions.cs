using SavageTools.Settings;
using System;
using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class CharacterOptions : ModelBase
    {
        public CharacterOptions(CharacterGenerator characterGenerator)
        {
            CharacterGenerator = characterGenerator ?? throw new ArgumentNullException(nameof(characterGenerator));
            BornAHero = characterGenerator.BornAHeroSetting;
            MoreSkills = characterGenerator.MoreSkillsSetting;
        }

        public bool BornAHero { get => GetDefault(false); set => Set(value); }
        public bool MoreSkills { get => GetDefault(false); set => Set(value); }
        public CharacterGenerator CharacterGenerator { get; }

        public int Count
        {
            get { return GetDefault(1); }
            set
            {
                if (value > 10)
                    Set(10);
                else if (value <= 0)
                    Set(1);
                else
                    Set(value);
            }
        }

        [CalculatedField("SelectedArchetype")]
        public bool RandomArchetype => SelectedArchetypeString == "";

        [CalculatedField("SelectedRace")]
        public bool RandomRace => SelectedRaceString == "";

        [CalculatedField("SelectedRank")]
        public bool RandomRank => SelectedRankString == "";

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

        [CalculatedField("SelectedArchetype")]
        public string SelectedArchetypeString
        {
            get => SelectedArchetype?.Name ?? "";
            set
            {
                if (string.IsNullOrEmpty(value))
                    SelectedArchetype = null;
                else
                    SelectedArchetype = CharacterGenerator.Archetypes.Single(a => a.Name == value);
            }
        }

        public SettingRace SelectedRace { get { return Get<SettingRace>(); } set { Set(value); } }

        [CalculatedField("SelectedRace")]
        public string SelectedRaceString
        {
            get => SelectedRace?.Name ?? "";
            set
            {
                if (string.IsNullOrEmpty(value))
                    SelectedRace = null;
                else
                    SelectedRace = CharacterGenerator.Races.Single(r => r.Name == value);
            }
        }

        public SettingRank SelectedRank { get { return Get<SettingRank>(); } set { Set(value); } }

        [CalculatedField("SelectedRank")]
        public string SelectedRankString
        {
            get => SelectedRank?.Name ?? "";
            set
            {
                if (string.IsNullOrEmpty(value))
                    SelectedRank = null;
                else
                    SelectedRank = CharacterGenerator.Ranks.Single(r => r.Name == value);
            }
        }

        public bool UseCoreSkills { get => GetDefault(true); set => Set(value); }
        public bool WildCard { get => GetDefault(false); set => Set(value); }

        public Character GenerateCharacter(Dice dice = null)
        {
            return CharacterGenerator.GenerateCharacter(this, dice);
        }

        /*
        public void LoadSetting(FileInfo file)
        {
            var currentArchetype = SelectedArchetype?.Name;
            var currentRace = SelectedRace?.Name;
            var currentRank = SelectedRank?.Name;

            CharacterGenerator.LoadSetting(file, true);

            if (currentArchetype != null && SelectedArchetype == null) //selected archetype was replaced so we need to reselect it
                SelectedArchetype = CharacterGenerator.Archetypes.Single(a => a.Name == currentArchetype);

            if (currentRace != null && SelectedRace == null) //selected race was replaced so we need to reselect it
                SelectedRace = CharacterGenerator.Races.Single(a => a.Name == currentRace);

            if (currentRank != null && SelectedRank == null) //selected rank was replaced so we need to reselect it
                SelectedRank = CharacterGenerator.Ranks.Single(a => a.Name == currentRank);

            if (CharacterGenerator.BornAHeroSetting)
                BornAHero = true;
        }
        */
    }
}
