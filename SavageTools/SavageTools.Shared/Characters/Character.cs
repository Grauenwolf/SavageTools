using System;
using System.Diagnostics;
using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class Character : ChangeTrackingModelBase
    {
        public string Name { get { return Get<string>(); } set { Set(value); } }
        public Trait Agility { get { return Get<Trait>(); } set { Set(value); } }
        public Trait Smarts { get { return Get<Trait>(); } set { Set(value); } }
        public Trait Strength { get { return Get<Trait>(); } set { Set(value); } }
        public Trait Spirit { get { return Get<Trait>(); } set { Set(value); } }
        public Trait Vigor { get { return Get<Trait>(); } set { Set(value); } }

        public SkillCollection Skills { get { return GetNew<SkillCollection>(); } }
        public HindranceCollection Hindrances { get { return GetNew<HindranceCollection>(); } }
        public EdgeCollection Edges { get { return GetNew<EdgeCollection>(); } }
        public FeatureCollection Features { get { return GetNew<FeatureCollection>(); } }

        public int Experience { get { return Get<int>(); } set { Set(value); } }

        public int UnusedAttributes { get { return Get<int>(); } set { Set(value); } }
        public int UnusedSkills { get { return Get<int>(); } set { Set(value); } }
        public int UnusedSmartSkills { get { return Get<int>(); } set { Set(value); } }
        public int UnusedEdges { get { return Get<int>(); } set { Set(value); } }
        public int UnusedHindrances { get { return Get<int>(); } set { Set(value); } }
        public int UnusedAdvances { get { return Get<int>(); } set { Set(value); } }



        public bool IsWildCard { get { return Get<bool>(); } set { Set(value); } }

        public string Archetype { get { return Get<string>(); } set { Set(value); } }
        public string Race { get { return Get<string>(); } set { Set(value); } }
        public string Rank { get { return Get<string>(); } set { Set(value); } }

        public int Pace { get { return GetDefault(6); } set { Set(value); } }
        public Trait Running { get { return GetDefault<Trait>(6); } set { Set(value); } }

        public int Charisma { get { return Get<int>(); } set { Set(value); } }
        public int Parry { get { return Get<int>(); } set { Set(value); } }
        public int Toughness { get { return Get<int>(); } set { Set(value); } }
        public int Strain { get { return Get<int>(); } set { Set(value); } }
        public int MaximumStrain { get { return Get<int>(); } set { Set(value); } }


        public int ParryTotal
        {
            get { return 2 + (Skills.SingleOrDefault(s => s.Name == "Fighting")?.Trait.HalfScore ?? 0); }
        }

        public int ToughnessTotal
        {
            get { return 2 + Vigor.HalfScore; }
        }

        public int MaximumStrainTotal
        {
            get { return MaximumStrain + Math.Min(Spirit.Score, Vigor.Score); }
        }

        public void Increment(string trait, Dice dice)
        {
            Increment(trait, 1, dice);
        }

        public void Increment(string trait, int bonus, Dice dice)
        {
            switch (trait)
            {
                case "Vigor": Vigor += bonus; return;
                case "Smarts": Smarts += bonus; return;
                case "Agility": Agility += bonus; return;
                case "Strength": Strength += bonus; return;
                case "Spirit": Spirit += bonus; return;

                case "Pace": Pace += bonus; return;
                case "Running": Running += bonus; return;
                case "Charisma": Charisma += bonus; return;
                case "Parry": Parry += bonus; return;
                case "Toughness": Toughness += bonus; return;
                case "Strain": Strain += bonus; return;
                case "MaximumStrain": MaximumStrain += bonus; return;


                case "UnusedAttributes": UnusedAttributes += bonus; return;
                case "UnusedSkills": UnusedSkills += bonus; return;
                case "UnusedEdges": UnusedEdges += bonus; return;
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

        /// <summary>
        /// Determines whether the specified feature has feature.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <param name="ignoreRank">if set to <c>true</c> to ignore rank. This is used by the "Born a Hero" option.</param>
        /// <returns><c>true</c> if the specified feature has feature; otherwise, <c>false</c>.</returns>
        public bool HasFeature(string feature, bool ignoreRank)
        {
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

        public PowerGroupCollection PowerGroups => GetNew<PowerGroupCollection>();
    }
}

