using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class PowerCollection : ChangeTrackingModelCollection<Power>
    {
        public void Add(string name, string trapping)
        {
            Add(new Power(name, trapping));
        }

    }
}

