using System;
using System.Diagnostics.CodeAnalysis;

namespace SavageTools
{
    [SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags]
    public enum Suit
    {
        BlackJ = 0,
        RedJ = 1,

        Spades = 2,
        Hearts = 3,
        Diamonds = 5,
        Clubs = 4
    }

    public enum CardColor
    {
        Black = 0,
        Red = 1
    }
}
