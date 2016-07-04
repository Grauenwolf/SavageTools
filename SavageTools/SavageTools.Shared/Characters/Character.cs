using System;
using System.Diagnostics;
using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class Character : ChangeTrackingModelBase
    {
        public string Name { get { return Get<string>(); } set { Set(value); } }
        public Trait Agility { get { return GetDefault<Trait>(4); } set { Set(value); } }
        public Trait Smarts { get { return GetDefault<Trait>(4); } set { Set(value); } }
        public Trait Strength { get { return GetDefault<Trait>(4); } set { Set(value); } }
        public Trait Spirit { get { return GetDefault<Trait>(4); } set { Set(value); } }
        public Trait Vigor { get { return GetDefault<Trait>(4); } set { Set(value); } }

        public SkillCollection Skills { get { return GetNew<SkillCollection>(); } }
        public HindranceCollection Hindrances { get { return GetNew<HindranceCollection>(); } }
        public EdgeCollection Edges { get { return GetNew<EdgeCollection>(); } }

        public int Experience { get { return Get<int>(); } set { Set(value); } }

        public int UnusedAttributes { get { return Get<int>(); } set { Set(value); } }
        public int UnusedSkills { get { return Get<int>(); } set { Set(value); } }
        public int UnusedEdges { get { return Get<int>(); } set { Set(value); } }
        public int UnusedHindrances { get { return Get<int>(); } set { Set(value); } }
        public int UnusedAdvances { get { return Get<int>(); } set { Set(value); } }

        public bool IsWildCard { get { return Get<bool>(); } set { Set(value); } }

        public string Archetype { get { return Get<string>(); } set { Set(value); } }

        public void Increment(string trait)
        {
            switch (trait)
            {
                case "Vigor": Vigor += 1; return;
                case "Smarts": Smarts += 1; return;
                case "Agility": Agility += 1; return;
                case "Strength": Strength += 1; return;
                case "Spirit": Spirit += 1; return;

                    //TODO: Check skills
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

        public bool HasFeature(string feature)
        {
            if (feature.Contains(" or "))
            {
                var parts = feature.Split(new[] { " or " }, StringSplitOptions.None).Select(f => f.Trim());
                foreach(var part in parts)
                {
                    if (HasFeature(part))
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
                case "Seasoned": return Experience >= 20;
                case "Veteran": return Experience >= 40;
                case "Heroic": return Experience >= 60;
                case "Legendary": return Experience >= 80;
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
                {
                    return (trait == null) || (skill.Trait > trait);
                }
            }

            //Check for edges
            if (Edges.Any(e => e.Name == name))
                return true;

            //Check for hindrances
            if (Hindrances.Any(e => e.Name == name))
                return true;

            Debug.WriteLine("Does not have feature " + feature);
            return false;
        }
    }
}

