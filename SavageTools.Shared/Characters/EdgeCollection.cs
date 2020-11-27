using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class EdgeCollection : ChangeTrackingModelCollection<Edge>
    {
        public void Add(string name)
        {
            Add(new Edge() { Name = name });
        }

        public void Add(string name, string? description)
        {
            Add(new Edge() { Name = name, Description = description });
        }
    }
}
