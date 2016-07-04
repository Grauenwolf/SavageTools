using System;
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
    }
}

