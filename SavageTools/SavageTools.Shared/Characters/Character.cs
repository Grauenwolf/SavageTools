using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class Character : ChangeTrackingModelBase
    {
        public Trait Agility { get => Get<Trait>(); set => Set(value); }
        public string Archetype { get { return Get<string>(); } set { Set(value); } }
        public int Armor { get { return Get<int>(); } set { Set(value); } }
        public int Charisma { get { return Get<int>(); } set { Set(value); } }
        public int? Fear { get { return Get<int?>(); } set { Set(value); } }
        public EdgeCollection Edges => GetNew<EdgeCollection>();
        public int Experience { get => Get<int>(); set => Set(value); }
        public FeatureCollection Features => GetNew<FeatureCollection>();
        public GearCollection Gear => GetNew<GearCollection>();
        public string Gender { get => Get<string>(); set => Set(value); }
        public HindranceCollection Hindrances => GetNew<HindranceCollection>();
        public bool IsWildCard { get { return Get<bool>(); } set { Set(value); } }
        public Trait MaxAgility { get => GetDefault<Trait>(12); set => Set(value); }
        public int MaximumStrain { get { return Get<int>(); } set { Set(value); } }
        [CalculatedField("MaximumStrain,Spirit,Vigor")]
        public int MaximumStrainTotal => MaximumStrain + Math.Min(Spirit.Score, Vigor.Score);

        public Trait MaxSmarts { get => GetDefault<Trait>(12); set => Set(value); }
        public Trait MaxSpirit { get => GetDefault<Trait>(12); set => Set(value); }
        public Trait MaxStrength { get => GetDefault<Trait>(12); set => Set(value); }
        public Trait MaxVigor { get => GetDefault<Trait>(12); set => Set(value); }
        public string Name { get => Get<string>(); set => Set(value); }
        public int Pace { get { return GetDefault(6); } set { Set(value); } }
        public int Parry { get { return Get<int>(); } set { Set(value); } }
        public int ParryTotal => 2 + (Skills.SingleOrDefault(s => s.Name == "Fighting")?.Trait.HalfScore ?? 0);

        public PersonalityCollection Personality => GetNew<PersonalityCollection>();
        public PowerGroupCollection PowerGroups => GetNew<PowerGroupCollection>();
        public string Race { get { return Get<string>(); } set { Set(value); } }
        public string Rank { get { return Get<string>(); } set { Set(value); } }
        public int Reason { get { return Get<int>(); } set { Set(value); } }
        [CalculatedField("Reason,Spirit")]
        public int ReasonTotal => 2 + Spirit.HalfScore + Reason;

        public Trait Running { get { return GetDefault<Trait>(6); } set { Set(value); } }
        public int Size { get { return Get<int>(); } set { Set(value); } }

        [CalculatedField("Size")]
        public string SizeDescription
        {
            get
            {
                if (Size < -6)
                    return "Swarm";

                switch (Size)
                {
                    case -6: return "1/2  to 2 oz ";
                    case -5: return "2  to 8 oz ";
                    case -4: return "8 oz  to 2 lbs";
                    case -3: return "2  to 8 lbs";
                    case -2: return "8 to 31 lbs";
                    case -1: return "31 to 125 lbs";
                    case 0: return "125  to 250 lbs";
                    case 1: return "250  to 500 lbs";
                    case 2: return "500  to 1,000 lbs";
                    case 3: return "1,000 lbs to 1 ton ";
                    case 4: return "1  to 2 tons";
                    case 5: return "2  to 4 tons";
                    case 6: return "4  to 8 tons";
                    case 7: return "8  to 16 tons";
                    case 8: return "16  to 32 tons";
                    case 9: return "32  to 64 tons";
                    case 10: return "64 tons+";
                    default: return "";
                }
            }
        }

        public SkillCollection Skills => GetNew<SkillCollection>();
        public Trait Smarts { get => Get<Trait>(); set => Set(value); }
        public Trait Spirit { get => Get<Trait>(); set => Set(value); }
        public int Status { get { return GetDefault(2); } set { Set(value); } }
        public int Strain { get { return Get<int>(); } set { Set(value); } }
        public Trait Strength { get => Get<Trait>(); set => Set(value); }
        public int Toughness { get { return Get<int>(); } set { Set(value); } }

        [CalculatedField("Vigor,Armor,Toughness")]
        public int ToughnessTotal => 2 + Vigor.HalfScore + Toughness + Armor;
        public int UnusedAdvances { get { return Get<int>(); } set { Set(value); } }
        public int UnusedAttributes { get { return Get<int>(); } set { Set(value); } }
        public int UnusedEdges { get { return Get<int>(); } set { Set(value); } }
        public int UnusedHindrances { get { return Get<int>(); } set { Set(value); } }
        public int UnusedIconicEdges { get { return Get<int>(); } set { Set(value); } }
        public int UnusedRacialEdges { get { return Get<int>(); } set { Set(value); } }
        public int UnusedSkills { get { return Get<int>(); } set { Set(value); } }
        public int UnusedSmartSkills { get { return Get<int>(); } set { Set(value); } }
        public bool UseReason { get => Get<bool>(); set => Set(value); }
        public bool UseStatus { get => Get<bool>(); set => Set(value); }
        public bool UseStrain { get => Get<bool>(); set => Set(value); }
        public Trait Vigor { get => Get<Trait>(); set => Set(value); }
        /// <summary>
        /// Determines whether the specified feature has feature.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <param name="ignoreRank">if set to <c>true</c> to ignore rank. This is used by the "Born a Hero" option.</param>
        /// <returns><c>true</c> if the specified feature has feature; otherwise, <c>false</c>.</returns>
        public bool HasFeature(string feature, bool ignoreRank)
        {
            if (string.IsNullOrEmpty(feature))
                throw new ArgumentException($"{nameof(feature)} is null or empty.", nameof(feature));

            if (feature.Contains(" or "))
            {
                var parts = feature.Split(new[] { " or " }, StringSplitOptions.None).Select(f => f.Trim());
                foreach (var part in parts)
                {
                    if (HasFeature(part, ignoreRank))
                        return true;
                }
                return false;
            }

            var name = feature;
            Trait? trait = null;

            if (name.EndsWith(" d4"))
            {
                trait = 4;
                name = name.Substring(0, name.Length - 3);
            }
            else if (name.EndsWith(" d6"))
            {
                trait = 6;
                name = name.Substring(0, name.Length - 3);
            }
            else if (name.EndsWith(" d8"))
            {
                trait = 8;
                name = name.Substring(0, name.Length - 3);
            }
            else if (name.EndsWith(" d10"))
            {
                trait = 10;
                name = name.Substring(0, name.Length - 4);
            }
            else if (name.EndsWith(" d12"))
            {
                trait = 12;
                name = name.Substring(0, name.Length - 4);
            }

            //Check for ranks
            switch (name)
            {
                case "Novice": return true;
                case "Seasoned": return ignoreRank || Experience >= 20;
                case "Veteran": return ignoreRank || Experience >= 40;
                case "Heroic": return ignoreRank || Experience >= 60;
                case "Legendary": return ignoreRank || Experience >= 80;
            }

            if (name == "Wild Card")
                return IsWildCard;

            //Check for attributes
            switch (name)
            {
                case "Vigor": return trait == null || Vigor >= trait;
                case "Smarts": return trait == null || Smarts >= trait;
                case "Agility": return trait == null || Agility >= trait;
                case "Strength": return trait == null || Strength >= trait;
                case "Spirit": return trait == null || Spirit >= trait;

                case "Status": return trait == null || Status >= trait;
                case "Reason": return trait == null || ReasonTotal >= trait;
            }

            //Check for skills
            foreach (var skill in Skills.Where(s => s.Trait > 0))
            {
                if (skill.Name == name)
                    return (trait == null) || (skill.Trait > trait);
            }

            //Check for edges
            if (Edges.Any(e => e.Name == name))
                return true;

            //Check for hindrances
            if (Hindrances.Any(e => e.Name == name))
                return true;

            //Check for actual features
            if (Features.Any(e => e.Name == name))
                return true;

            Debug.WriteLine("Does not have feature " + feature);
            return false;
        }

        public void Increment(string trait, Dice dice) => Increment(trait, 1, dice);

        public void Increment(string trait, int bonus, Dice dice)
        {
            if (string.IsNullOrEmpty(trait))
                throw new ArgumentException($"{nameof(trait)} is null or empty.", nameof(trait));

            if (dice == null)
                throw new ArgumentNullException(nameof(dice), $"{nameof(dice)} is null.");

            switch (trait)
            {
                case "Vigor": Vigor += bonus; return;
                case "Smarts": Smarts += bonus; return;
                case "Agility": Agility += bonus; return;
                case "Strength": Strength += bonus; return;
                case "Spirit": Spirit += bonus; return;

                case "MaxVigor": MaxVigor += bonus; return;
                case "MaxSmarts": MaxSmarts += bonus; return;
                case "MaxAgility": MaxAgility += bonus; return;
                case "MaxStrength": MaxStrength += bonus; return;
                case "MaxSpirit": MaxSpirit += bonus; return;

                case "Pace": Pace += bonus; return;
                case "Running": Running += bonus; return;
                case "Charisma": Charisma += bonus; return;
                case "Fear": Fear += bonus; return;
                case "Parry": Parry += bonus; return;
                case "Toughness": Toughness += bonus; return;
                case "Strain": Strain += bonus; return;
                case "MaximumStrain": MaximumStrain += bonus; return;
                case "Reason": Reason += bonus; return;
                case "Status": Status += bonus; return;
                case "Size": Size += bonus; return;
                case "Armor": Armor += bonus; return;


                case "UnusedAttributes": UnusedAttributes += bonus; return;
                case "UnusedSkills": UnusedSkills += bonus; return;
                case "UnusedSmartSkills": UnusedSmartSkills += bonus; return;
                case "UnusedEdges": UnusedEdges += bonus; return;
                case "UnusedRacialEdges": UnusedRacialEdges += bonus; return;
                case "UnusedHindrances": UnusedHindrances += bonus; return;
                case "UnusedAdvances": UnusedAdvances += bonus; return;
                case "PowerPoints": dice.Choose(PowerGroups).PowerPoints += bonus; return;
                case "UnusedPowers": dice.Choose(PowerGroups).UnusedPowers += bonus; return;
            }

            if (trait.StartsWith("PowerPoints:"))
            {
                PowerGroups[trait.Substring("PowerPoints:".Length)].PowerPoints += bonus;
                return;
            }

            if (trait.StartsWith("UnusedPowers:"))
            {
                PowerGroups[trait.Substring("UnusedPowers:".Length)].UnusedPowers += bonus;
                return;
            }

            throw new ArgumentException("Unknown trait " + trait);
        }

        internal Trait GetAttribute(string attribute)
        {
            switch (attribute)
            {
                case "Vigor": return Vigor;
                case "Smarts": return Smarts;
                case "Agility": return Agility;
                case "Strength": return Strength;
                case "Spirit": return Spirit;
            }
            throw new ArgumentException("Unknown attribute " + attribute);
        }

        public Character Clone()
        {
            var result = new Character()
            {
                Agility = Agility,
                Archetype = Archetype,
                Armor = Armor,
                Charisma = Charisma,
                Experience = Experience,
                Fear = Fear,
                Gender = Gender,
                IsWildCard = IsWildCard,
                MaxAgility = MaxAgility,
                MaximumStrain = MaximumStrain,
                MaxSmarts = MaxSmarts,
                MaxSpirit = MaxSpirit,
                MaxVigor = MaxVigor,
                MaxStrength = MaxStrength,
                Name = Name,
                Pace = Pace,
                Parry = Parry,
                Race = Race,
                Rank = Rank,
                Reason = Reason,
                Size = Size,
                Smarts = Smarts,
                Spirit = Spirit,
                Status = Status,
                Strain = Strain,
                Strength = Strength,
                Toughness = Toughness,
                UnusedAdvances = UnusedAdvances,
                UnusedAttributes = UnusedAttributes,
                UnusedEdges = UnusedEdges,
                UnusedHindrances = UnusedHindrances,
                UnusedIconicEdges = UnusedIconicEdges,
                UnusedRacialEdges = UnusedRacialEdges,
                UnusedSkills = UnusedSkills,
                UnusedSmartSkills = UnusedSmartSkills,
                UseReason = UseReason,
                UseStatus = UseStatus,
                UseStrain = UseStrain,
                Running = Running,
                Vigor = Vigor,

            };

            result.Edges.AddRange(Edges.Select(e => e.Clone()));
            result.Features.AddRange(Features.Select(e => e.Clone()));
            result.Skills.AddRange(Skills.Select(e => e.Clone()));
            result.PowerGroups.AddRange(PowerGroups.Select(e => e.Clone()));
            result.Gear.AddRange(Gear.Select(e => e.Clone()));
            result.Hindrances.AddRange(Hindrances.Select(e => e.Clone()));

            return result;
        }

        public void CopyToStory(StoryBuilder story, bool indentAfterName = false)
        {
            if (story == null)
                throw new ArgumentNullException(nameof(story), $"{nameof(story)} is null.");

            if (!string.IsNullOrEmpty(Gender))
                story.Append($"{Name} ({Gender})");
            else
                story.Append($"{Name}");

            if (indentAfterName)
                story.IncreaseIndent();

            if (!string.IsNullOrWhiteSpace(Archetype) && Archetype != "(None)")
                story.Append($", {Archetype}");
            if (Race != "Human")
                story.Append($", {Race}");
            if (Rank != "Novice")
                story.Append($", Rank {Rank}");
            story.AppendLine();

            story.AppendLine($"Agility {Agility}, Smarts {Smarts}, Strength {Strength}, Spirt {Spirit}, Vigor {Vigor}");

            story.Append($"Charisma {Charisma}, Parry {ParryTotal}, Toughness {ToughnessTotal}, Pace {Pace}+{Running}, Size {Size}, ");

            var additionTraits = new List<string>();

            if (Fear == 0)
                additionTraits.Add($"Fear");
            else if (Fear.HasValue)
                additionTraits.Add($"Fear {Fear}");

            if (UseReason)
                additionTraits.Add($"Reason {ReasonTotal}");
            if (UseStatus)
                additionTraits.Add($"Status {Status}");
            if (UseStrain)
                additionTraits.Add($"Strain {Strain}/{MaximumStrainTotal}");

            if (additionTraits.Count > 0)
                story.AppendLine(string.Join(", ", additionTraits));


            story.AppendLine(string.Join(", ", Skills.Select(s => s.ShortName)));
            story.AppendLine(string.Join(", ", Edges.Select(e => e.ToString())));
            story.AppendLine(string.Join(", ", Hindrances.Select(h => h.ToString())));
            story.AppendLine(string.Join(", ", Features.Select(h => h.Name)));
            story.AppendLine(string.Join(", ", Personality.Select(h => h.Name)));

            foreach (var group in PowerGroups)
                story.AppendLine($"{group.Skill}, Power Points {group.PowerPoints}, Powers: {string.Join(", ", group.Powers.Select(p => p.LongName))}");

            story.AppendLine(string.Join(", ", Gear.Select(h => h.Name + (string.IsNullOrEmpty(h.Description) ? "" : ": " + h.Description))));

            if (indentAfterName)
                story.DecreaseIndent();
        }
    }
}

