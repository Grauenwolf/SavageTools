using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class Hindrance : ChangeTrackingModelBase
    {
        public string Name { get { return Get<string>(); } set { Set(value); } }

        /// <summary>
        /// Level 1 = minor, level 2 = major
        /// Level 0 means it is a built-in complication that doesn't count towards the hindrence limit
        /// </summary>
        public int Level { get { return Get<int>(); } set { Set(value); } }
    }
}

