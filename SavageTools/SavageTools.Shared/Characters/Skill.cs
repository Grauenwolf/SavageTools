using System;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{

    public class Skill : ChangeTrackingModelBase
    {
        public Skill(string name, string attribute)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"{nameof(name)} is null or empty.", nameof(name));

            if (string.IsNullOrEmpty(attribute))
                throw new ArgumentException($"{nameof(attribute)} is null or empty for skill {name}.", nameof(attribute));

            Name = name;
            Attribute = attribute;
        }

        public string Name { get { return Get<string>(); } set { Set(value); } }
        public string Attribute { get { return Get<string>(); } set { Set(value); } }
        public Trait Trait { get { return GetDefault<Trait>(4); } set { Set(value); } }

        public string LongName
        {
            get { return $"{Name} [{Attribute}] {Trait}"; }
        }

        public override string ToString()
        {
            return LongName;
        }
    }
}

