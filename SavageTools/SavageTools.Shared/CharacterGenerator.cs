using SavageTools.Characters;
using SavageTools.Settings;
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
        public ObservableCollectionExtended<SettingSkill> Skills { get; } = new ObservableCollectionExtended<SettingSkill>();

        public ObservableCollectionExtended<SettingHindrance> Hindrances { get; } = new ObservableCollectionExtended<SettingHindrance>();

        public ObservableCollectionExtended<SettingEdge> Edges { get; } = new ObservableCollectionExtended<SettingEdge>();

        public ObservableCollectionExtended<SettingArchetype> Archetypes { get; } = new ObservableCollectionExtended<SettingArchetype>();

        public SettingArchetype SelectedArchetype { get { return Get<SettingArchetype>(); } set { Set(value); } }


        public bool WildCard { get { return GetDefault(true); } set { Set(value); } }


        public Character GenerateCharacter()
        {
            var dice = new Dice();
            var result = new Character();
            result.Archetype = SelectedArchetype.Name;
            result.IsWildCard = WildCard;

            if (SelectedArchetype.UnusedAdvancesSpecified)
                result.UnusedAdvances = SelectedArchetype.UnusedAdvances;
            result.UnusedAttributes = SelectedArchetype.UnusedAttributes;
            result.UnusedEdges = SelectedArchetype.UnusedEdges;
            result.UnusedSkills = SelectedArchetype.UnusedSkills;

            //Add all possible skills
            foreach (var item in Skills)
                result.Skills.Add(new Skill() { Name = item.Name, Attribute = item.Attribute, Trait = 0 });

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
                            table.Add(item, item.Trait.Score); //Raising skills above the controlling attribute is realatively rare
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
                        if (result.Edges.Any(e => e.Name == item.Name))
                            continue; //no dups

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
                        result.Edges.Add(new Edge() { Name = edge.Name, Description = edge.Description });
                        //TODO: edge effects

                        result.UnusedEdges -= 1;
                    }
                }

                if (result.UnusedHindrances > 0)
                    result.UnusedHindrances -= 1; //TODO

                //only apply an advance when everything else has been used
                if (result.UnusedAdvances > 0 && result.UnusedAttributes == 0 && result.UnusedSkills == 0 && result.UnusedEdges == 0 && result.UnusedHindrances == 0)
                {
                    result.UnusedAdvances -= 1;
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

            //Remove the skills that were not chosen
            foreach (var item in result.Skills.Where(s => s.Trait == 0).ToList())
                result.Skills.Remove(item);

            return result;
        }

        static readonly XmlSerializer SettingXmlSerializer = new XmlSerializer(typeof(Setting));

        public void LoadSetting(FileInfo file)
        {
            var currentArchetype = SelectedArchetype?.Name;
            Setting book;
            // Open document
            using (var stream = file.OpenRead())
                book = (Setting)SettingXmlSerializer.Deserialize(stream);

            //Adding a book overwrites the previous book.
            foreach (var item in book.Skills)
            {
                Skills.RemoveAll(s => s.Name == item.Name);
                Skills.Add(item);
            }
            foreach (var item in book.Edges)
            {
                Edges.RemoveAll(s => s.Name == item.Name);
                Edges.Add(item);
            }
            foreach (var item in book.Hindrances)
            {
                Hindrances.RemoveAll(s => s.Name == item.Name);
                Hindrances.Add(item);
            }
            foreach (var item in book.Archetypes)
            {
                Archetypes.RemoveAll(s => s.Name == item.Name);
                Archetypes.Add(item);
            }

            if (currentArchetype != null && SelectedArchetype == null) //selected archetype was replaced so we need to reselect it
                SelectedArchetype = Archetypes.Single(a => a.Name == currentArchetype);
        }
    }




}


