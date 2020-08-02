using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class FeatureCollection : ChangeTrackingModelCollection<Feature>
    {
        internal void Add(string name)
        {
            if (!this.Any(f => f.Name == name))
                Add(new Feature() { Name = name });
        }
    }
}