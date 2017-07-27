using SavageTools.Characters;
using SavageTools.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Tortuga.Anchor.Collections;
using Tortuga.Anchor.Modeling;

namespace SavageTools
{


    public class CharacterGenerator : ModelBase
    {
        public ObservableCollectionExtended<string> Settings { get; } = new ObservableCollectionExtended<string>();

        public ObservableCollectionExtended<SettingSkillOption> Skills { get; } = new ObservableCollectionExtended<SettingSkillOption>();

        public ObservableCollectionExtended<SettingHindrance> Hindrances { get; } = new ObservableCollectionExtended<SettingHindrance>();

        public ObservableCollectionExtended<SettingEdge> Edges { get; } = new ObservableCollectionExtended<SettingEdge>();

        public ObservableCollectionExtended<SettingArchetype> Archetypes { get; } = new ObservableCollectionExtended<SettingArchetype>();
        public ObservableCollectionExtended<SettingRace> Races { get; } = new ObservableCollectionExtended<SettingRace>();
        public ObservableCollectionExtended<SettingRank> Ranks { get; } = new ObservableCollectionExtended<SettingRank>();
        public ObservableCollectionExtended<SettingTrapping> Trappings { get; } = new ObservableCollectionExtended<SettingTrapping>();
        public ObservableCollectionExtended<SettingPower> Powers { get; } = new ObservableCollectionExtended<SettingPower>();


        public bool RandomArchetype { get => GetDefault(false); set => Set(value); }
        public bool RandomRace { get => GetDefault(false); set => Set(value); }
        public bool RandomRank { get => GetDefault(false); set => Set(value); }

        public bool BornAHero { get => GetDefault(false); set => Set(value); }

        SettingEdge FindEdge(SettingEdge prototype)
        {
            //If any of these are set, assume its a custom edge
            if (!string.IsNullOrEmpty(prototype.Description))
                return prototype;
            if (prototype.Features?.Length > 0)
                return prototype;
            if (prototype.Traits?.Length > 0)
                return prototype;
            if (prototype.Skills?.Length > 0)
                return prototype;

            //Look for a shared edge
            var result = Edges.FirstOrDefault(e => e.Name == prototype.Name);
            return result ?? prototype;
        }

        SettingHindrance FindHindrance(SettingHindrance prototype)
        {
            //If any of these are set, assume its a custom edge
            if (!string.IsNullOrEmpty(prototype.Description))
                return prototype;
            if (prototype.Features?.Length > 0)
                return prototype;

            //Look for a shared hindrance
            var result = Hindrances.FirstOrDefault(e => e.Name == prototype.Name);
            return result ?? prototype;
        }

        public SettingArchetype SelectedArchetype
        {
            get { return Get<SettingArchetype>(); }
            set
            {
                if (Set(value) && value != null && !string.IsNullOrEmpty(value.Race))
                {
                    var newRace = Races.SingleOrDefault(r => r.Name == value.Race);
                    if (newRace != null)
                        SelectedRace = newRace;
                }
            }
        }

        public SettingRace SelectedRace { get { return Get<SettingRace>(); } set { Set(value); } }

        public SettingRank SelectedRank { get { return Get<SettingRank>(); } set { Set(value); } }


        public bool WildCard { get { return GetDefault(false); } set { Set(value); } }


        public async Task<Character> GenerateCharacterAsync()
        {
            var dice = new Dice();

            if (RandomArchetype)
            {
                var list = Archetypes.Where(a => WildCard || !a.WildCard).ToList(); //Don't pick wildcard archetypes unless WildCard is checked.
                SelectedArchetype = dice.Choose(list);
            }
            if (RandomRace && string.IsNullOrEmpty(SelectedArchetype.Race))
            {
                var list = Races.Where(r => r.Name != "(Special)").ToList();
                SelectedRace = dice.Choose(list);
            }
            if (RandomRank)
            {
                var table = new Table<SettingRank>();
                foreach (var item in Ranks)
                    table.Add(item, 100 - item.Experience); //this should weigh it towards novice

                SelectedRank = table.RandomChoose(dice);
            }


            var result = new Character() { Rank = SelectedRank.Name, IsWildCard = WildCard };

            var name = await Names.NameService.CreateRandomPersonAsync();
            result.Name = name.FullName;
            result.Gender = name.Gender;

            //Add all possible skills (except ones created by edges)
            foreach (var item in Skills)
                result.Skills.Add(new Skill(item.Name, item.Attribute) { Trait = 0 });



            ApplyArchetype(result, dice);

            ApplyRace(result, dice);

            //Add the rank
            result.UnusedAdvances = SelectedRank.UnusedAdvances;
            result.Experience = SelectedRank.Experience;


            if (result.UnusedHindrances == 0)//Add some hindrances to make it interesting
            {
                var extraHindrances = Math.Max(dice.D(6) - 2, 0);
                result.UnusedHindrances += extraHindrances;

                //Assign the points gained from the extra hindrances
                while (extraHindrances > 0)
                {
                    if (extraHindrances >= 2 && dice.NextBoolean())
                    {
                        extraHindrances -= 2;
                        if (dice.NextBoolean())
                            result.UnusedAttributes += 1;
                        else
                            result.UnusedEdges += 1;
                    }
                    else
                    {
                        extraHindrances -= 1;
                        result.UnusedSkills += 1;
                    }
                }
            }

            //Main loop for random picks
            while (result.UnusedAttributes > 0 || result.UnusedSkills > 0 || result.UnusedSmartSkills > 0 || result.UnusedAdvances > 0 || result.UnusedEdges > 0 || result.UnusedHindrances > 0 || result.UnusedRacialEdges > 0 || result.UnusedIconicEdges > 0)
            {
                //tight loop to apply current hindrances
                while (result.UnusedHindrances > 0)
                    PickHindrance(result, dice);

                //Pick up racial and iconic edges early because they can grant skills
                if (result.UnusedRacialEdges > 0)
                    PickRacialEdge(result, dice);

                if (result.UnusedIconicEdges > 0)
                    PickIconicEdge(result, dice);

                if (result.UnusedAttributes > 0)
                    PickAttribute(result, dice);

                if (result.UnusedSmartSkills > 0)
                    PickSmartsSkill(result, dice);

                if (result.UnusedSkills > 0)
                    PickSkill(result, dice);

                if (result.UnusedEdges > 0)
                    PickEdge(result, dice);



                //only apply an advance when everything else has been used
                if (result.UnusedAdvances > 0 && result.UnusedAttributes <= 0 && result.UnusedSkills <= 0 && result.UnusedEdges <= 0 && result.UnusedSmartSkills <= 0)
                {
                    PickAdvancement(result, dice);
                }

                //Out of advancements. Do we need a hindrance?
                if (result.UnusedAdvances == 0)
                {
                    if (result.UnusedAttributes < 0)
                    {
                        result.UnusedHindrances += -2 * result.UnusedAttributes;
                        result.UnusedAttributes = 0;
                    }
                    if (result.UnusedSmartSkills < 0)
                    {
                        result.UnusedSkills += result.UnusedSmartSkills;
                        result.UnusedSmartSkills = 0;
                    }
                    if (result.UnusedSkills < 0)
                    {
                        result.UnusedHindrances += -1 * result.UnusedSkills;
                        result.UnusedSkills = 0;
                    }
                    if (result.UnusedEdges < 0)
                    {
                        result.UnusedHindrances += -2 * result.UnusedEdges;
                        result.UnusedEdges = 0;
                    }
                }

            }

            //pick powers
            foreach (var group in result.PowerGroups)
                while (group.UnusedPowers > 0)
                    PickPower(result, group, dice);

            //Remove the skills that were not chosen
            foreach (var item in result.Skills.Where(s => s.Trait == 0).ToList())
                result.Skills.Remove(item);

            return result;
        }

        void ApplyArchetype(Character result, Dice dice)
        {
            //Add the archetype
            result.Archetype = SelectedArchetype.Name;
            result.UnusedAttributes = SelectedArchetype.UnusedAttributes;
            result.UnusedSkills = SelectedArchetype.UnusedSkills;
            result.Agility = SelectedArchetype.Agility;
            result.Smarts = SelectedArchetype.Smarts;
            result.Spirit = SelectedArchetype.Spirit;
            result.Strength = SelectedArchetype.Strength;
            result.Vigor = SelectedArchetype.Vigor;


            //then edges first because they can create new skills
            if (SelectedArchetype.Edges != null)
                foreach (var item in SelectedArchetype.Edges)
                    ApplyEdge(result, FindEdge(item), dice);

            if (SelectedArchetype.Hindrances != null)
                foreach (var item in SelectedArchetype.Hindrances)
                {
                    //pick up the level from the archetype
                    var level = 0;
                    if (item.Type == "Minor" || item.Type == "Minor/Major")
                        level = 1;
                    else if (item.Type == "Major")
                        level = 2;

                    var hindrance = FindHindrance(item);

                    if (level == 0) //check for a default
                    {
                        if (hindrance.Type == "Minor" || hindrance.Type == "Minor/Major")
                            level = 1;
                        else if (hindrance.Type == "Major")
                            level = 2;
                    }
                    ApplyHindrance(result, hindrance, level, dice);
                }

            if (SelectedArchetype.Skills != null)
                foreach (var item in SelectedArchetype.Skills)
                {
                    var skill = result.Skills[item.Name];
                    if (skill != null)
                        skill.Trait = Math.Max(skill.Trait.Score, item.Level);
                    else
                        result.Skills.Add(new Skill(item.Name, item.Attribute) { Trait = item.Level });
                }
            if (SelectedArchetype.Traits != null)
                foreach (var item in SelectedArchetype.Traits)
                    result.Increment(item.Name, item.Bonus, dice);

        }
        void ApplyRace(Character result, Dice dice)
        {
            //Add the race
            result.Race = SelectedRace.Name;

            if (SelectedRace.Edges != null)
                foreach (var item in SelectedRace.Edges)
                    ApplyEdge(result, FindEdge(item), dice);

            if (SelectedRace.Hindrances != null)
                foreach (var item in SelectedRace.Hindrances)
                {
                    //pick up the level from the archetype
                    var level = 0;
                    if (item.Type == "Minor" || item.Type == "Minor/Major")
                        level = 1;
                    else if (item.Type == "Major")
                        level = 2;

                    var hindrance = FindHindrance(item);

                    if (level == 0) //check for a default
                    {
                        if (hindrance.Type == "Minor" || hindrance.Type == "Minor/Major")
                            level = 1;
                        else if (hindrance.Type == "Major")
                            level = 2;
                    }
                    ApplyHindrance(result, hindrance, level, dice);
                }

            if (SelectedRace.Skills != null)
                foreach (var item in SelectedRace.Skills)
                {
                    var skill = result.Skills[item.Name];
                    if (skill != null)
                        skill.Trait = Math.Max(skill.Trait.Score, item.Level);
                    else
                        result.Skills.Add(new Skill(item.Name, item.Attribute) { Trait = item.Level });
                }
            if (SelectedRace.Traits != null)
                foreach (var item in SelectedRace.Traits)
                    result.Increment(item.Name, item.Bonus, dice);

            if (SelectedRace.Powers != null)
                foreach (var item in SelectedRace.Powers)
                    AddPower(result, item, dice);


        }
        void ApplyHindrance(Character result, SettingHindrance hindrance, int level, Dice dice)
        {
            result.Hindrances.Add(new Hindrance() { Name = hindrance.Name, Description = hindrance.Description, Level = level });

            if (hindrance.Trait != null)
                foreach (var item in hindrance.Trait)
                    result.Increment(item.Name, item.Bonus, dice);

            if (hindrance.Features != null)
                foreach (var item in hindrance.Features)
                    result.Features.Add(item.Name);

            if (hindrance.Skill != null)
                foreach (var item in hindrance.Skill)
                {
                    var skill = result.Skills.FirstOrDefault(s => s.Name == item.Name);

                    if (skill == null)
                        result.Skills.Add(new Skill(item.Name, item.Attribute) { Trait = item.Level });
                    else if (skill.Trait < item.Level)
                        skill.Trait = item.Level;
                }
        }

        static void ApplyEdge(Character result, SettingEdge edge, Dice dice)
        {
            result.Edges.Add(new Edge() { Name = edge.Name, Description = edge.Description, UniqueGroup = edge.UniqueGroup });

            if (edge.Traits != null)
                foreach (var item in edge.Traits)
                    result.Increment(item.Name, item.Bonus, dice);

            if (edge.Features != null)
                foreach (var item in edge.Features)
                    result.Features.Add(item.Name);

            if (edge.AvailablePowers != null)
                foreach (var item in edge.AvailablePowers)
                    result.PowerGroups[item.Skill].AvailablePowers.Add(item.Name);

            if (edge.Skills != null)
                foreach (var item in edge.Skills)
                {
                    var skill = result.Skills.FirstOrDefault(s => s.Name == item.Name);
                    if (skill == null)
                    {
                        result.Skills.Add(new Skill(item.Name, item.Attribute) { Trait = item.Level });
                    }
                    else if (skill.Trait < item.Level)
                    {
                        skill.Trait = item.Level;
                    }
                }
        }

        static readonly XmlSerializer SettingXmlSerializer = new XmlSerializer(typeof(Setting));

        public void LoadSetting(FileInfo file)
        {
            var currentArchetype = SelectedArchetype?.Name;
            var currentRace = SelectedRace?.Name;
            var currentRank = SelectedRank?.Name;

            Setting book;
            // Open document
            using (var stream = file.OpenRead())
                book = (Setting)SettingXmlSerializer.Deserialize(stream);

            if (Settings.Any(s => s == book.Name))
                return; //already loaded

            Settings.Add(book.Name);
            if (book.BornAHero)
                BornAHero = true;

            if (book.References != null)
                foreach (var item in book.References.Where(r => !Settings.Any(s => s == r.Name)))
                {
                    var referencedBook = new FileInfo(Path.Combine(file.DirectoryName, item.Name + ".savage-setting"));
                    if (referencedBook.Exists)
                        LoadSetting(referencedBook);
                }

            //Adding a book overwrites the previous book.
            if (book.Skills != null)
                foreach (var item in book.Skills)
                {
                    Skills.RemoveAll(s => s.Name == item.Name);
                    Skills.Add(item);
                }
            if (book.Edges != null)
                foreach (var item in book.Edges)
                {
                    Edges.RemoveAll(s => s.Name == item.Name);
                    Edges.Add(item);
                }
            if (book.Hindrances != null)
                foreach (var item in book.Hindrances)
                {
                    Hindrances.RemoveAll(s => s.Name == item.Name);
                    Hindrances.Add(item);
                }
            if (book.Archetypes != null)
            {
                foreach (var item in book.Archetypes)
                {
                    Archetypes.RemoveAll(s => s.Name == item.Name);
                    Archetypes.Add(item);
                }
                Archetypes.Sort(a => a.Name);
            }
            if (book.Races != null)
            {
                foreach (var item in book.Races)
                {
                    Races.RemoveAll(s => s.Name == item.Name);
                    Races.Add(item);
                }
                Races.Sort(r => r.Name);
            }
            if (book.Ranks != null)
                foreach (var item in book.Ranks)
                {
                    Ranks.RemoveAll(s => s.Name == item.Name);
                    Ranks.Add(item);
                }

            if (book.Trappings != null)
                foreach (var item in book.Trappings)
                {
                    Trappings.RemoveAll(s => s.Name == item.Name);
                    Trappings.Add(item);
                }

            if (book.Powers != null)
                foreach (var item in book.Powers)
                {
                    Powers.RemoveAll(s => s.Name == item.Name);
                    Powers.Add(item);
                }

            if (currentArchetype != null && SelectedArchetype == null) //selected archetype was replaced so we need to reselect it
                SelectedArchetype = Archetypes.Single(a => a.Name == currentArchetype);

            if (currentRace != null && SelectedRace == null) //selected race was replaced so we need to reselect it
                SelectedRace = Races.Single(a => a.Name == currentRace);

            if (currentRank != null && SelectedRank == null) //selected rank was replaced so we need to reselect it
                SelectedRank = Ranks.Single(a => a.Name == currentRank);
        }
        void PickHindrance(Character result, Dice dice)
        {
            var minors = result.Hindrances.Where(h => h.Level == 1).Count();
            var majors = result.Hindrances.Where(h => h.Level == 2).Count();

            bool useMajor;

            if (result.UnusedHindrances < 2)
                useMajor = false; //
            else if (result.UnusedHindrances == 2)
            {
                if (majors == 0 && minors == 0)
                    useMajor = dice.NextBoolean(); //empty slate, can go either way
                else if (majors == 0)
                    useMajor = true; //we already have 1+ minors so use a major
                else
                    useMajor = false;
            }
            else
            {
                if (majors == 0)
                    useMajor = true; //pick a major if we don't have one already
                else if (minors == 0)
                    useMajor = false; //pick a minor since we don't have any yet
                else
                    useMajor = dice.NextBoolean(); //this shouldn't happen with normal characters
            }

            var table = new Table<SettingHindrance>();
            foreach (var item in Hindrances)
            {
                //TODO: Some hindrances allow duplicates
                if (result.Hindrances.Any(e => e.Name == item.Name))
                    continue; //no dups

                if ((item.Type == "Minor" && !useMajor)
                || (item.Type == "Major" && useMajor)
                || (item.Type == "Minor/Major"))
                    table.Add(item, 1);
            }

            var hindrance = table.RandomChoose(dice);
            ApplyHindrance(result, hindrance, useMajor ? 2 : 1, dice);

            result.UnusedHindrances -= useMajor ? 2 : 1;
        }
        static void PickSkill(Character result, Dice dice)
        {
            bool allowHigh = result.UnusedSkills >= 2 && result.UnusedAttributes == 0; //don't buy expensive skills until all of the attributes are picked

            var table = new Table<Skill>();
            foreach (var item in result.Skills)
            {

                if (item.Trait == 0)
                    table.Add(item, result.GetAttribute(item.Attribute).Score - 3); //favor skills that match your attributes
                else if ((item.Trait + 1) < result.GetAttribute(item.Attribute)) //favor improving what you know
                    table.Add(item, result.GetAttribute(item.Attribute).Score - 3 + item.Trait.Score);
                else if (allowHigh && item.Trait < 12)
                    table.Add(item, item.Trait.Score); //Raising skills above the controlling attribute is relatively rare
            }
            var skill = table.RandomChoose(dice);
            if (skill.Trait == 0)
            {
                result.UnusedSkills -= 1;
                skill.Trait = 4;
            }
            else if (skill.Trait < result.GetAttribute(skill.Attribute))
            {
                result.UnusedSkills -= 1;
                skill.Trait += 1;
            }
            else
            {
                result.UnusedSkills -= 2;
                skill.Trait += 1;
            }
        }

        static void PickSmartsSkill(Character result, Dice dice)
        {
            bool allowHigh = result.UnusedSmartSkills >= 2 && result.UnusedAttributes == 0; //don't buy expensive skills until all of the attributes are picked

            var table = new Table<Skill>();
            foreach (var item in result.Skills.Where(s => s.Attribute == "Smarts"))
            {

                if (item.Trait == 0)
                    table.Add(item, result.GetAttribute(item.Attribute).Score - 3); //favor skills that match your attributes
                else if ((item.Trait + 1) < result.GetAttribute(item.Attribute)) //favor improving what you know
                    table.Add(item, result.GetAttribute(item.Attribute).Score - 3 + item.Trait.Score);
                else if (allowHigh && item.Trait < 12)
                    table.Add(item, item.Trait.Score); //Raising skills above the controlling attribute is relatively rare
            }
            var skill = table.RandomChoose(dice);
            if (skill.Trait == 0)
            {
                result.UnusedSmartSkills -= 1;
                skill.Trait = 4;
            }
            else if (skill.Trait < result.GetAttribute(skill.Attribute))
            {
                result.UnusedSmartSkills -= 1;
                skill.Trait += 1;
            }
            else
            {
                result.UnusedSmartSkills -= 2;
                skill.Trait += 1;
            }
        }

        static void PickAttribute(Character result, Dice dice)
        {
            //Attributes are likely to stack rather than spread evenly
            var table = new Table<string>();
            if (result.Vigor < result.MaxVigor)
                table.Add("Vigor", result.Vigor.Score);
            if (result.Smarts < result.MaxSmarts)
                table.Add("Smarts", result.Smarts.Score);
            if (result.Agility < result.MaxAgility)
                table.Add("Agility", result.Agility.Score);
            if (result.Strength < result.MaxStrength)
                table.Add("Strength", result.Strength.Score);
            if (result.Spirit < result.MaxSpirit)
                table.Add("Spirit", result.Spirit.Score);

            result.Increment(table.RandomChoose(dice), dice);

            result.UnusedAttributes -= 1;
        }

        void PickPower(Character result, PowerGroup group, Dice dice)
        {

            var powers = new List<SettingPower>();
            var trappings = new List<SettingTrapping>();

            foreach (var item in Powers)
            {
                if (group.AvailablePowers.Count > 0 && !group.AvailablePowers.Contains(item.Name))
                    continue;

                var requirements = item.Requires.Split(',').Select(e => e.Trim());
                if (requirements.All(c => result.HasFeature(c, BornAHero)))
                    powers.Add(item);
            }

            foreach (var item in Trappings)
            {
                if (group.AvailableTrappings.Count > 0 && !group.AvailableTrappings.Contains(item.Name))
                    continue;

                if (group.ProhibitedTrappings.Contains(item.Name))
                    continue;

                trappings.Add(item);
            }

            if (powers.Count == 0 || trappings.Count == 0)
            {
                result.Features.Add($"Has {group.UnusedPowers} unused powers for {group.Skill}.");
                group.UnusedPowers = 0;
                return;
            }

            TryAgain:
            var trapping = dice.Choose(Trappings);
            var power = dice.Choose(powers);

            if (result.PowerGroups.ContainsPower(power.Name, trapping.Name))
                goto TryAgain;

            group.Powers.Add(new Power(power.Name, trapping.Name));

            group.UnusedPowers -= 1;
        }

        void AddPower(Character result, SettingPower power, Dice dice)
        {
            var trappings = new List<SettingTrapping>();
            var group = result.PowerGroups[power.Skill];
            
            foreach (var item in Trappings)
            {
                if (group.AvailableTrappings.Count > 0 && !group.AvailableTrappings.Contains(item.Name))
                    continue;

                if (group.ProhibitedTrappings.Contains(item.Name))
                    continue;

                trappings.Add(item);
            }

            if (trappings.Count == 0)
                trappings.Add(new SettingTrapping() { Name = "None" });

            var trapping = dice.Choose(Trappings);

            group.Powers.Add(new Power(power.Name, trapping.Name));
        }


        void PickEdge(Character result, Dice dice)
        {
            var table = new Table<SettingEdge>();
            var edgeList = (IEnumerable<SettingEdge>)Edges;

            if (SelectedArchetype.IconicEdges != null)
                edgeList = edgeList.Concat(SelectedArchetype.IconicEdges);
            if (SelectedRace.RacialEdges != null)
                edgeList = edgeList.Concat(SelectedRace.RacialEdges);

            foreach (var item in edgeList)
            {
                //TODO: Some edges allow duplicates
                if (result.Edges.Any(e => e.Name == item.Name))
                    continue; //no dups

                if (!string.IsNullOrEmpty(item.UniqueGroup))
                    if (result.Edges.Any(e => e.UniqueGroup == item.UniqueGroup))
                        continue; //can't have multiple from a unique group (i.e. arcane background)

                var requirements = item.Requires.Split(',').Select(e => e.Trim());
                if (requirements.All(c => result.HasFeature(c, BornAHero)))
                    table.Add(item, 1);
            }
            if (table.Count == 0)
            {
                Debug.WriteLine("No edges were available");
            }
            else
            {
                Debug.WriteLine($"Found {table.Count} edges");
                var edge = table.RandomChoose(dice);
                ApplyEdge(result, edge, dice);

                result.UnusedEdges -= 1;
            }
        }

        void PickRacialEdge(Character result, Dice dice)
        {
            var table = new Table<SettingEdge>();

            foreach (var item in SelectedRace.RacialEdges)
            {
                //TODO: Some edges allow duplicates
                if (result.Edges.Any(e => e.Name == item.Name))
                    continue; //no dups

                if (!string.IsNullOrEmpty(item.UniqueGroup))
                    if (result.Edges.Any(e => e.UniqueGroup == item.UniqueGroup))
                        continue; //can't have multiple from a unique group (i.e. arcane background)

                var requirements = item.Requires.Split(',').Select(e => e.Trim());
                if (requirements.All(c => result.HasFeature(c, BornAHero)))
                    table.Add(item, 1);
            }
            if (table.Count == 0)
            {
                Debug.WriteLine("No edges were available");
            }
            else
            {
                Debug.WriteLine($"Found {table.Count} edges");
                var edge = table.RandomChoose(dice);
                ApplyEdge(result, edge, dice);

                result.UnusedRacialEdges -= 1;
            }
        }

        void PickIconicEdge(Character result, Dice dice)
        {
            var table = new Table<SettingEdge>();

            foreach (var item in SelectedArchetype.IconicEdges)
            {
                //TODO: Some edges allow duplicates
                if (result.Edges.Any(e => e.Name == item.Name))
                    continue; //no dups

                if (!string.IsNullOrEmpty(item.UniqueGroup))
                    if (result.Edges.Any(e => e.UniqueGroup == item.UniqueGroup))
                        continue; //can't have multiple from a unique group (i.e. arcane background)

                var requirements = item.Requires.Split(',').Select(e => e.Trim());
                if (requirements.All(c => result.HasFeature(c, BornAHero)))
                    table.Add(item, 1);
            }
            if (table.Count == 0)
            {
                Debug.WriteLine("No edges were available");
            }
            else
            {
                Debug.WriteLine($"Found {table.Count} edges");
                var edge = table.RandomChoose(dice);
                ApplyEdge(result, edge, dice);

                result.UnusedIconicEdges -= 1;
            }
        }

        static void PickAdvancement(Character result, Dice dice)
        {
            result.UnusedAdvances -= 1;

            if (result.UnusedEdges < 0)
                result.UnusedEdges += 1;
            else if (result.UnusedSkills < 0)
                result.UnusedSkills += 2; //pay back the skill point loan
            else if (result.UnusedSmartSkills < 0)
                result.UnusedSmartSkills += 2; //pay back the skill point loan
            else

            {
                switch (dice.Next(5))
                {
                    case 0:
                        result.UnusedEdges += 1;
                        break;
                    case 1: //increase a high skill

                        {
                            var table = new Table<Skill>();
                            foreach (var skill in result.Skills)
                            {
                                if (skill.Trait >= result.GetAttribute(skill.Attribute) && skill.Trait < 12)
                                    table.Add(skill, skill.Trait.Score);
                            }
                            if (table.Count == 0)
                                goto case 2;
                            table.RandomChoose(dice).Trait += 1;
                            break;
                        }
                    case 2: //increase a low skill

                        {
                            var table = new Table<Skill>();
                            foreach (var skill in result.Skills)
                            {
                                if (skill.Trait < result.GetAttribute(skill.Attribute) && skill.Trait < 12)
                                    table.Add(skill, skill.Trait.Score);
                            }
                            if (table.Count >= 2)
                                goto case 3;
                            table.RandomPick(dice).Trait += 1;
                            table.RandomPick(dice).Trait += 1; //use Pick so we get 2 different skills
                            break;
                        }
                    case 3: //add a new skill

                        {
                            var table = new Table<Skill>();
                            foreach (var skill in result.Skills)
                            {
                                if (skill.Trait == 0)
                                    table.Add(skill, result.GetAttribute(skill.Attribute).Score);
                            }
                            if (table.Count == 0)
                                break; //really?
                            table.RandomChoose(dice).Trait = 4;
                            break;
                        }
                    case 4:
                        result.UnusedAttributes += 1;
                        break;
                }
            }
        }
    }
}


