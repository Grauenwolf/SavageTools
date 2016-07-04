using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{

    public class Edge : ChangeTrackingModelBase
    {
        public string Name { get { return Get<string>(); } set { Set(value); } }

    }
}

