using SavageTools.Characters;
using SavageTools.Settings;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public SettingArchetype SelectedArchetype { get { return Get<SettingArchetype>(); } set { Set(value); } }

        public SettingRace SelectedRace { get { return Get<SettingRace>(); } set { Set(value); } }

        public SettingRank SelectedRank { get { return Get<SettingRank>(); } set { Set(value); } }


        public bool WildCard { get { return GetDefault(true); } set { Set(value); } }


        public Character GenerateCharacter()
        {
            var dice = new Dice();
            var result = new Character();
            result.Archetype = SelectedArchetype.Name;
            result.Race = SelectedRace.Name;
            result.Rank = SelectedRank.Name;
            result.IsWildCard = WildCard;

            //Add all possible skills (except ones created by edges)
            foreach (var item in Skills)
                result.Skills.Add(new Skill() { Name = item.Name, Attribute = item.Attribute, Trait = 0 });

            //Add the archetype

            result.UnusedAttributes = SelectedArchetype.UnusedAttributes;
            result.UnusedSkills = SelectedArchetype.UnusedSkills;
            result.Agility = SelectedArchetype.Agility;
            result.Smarts = SelectedArchetype.Smarts;
            result.Spirit = SelectedArchetype.Spirit;
            result.Strength = SelectedArchetype.Strength;
            result.Vigor = SelectedArchetype.Vigor;


            if (SelectedArchetype.Skills != null)
                foreach (var item in SelectedArchetype.Skills)
                {
                    var skill = result.Skills[item.Name];
                    if (skill != null)
                        skill.Trait = Math.Max(skill.Trait.Score, item.Level);
                    else
                        result.Skills.Add(new Skill() { Name = item.Name, Attribute = item.Attribute, Trait = item.Level });
                }
            if (SelectedArchetype.Traits != null)
                foreach (var item in SelectedArchetype.Traits)
                    result.Increment(item.Name, item.Bonus);

            if (SelectedArchetype.Edges != null)
                foreach (var item in SelectedArchetype.Edges)
                    ApplyEdge(result, item);

            //Add the race
            if (SelectedRace.Skills != null)
                foreach (var item in SelectedRace.Skills)
                {
                    var skill = result.Skills[item.Name];
                    if (skill != null)
                        skill.Trait = Math.Max(skill.Trait.Score, item.Level);
                    else
                        result.Skills.Add(new Skill() { Name = item.Name, Attribute = item.Attribute, Trait = item.Level });
                }
            if (SelectedRace.Traits != null)
                foreach (var item in SelectedRace.Traits)
                    result.Increment(item.Name, item.Bonus);

            if (SelectedRace.Edges != null)
                foreach (var item in SelectedRace.Edges)
                    ApplyEdge(result, item);

            //Add the rank
            result.UnusedAdvances = SelectedRank.UnusedAdvances;
            result.Experience = SelectedRank.Experience;

            //Main loop for random picks
            while (result.UnusedAttributes > 0 || result.UnusedSkills > 0 || result.UnusedAdvances > 0 || result.UnusedEdges > 0 || result.UnusedHindrances > 0)
            {
                if (result.UnusedAttributes > 0)
                {
                    //Attributes are likely to stack rather than spread evenly
                    var table = new Table<string>();
                    if (result.Vigor < 12)
                        table.Add("Vigor", result.Vigor.Score);
                    if (result.Smarts < 12)
                        table.Add("Smarts", result.Smarts.Score);
                    if (result.Agility < 12)
                        table.Add("Agility", result.Agility.Score);
                    if (result.Strength < 12)
                        table.Add("Strength", result.Strength.Score);
                    if (result.Spirit < 12)
                        table.Add("Spirit", result.Spirit.Score);

                    result.Increment(table.RandomChoose(dice));

                    result.UnusedAttributes -= 1;
                }

                if (result.UnusedSkills > 0)
                {
                    bool allowHigh = result.UnusedSkills >= 2 && result.UnusedAttributes == 0; //don't buy expensive skills until all of the attributes are picked

                    var table = new Table<Skill>();
                    foreach (var item in result.Skills)
                    {
                        if (item.Trait == 0)
                            table.Add(item, result.GetAttribute(item.Attribute).Score - 3); //favor skills that match your attributes
                        else if (item.Trait < result.GetAttribute(item.Attribute)) //favor improving what you know
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



                if (result.UnusedEdges > 0)
                {
                    var table = new Table<SettingEdge>();
                    foreach (var item in Edges)
                    {
                        //TODO: Some edges allow duplicates
                        if (result.Edges.Any(e => e.Name == item.Name))
                            continue; //no dups

                        if (!string.IsNullOrEmpty(item.UniqueGroup))
                            if (result.Edges.Any(e => e.UniqueGroup == item.UniqueGroup))
                                continue; //can't have multiple from a unique group (i.e. arcane background)

                        var checks = item.Requires.Split(',').Select(e => e.Trim());
                        if (checks.All(c => result.HasFeature(c)))
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
                        ApplyEdge(result, edge);

                        result.UnusedEdges -= 1;
                    }
                }

                if (result.UnusedHindrances > 0)
                    result.UnusedHindrances -= 1; //TODO

                //only apply an advance when everything else has been used
                if (result.UnusedAdvances > 0 && result.UnusedAttributes <= 0 && result.UnusedSkills <= 0 && result.UnusedEdges <= 0 && result.UnusedHindrances <= 0)
                {
                    result.UnusedAdvances -= 1;

                    if (result.UnusedSkills < 0)
                        result.UnusedSkills += 2; //pay back the skill point loan
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

            //Remove the skills that were not chosen
            foreach (var item in result.Skills.Where(s => s.Trait == 0).ToList())
                result.Skills.Remove(item);

            return result;
        }

        static void ApplyEdge(Character result, SettingEdge edge)
        {
            result.Edges.Add(new Edge() { Name = edge.Name, Description = edge.Description, UniqueGroup = edge.UniqueGroup });

            if (edge.Trait != null)
                foreach (var item in edge.Trait)
                    result.Increment(item.Name, item.Bonus);

            if (edge.Features != null)
                foreach (var item in edge.Features)
                    result.Features.Add(item.Name);

            if (edge.Skill != null)
                foreach (var item in edge.Skill)
                {
                    var skill = result.Skills.FirstOrDefault(s => s.Name == item.Name);
                    if (skill == null)
                    {
                        result.Skills.Add(new Skill() { Name = item.Name, Attribute = item.Attribute, Trait = item.Level });
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
                foreach (var item in book.Archetypes)
                {
                    Archetypes.RemoveAll(s => s.Name == item.Name);
                    Archetypes.Add(item);
                }
            if (book.Races != null)
                foreach (var item in book.Races)
                {
                    Races.RemoveAll(s => s.Name == item.Name);
                    Races.Add(item);
                }
            if (book.Ranks != null)
                foreach (var item in book.Ranks)
                {
                    Ranks.RemoveAll(s => s.Name == item.Name);
                    Ranks.Add(item);
                }

            if (currentArchetype != null && SelectedArchetype == null) //selected archetype was replaced so we need to reselect it
                SelectedArchetype = Archetypes.Single(a => a.Name == currentArchetype);

            if (currentRace != null && SelectedRace == null) //selected race was replaced so we need to reselect it
                SelectedRace = Races.Single(a => a.Name == currentRace);

            if (currentRank != null && SelectedRank == null) //selected rank was replaced so we need to reselect it
                SelectedRank = Ranks.Single(a => a.Name == currentRank);
        }
    }





}


