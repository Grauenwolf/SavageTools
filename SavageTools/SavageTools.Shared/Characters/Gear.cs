using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class Gear : ChangeTrackingModelBase
    {
        public string Name { get => Get<string>(); set => Set(value); }
        public string Description { get => Get<string>(); set => Set(value); }

    }
}