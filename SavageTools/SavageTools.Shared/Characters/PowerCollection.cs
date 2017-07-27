using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{

    public class PowerCollection : ChangeTrackingModelCollection<PowerGroup>
    {
        public PowerCollection()
        {
            ItemPropertyChanged += (s, e) => OnPropertyChanged("UnusedPowers");
            CollectionChanged += (s, e) => OnPropertyChanged("UnusedPowers");
        }

        public PowerGroup this[string skill]
        {
            get
            {
                var result = this.FirstOrDefault(s => s.Skill == skill);
                if (result == null)
                {
                    result = new PowerGroup() { Skill = skill };
                    Add(result);
                }
                return result;
            }
        }

        public int UnusedPowers => this.Sum(p => p.UnusedPowers);

        public bool ContainsPower(string power, string trapping)
        {
            return this.Any(g => g.Any(p => p.Name == power && p.Trapping == trapping));
        }

    }
}

