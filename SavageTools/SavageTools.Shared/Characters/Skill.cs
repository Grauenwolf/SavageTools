using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{

    public class Skill : ChangeTrackingModelBase
    {
        public string Name { get { return Get<string>(); } set { Set(value); } }
        public string Attribute { get { return Get<string>(); } set { Set(value); } }
        public Trait Trait { get { return GetDefault<Trait>(4); } set { Set(value); } }

        public string LongName
        {
            get { return $"{Name} [{Attribute}] {Trait}"; }
        }
    }


}

