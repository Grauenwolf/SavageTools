using System;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class Gear : ChangeTrackingModelBase
    {
        public string? Description { get => Get<string?>(); set => Set(value); }
        public string Name { get => Get<string>(); set => Set(value); }

        public Gear Clone()
        {
            return new Gear() { Name = Name, Description = Description };
        }
    }
}
