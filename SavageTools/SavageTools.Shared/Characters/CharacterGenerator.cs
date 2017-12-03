using SavageTools.Names;
using SavageTools.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Tortuga.Anchor.Collections;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class CharacterGenerator : ModelBase
    {
        public LocalNameService NameService;

        public PersonalityService PersonalityService;

        static readonly XmlSerializer SettingXmlSerializer = new XmlSerializer(typeof(Setting));

        public CharacterGenerator(FileInfo setting)
        {
            LoadSetting(setting);

            NameService = new LocalNameService(setting.DirectoryName, NamePrefix);
            PersonalityService = new PersonalityService(setting.DirectoryName);
        }

        public ObservableCollectionExtended<SettingArchetype> Archetypes => GetNew<ObservableCollectionExtended<SettingArchetype>>();
        public bool BornAHeroSetting { get => GetDefault(false); private set => Set(value); }
        public ObservableCollectionExtended<SettingEdge> Edges => GetNew<ObservableCollectionExtended<SettingEdge>>();
        public ObservableCollectionExtended<SettingHindrance> Hindrances => GetNew<ObservableCollectionExtended<SettingHindrance>>();
        public string NamePrefix { get => Get<string>(); set => Set(value); }
        public ObservableCollectionExtended<SettingPower> Powers => GetNew<ObservableCollectionExtended<SettingPower>>();
        public ObservableCollectionExtended<SettingRace> Races => GetNew<ObservableCollectionExtended<SettingRace>>();

        public ObservableCollectionExtended<SettingRank> Ranks => GetNew<ObservableCollectionExtended<SettingRank>>();


        public ObservableCollectionExtended<string> Settings => GetNew<ObservableCollectionExtended<string>>();
        public ObservableCollectionExtended<SettingSkillOption> Skills => GetNew<ObservableCollectionExtended<SettingSkillOption>>();
        public ObservableCollectionExtended<SettingTrapping> Trappings => GetNew<ObservableCollectionExtended<SettingTrapping>>();
        public bool UseReason { get => Get<bool>(); set => Set(value); }
        public bool UseStatus { get => Get<bool>(); set => Set(value); }
        public bool UseStrain { get => Get<bool>(); set => Set(value); }
        public void AddPower(Character result, string arcaneSkill, string power, Dice dice)
        {
            var trappings = new List<SettingTrapping>();
            var group = result.PowerGroups[arcaneSkill];

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

            group.Powers.Add(new Power(power, trapping.Name));
        }

        public void ApplyEdge(Character result, Dice dice, string edgeName, string description = null)
        {
            var edge = Edges.SingleOrDefault(x => string.Equals(x.Name, edgeName, StringComparison.OrdinalIgnoreCase));
            if (edge == null)
                result.Edges.Add(edgeName, description);
            else
                ApplyEdge(result, edge, dice);
        }

        public Character GenerateCharacter(CharacterOptions options, Dice dice = null)
        {
            dice = dice ?? new Dice();

            if (options.RandomArchetype)
            {
                var list = Archetypes.Where(a => options.WildCard || !a.WildCard).ToList(); //Don't pick wildcard archetypes unless WildCard is checked.
                options.SelectedArchetype = dice.Choose(list);
            }
            if (options.RandomRace && string.IsNullOrEmpty(options.SelectedArchetype.Race))
            {
                var list = Races.Where(r => r.Name != "(Special)").ToList();
                options.SelectedRace = dice.Choose(list);
            }
            if (options.RandomRank)
            {
                var table = new Table<SettingRank>();
                foreach (var item in Ranks)
                    table.Add(item, 100 - item.Experience); //this should weigh it towards novice

                options.SelectedRank = table.RandomChoose(dice);
            }


            var result = new Character() { Rank = options.SelectedRank.Name, IsWildCard = options.WildCard, UseReason = UseReason, UseStatus = UseStatus, UseStrain = UseStrain };

            var name = NameService.CreateRandomPerson(dice);
            result.Name = name.FullName;
            result.Gender = name.Gender;

            //Add all possible skills (except ones created by edges)
            foreach (var item in Skills)
                result.Skills.Add(new Skill(item.Name, item.Attribute) { Trait = 0 });



            ApplyArchetype(result, dice, options.SelectedArchetype);

            ApplyRace(result, dice, options.SelectedRace);

            //Add the rank
            result.UnusedAdvances = options.SelectedRank.UnusedAdvances;
            result.Experience = options.SelectedRank.Experience;


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
                    PickRacialEdge(result, dice, options.SelectedRace, options.BornAHero);

                if (result.UnusedIconicEdges > 0)
                    PickIconicEdge(result, dice, options.SelectedArchetype, options.BornAHero);

                if (result.UnusedAttributes > 0)
                    PickAttribute(result, dice);

                if (result.UnusedSmartSkills > 0)
                    PickSmartsSkill(result, dice);

                if (result.UnusedSkills > 0)
                    PickSkill(result, dice);

                if (result.UnusedEdges > 0)
                    PickEdge(result, dice, options.SelectedArchetype, options.SelectedRace, options.BornAHero);



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
                    PickPower(result, group, dice, options.BornAHero);

            //Remove the skills that were not chosen
            foreach (var item in result.Skills.Where(s => s.Trait == 0).ToList())
                result.Skills.Remove(item);

            //Add personality
            int personalityTraits = dice.D(3);
            for (var i = 0; i < personalityTraits; i++)
                result.Personality.Add(PersonalityService.CreateRandomPersonality(dice));

            return result;
        }

        public IEnumerable<SettingSkillOption> KnowledgeSkills()
        {
            return Skills.Where(x => x.Name.StartsWith("Knowledge "));
        }
        public void LoadSetting(FileInfo file)
        {
            Setting book;
            // Open document
            using (var stream = file.OpenRead())
                book = (Setting)SettingXmlSerializer.Deserialize(stream);

            if (Settings.Any(s => s == book.Name))
                return; //already loaded

            Settings.Add(book.Name);

            if (book.BornAHero)
                BornAHeroSetting = true;

            if (book.UseReason)
                UseReason = true;

            if (book.UseStatus)
                UseStatus = true;

            if (book.UseStrain)
                UseStrain = true;

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

            NamePrefix = book.NamePrefix;
        }
        public void PickEdge(Character result, Dice dice, bool bornAHero)
        {
            PickEdge(result, dice, null, null, bornAHero);
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

        void ApplyArchetype(Character result, Dice dice, SettingArchetype archetype)
        {
            //Add the archetype
            result.Archetype = archetype.Name;
            result.UnusedAttributes = archetype.UnusedAttributes;
            result.UnusedSkills = archetype.UnusedSkills;
            result.Agility = archetype.Agility;
            result.Smarts = archetype.Smarts;
            result.Spirit = archetype.Spirit;
            result.Strength = archetype.Strength;
            result.Vigor = archetype.Vigor;


            //then edges first because they can create new skills
            if (archetype.Edges != null)
                foreach (var item in archetype.Edges)
                    ApplyEdge(result, FindEdge(item), dice);

            if (archetype.Hindrances != null)
                foreach (var item in archetype.Hindrances)
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

            if (archetype.Skills != null)
                foreach (var item in archetype.Skills)
                {
                    var skill = result.Skills[item.Name];
                    if (skill != null)
                        skill.Trait = Math.Max(skill.Trait.Score, item.Level);
                    else
                        result.Skills.Add(new Skill(item.Name, item.Attribute) { Trait = item.Level });
                }
            if (archetype.Traits != null)
                foreach (var item in archetype.Traits)
                    result.Increment(item.Name, item.Bonus, dice);

            if (archetype.Features != null)
                foreach (var item in archetype.Features)
                    result.Features.Add(item.Name);

            if (archetype.Gear != null)
                foreach (var item in archetype.Gear)
                {
                    result.Gear.Add(item.Name, item.Description);

                    if (item.Traits != null)
                        foreach (var trait in item.Traits)
                            result.Increment(trait.Name, trait.Bonus, dice);
                }

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

        void ApplyRace(Character result, Dice dice, SettingRace race)
        {
            //Add the race
            result.Race = race.Name;

            if (race.Edges != null)
                foreach (var item in race.Edges)
                    ApplyEdge(result, FindEdge(item), dice);

            if (race.Hindrances != null)
                foreach (var item in race.Hindrances)
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

            if (race.Skills != null)
                foreach (var item in race.Skills)
                {
                    var skill = result.Skills[item.Name];
                    if (skill != null)
                        skill.Trait = Math.Max(skill.Trait.Score, item.Level);
                    else
                        result.Skills.Add(new Skill(item.Name, item.Attribute) { Trait = item.Level });
                }
            if (race.Traits != null)
                foreach (var item in race.Traits)
                    result.Increment(item.Name, item.Bonus, dice);

            if (race.Powers != null)
                foreach (var item in race.Powers)
                    AddPower(result, item, dice);


        }

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
        void PickEdge(Character result, Dice dice, SettingArchetype archetype, SettingRace race, bool bornAHero)
        {
            var table = new Table<SettingEdge>();
            var edgeList = (IEnumerable<SettingEdge>)Edges;

            if (archetype?.IconicEdges != null)
                edgeList = edgeList.Concat(archetype.IconicEdges);
            if (race?.RacialEdges != null)
                edgeList = edgeList.Concat(race.RacialEdges);

            foreach (var item in edgeList)
            {
                //TODO: Some edges allow duplicates
                if (result.Edges.Any(e => e.Name == item.Name))
                    continue; //no dups

                if (!string.IsNullOrEmpty(item.UniqueGroup))
                    if (result.Edges.Any(e => e.UniqueGroup == item.UniqueGroup))
                        continue; //can't have multiple from a unique group (i.e. arcane background)

                var requirements = item.Requires.Split(',').Select(e => e.Trim());
                if (requirements.All(c => result.HasFeature(c, bornAHero)))
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
        void PickIconicEdge(Character result, Dice dice, SettingArchetype archetype, bool bornAHero)
        {
            var table = new Table<SettingEdge>();

            foreach (var item in archetype.IconicEdges)
            {
                //TODO: Some edges allow duplicates
                if (result.Edges.Any(e => e.Name == item.Name))
                    continue; //no dups

                if (!string.IsNullOrEmpty(item.UniqueGroup))
                    if (result.Edges.Any(e => e.UniqueGroup == item.UniqueGroup))
                        continue; //can't have multiple from a unique group (i.e. arcane background)

                var requirements = item.Requires.Split(',').Select(e => e.Trim());
                if (requirements.All(c => result.HasFeature(c, bornAHero)))
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

        void PickPower(Character result, PowerGroup group, Dice dice, bool bornAHero)
        {

            var powers = new List<SettingPower>();
            var trappings = new List<SettingTrapping>();

            foreach (var item in Powers)
            {
                if (group.AvailablePowers.Count > 0 && !group.AvailablePowers.Contains(item.Name))
                    continue;

                var requirements = item.Requires.Split(',').Select(e => e.Trim());
                if (requirements.All(c => result.HasFeature(c, bornAHero)))
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
        void PickRacialEdge(Character result, Dice dice, SettingRace race, bool bornAHero)
        {
            var table = new Table<SettingEdge>();

            foreach (var item in race.RacialEdges)
            {
                //TODO: Some edges allow duplicates
                if (result.Edges.Any(e => e.Name == item.Name))
                    continue; //no dups

                if (!string.IsNullOrEmpty(item.UniqueGroup))
                    if (result.Edges.Any(e => e.UniqueGroup == item.UniqueGroup))
                        continue; //can't have multiple from a unique group (i.e. arcane background)

                var requirements = item.Requires.Split(',').Select(e => e.Trim());
                if (requirements.All(c => result.HasFeature(c, bornAHero)))
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
    }
}


