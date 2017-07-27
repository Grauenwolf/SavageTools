using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class PowerGroup : ChangeTrackingModelBase
    {
        public string Skill { get => Get<string>(); set => Set(value); }

        public int UnusedPowers { get => Get<int>(); set => Set(value); }

        public int PowerPoints { get => Get<int>(); set => Set(value); }

        public string PowerList => string.Join(", ", Powers.Select(p => p.LongName));

        public PowerCollection Powers => GetNew<PowerCollection>();


        /// <summary>
        /// Gets the available powers.
        /// </summary>
        /// <value>The available powers.</value>
        /// <remarks>If empty, uses the default collection</remarks>
        public StringCollection AvailablePowers => GetNew<StringCollection>();


        /// <summary>
        /// Gets the available trappings.
        /// </summary>
        /// <value>The available trappings.</value>
        /// <remarks>If empty, uses the default collection</remarks>
        public StringCollection AvailableTrappings => GetNew<StringCollection>();


        /// <summary>
        /// Gets the prohibited trappings.
        /// </summary>
        /// <value>The prohibited trappings.</value>
        public StringCollection ProhibitedTrappings => GetNew<StringCollection>();

    }

    public class StringCollection : ChangeTrackingModelCollection<string> { }
    public class PowerCollection : ChangeTrackingModelCollection<Power> { }

}

