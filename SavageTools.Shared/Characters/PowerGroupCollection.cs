using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class PowerGroupCollection : ChangeTrackingModelCollection<PowerGroup>
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PowerGroupCollection()
        {
            ItemPropertyChanged += (s, e) => OnPropertyChanged("UnusedPowers");
            CollectionChanged += (s, e) => OnPropertyChanged("UnusedPowers");
        }

        public int UnusedPowers => this.Sum(p => p.UnusedPowers);

        public PowerGroup this[string powerType]
        {
            get
            {
                var result = this.FirstOrDefault(s => s.PowerType == powerType);
                if (result == null)
                {
                    result = new PowerGroup() { PowerType = powerType };
                    Add(result);
                }
                return result;
            }
        }

        public bool ContainsPower(string power, string trapping)
        {
            return this.Any(g => g.Powers.Any(p => p.Name == power && p.Trapping == trapping));
        }
    }
}
