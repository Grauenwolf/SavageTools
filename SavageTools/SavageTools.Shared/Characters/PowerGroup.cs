using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class PowerGroup : ChangeTrackingModelCollection<Power>
    {
        public string Skill { get => Get<string>(); set => Set(value); }

        public int UnusedPowers { get => Get<int>(); set => Set(value); }

        public int PowerPoints { get => Get<int>(); set => Set(value); }

        public string PowerList => string.Join(", ", this.Select(p => p.LongName));
    }
}

