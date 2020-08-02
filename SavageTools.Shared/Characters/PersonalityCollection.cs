using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class PersonalityCollection : ChangeTrackingModelCollection<Personality>
    {
        internal void Add(string name)
        {
            if (!this.Any(f => f.Name == name))
                Add(new Personality() { Name = name });
        }
    }
}