using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class GearCollection : ChangeTrackingModelCollection<Gear>
    {
        internal void Add(string name, string description)
        {
            if (!this.Any(f => f.Name == name))
                Add(new Gear() { Name = name, Description = description });
        }
    }
}