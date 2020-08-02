using SavageTools.Settings;
using System;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class Power : ChangeTrackingModelBase
    {
        public Power(SettingPower power, string trapping, string trigger)
        {
            //if (string.IsNullOrEmpty(name))
            //    throw new ArgumentException($"{nameof(name)} is null or empty.", nameof(name));

            if (string.IsNullOrEmpty(trapping))
                throw new ArgumentException($"{nameof(trapping)} is null or empty.", nameof(trapping));

            Name = power.Name;
            Trapping = trapping;
            Trigger = trigger;
            PowerPoints = power.PowerPoints;
            Description = power.Description;
        }

        public string LongName => $"{Name} [{Trigger} => {Trapping}]";
        public string Name { get => Get<string>(); set => Set(value); }
        public string Trapping { get => Get<string>(); set => Set(value); }
        public string Trigger { get => Get<string>(); set => Set(value); }
        public string Description { get => Get<string>(); set => Set(value); }
        public int? PowerPoints { get => Get<int?>(); set => Set(value); }

        public override string ToString() => LongName;

        //public Power Clone()
        //{
        //    return new Power(Name, Trapping, Trigger, PowerPoints);
        //}
    }
}
