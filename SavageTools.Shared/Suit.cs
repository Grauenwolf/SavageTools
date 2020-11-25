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

        Spade = 2,
        Heart = 3,
        Diamond = 5,
        Club = 4
    }

    public enum CardColor
    {
        Black = 0,
        Red = 1
    }
}
