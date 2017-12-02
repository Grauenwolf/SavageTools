using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{

    public class Edge : ChangeTrackingModelBase
    {
        public string Description { get => Get<string>(); set => Set(value); }
        public string Name { get => Get<string>(); set => Set(value); }
        public string UniqueGroup { get => Get<string>(); set => Set(value); }
    }
}

