﻿using System;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class Power : ChangeTrackingModelBase
    {
        public Power(string name, string trapping)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"{nameof(name)} is null or empty.", nameof(name));

            if (string.IsNullOrEmpty(trapping))
                throw new ArgumentException($"{nameof(trapping)} is null or empty.", nameof(trapping));


            Name = name;
            Trapping = trapping;
        }

        public string LongName => $"{Name} [{Trapping}]";
        public string Name { get => Get<string>(); set => Set(value); }
        public string Trapping { get => Get<string>(); set => Set(value); }
        public override string ToString() => LongName;
        public Power Clone()
        {
            return new Power(Name, Trapping);
        }
    }
}

