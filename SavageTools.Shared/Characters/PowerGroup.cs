using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class PowerGroup : ChangeTrackingModelBase
    {
        /// <summary>
        /// Gets the available powers.
        /// </summary>
        /// <value>The available powers.</value>
        /// <remarks>If empty, uses the default collection</remarks>
        public StringCollection AvailablePowers => GetNew<StringCollection>();

        /// <summary>
        /// Gets the list of available triggers.
        /// </summary>
        /// <remarks>If empty, no trigger will be generated.</remarks>
        public StringCollection AvailableTriggers => GetNew<StringCollection>();

        /// <summary>
        /// Gets the available trappings.
        /// </summary>
        /// <value>The available trappings.</value>
        /// <remarks>If empty, uses the default collection</remarks>
        public StringCollection AvailableTrappings => GetNew<StringCollection>();

        public string PowerList => string.Join(", ", Powers.Select(p => p.LongName));
        public int PowerPoints { get => Get<int>(); set => Set(value); }
        public PowerCollection Powers => GetNew<PowerCollection>();

        /// <summary>
        /// Gets the prohibited trappings.
        /// </summary>
        /// <value>The prohibited trappings.</value>
        public StringCollection ProhibitedTrappings => GetNew<StringCollection>();

        public string PowerType { get => Get<string>(); set => Set(value); }

        public int UnusedPowers { get => Get<int>(); set => Set(value); }

        //public PowerGroup Clone()
        //{
        //    var result = new PowerGroup() { PowerPoints = PowerPoints, PowerType = PowerType, UnusedPowers = UnusedPowers };

        //    result.ProhibitedTrappings.AddRange(ProhibitedTrappings);
        //    result.AvailableTrappings.AddRange(AvailableTrappings);
        //    result.Powers.AddRange(Powers.Select(x => x.Clone()));

        //    return result;
        //}
    }
}
