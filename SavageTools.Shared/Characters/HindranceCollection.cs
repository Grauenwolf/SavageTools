using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class HindranceCollection : ChangeTrackingModelCollection<Hindrance>
    {
        public void Add(string name, string description)
        {
            Add(new Hindrance() { Name = name, Description = description, Level = 0 });
        }
    }
}
