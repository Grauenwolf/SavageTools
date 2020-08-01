using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Tortuga.Anchor;

namespace SavageTools
{
    public class Dice : RandomExtended
    {
        ImmutableArray<Card> m_Deck;

        public Dice(int seed) : base(seed) => Setup();

        public Dice() => Setup();

        void Setup()
        {
            var deck = new List<Card>();
            for (var rank = Rank.Two; rank <= Rank.Ace; rank++)
                for (var suit = Suit.Spade; suit <= Suit.Diamond; suit++)
                    deck.Add(new Card(suit, rank));
            deck.Add(new Card(Suit.Red, Rank.Joker));
            deck.Add(new Card(Suit.Black, Rank.Joker));
            m_Deck = deck.ToImmutableArray();
        }

        public Card Card()
        {
            return Choose(m_Deck);
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "D")]
        public int D(int count, int die)
        {
            var result = 0;
            for (var i = 0; i < count; i++)
                result += Next(1, die + 1);

            return result;
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "D")]
        public int D(int die) => D(1, die);

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "D")]
        public int D(string dieCode)
        {
            if (string.IsNullOrWhiteSpace(dieCode))
                return 0;
            try
            {
                var result = 0;
                var array = dieCode.ToUpperInvariant().Replace("-", "+-").Split(new[] { '+' });
                foreach (var expression in array)
                {
                    if (expression.StartsWith("D"))
                    {
                        result += D(1, int.Parse(expression.Substring(1)));
                    }
                    else if (expression.Contains("D"))
                    {
                        var isNegative = 1;
                        var fixedExpression = expression;

                        if (expression.StartsWith("-"))
                        {
                            isNegative = -1;
                            fixedExpression = expression.Substring(1);
                        }

                        var parts = fixedExpression.Split(new[] { 'D' }, StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray();

                        if (parts.Length == 1)
                            result += D(parts[0], 6) * isNegative;
                        else if (parts[1] == 66)
                            result += D66() * isNegative;
                        else
                            result += D(parts[0], parts[1]) * isNegative;
                    }
                    else if (expression.Length == 0)
                    {
                        //skip
                    }
                    else
                    {
                        result += int.Parse(expression);
                    }
                }

                return result;
            }
            catch (FormatException ex)
            {
                throw new ArgumentException(string.Format("Cannot parse '{0}'", dieCode), "dieCode", ex);
            }
        }

        public int D66() => (Next(1, 7) * 10) + Next(1, 7);

        public bool NextBoolean() => Next(0, 2) == 1;
    }
}
