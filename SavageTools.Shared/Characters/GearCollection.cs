using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class GearCollection : ChangeTrackingModelCollection<Gear>
    {
        public void Add(string name, string? description = null)
        {
            Add(new Gear() { Name = name, Description = description });
        }
    }
}
