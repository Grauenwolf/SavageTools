using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class Personality : ChangeTrackingModelBase
    {
        public string Name { get => Get<string>(); set => Set(value); }

    }
}