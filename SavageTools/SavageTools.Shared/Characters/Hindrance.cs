using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class Hindrance : ChangeTrackingModelBase
    {
        public string Name { get => Get<string>(); set => Set(value); }
        public string Description { get => Get<string>(); set => Set(value); }

        /// <summary>
        /// Level 1 = minor, level 2 = major
        /// Level 0 means it is a built-in complication that doesn't count towards the hindrance limit
        /// </summary>
        public int Level { get => Get<int>(); set => Set(value); }

        public string LevelName
        {
            get
            {
                switch (Level)
                {
                    case 0: return "";
                    case 1: return "(Minor)";
                    case 2: return "(Major)";
                }
                return "";
            }
        }
    }
}

