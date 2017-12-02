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

        public string Attribute { get => Get<string>(); set => Set(value); }
        public string LongName => $"{Name} [{Attribute}] {Trait}";
        public string Name { get => Get<string>(); set => Set(value); }
        public string ShortName => $"{Name} {Trait}";
        public Trait Trait { get => GetDefault<Trait>(4); set => Set(value); }
        public override string ToString() => LongName;
    }
}

