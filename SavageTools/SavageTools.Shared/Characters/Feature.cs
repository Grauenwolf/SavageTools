using System;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class Feature : ChangeTrackingModelBase
    {
        public string Name { get => Get<string>(); set => Set(value); }
        public Feature Clone()
        {
            return new Feature() { Name = Name };
        }

    }
}