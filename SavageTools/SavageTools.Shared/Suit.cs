
using System;

namespace SavageTools
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags]
    public enum Suit
    {
        Black = 0,
        Red = 1,

        Spade = 2,
        Heart = 3,
        Diamond = 5,
        Club = 4
    }
}


