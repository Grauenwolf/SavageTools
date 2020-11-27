using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Tortuga.Anchor;

namespace SavageTools
{
    public class Dice : RandomExtended
    {
        List<Card> m_Deck = new List<Card>();

        public Dice(int seed) : base(seed)
        {
        }

        public Dice()
        {
        }

        void ShuffleCards()
        {
            var deck = new List<Card>();
            for (var rank = Rank.Two; rank <= Rank.Ace; rank++)
                for (var suit = Suit.Spades; suit <= Suit.Diamonds; suit++)
                    deck.Add(new Card(suit, rank));
            deck.Add(new Card(Suit.RedJ, Rank.Joker));
            deck.Add(new Card(Suit.BlackJ, Rank.Joker));
            m_Deck = deck;
        }

        /// <summary>
        /// Chooses a card from the deck, then put it back.
        /// </summary>
        /// <returns>Card.</returns>
        /// <remarks>If the card deck is empty, shuffle.</remarks>
        public Card ChooseCard()
        {
            if (m_Deck.Count == 0)
                ShuffleCards();
            return Choose(m_Deck);
        }

        /// <summary>
        /// Pick a card from the deck. This card may not be pulled again.
        /// </summary>
        /// <returns>Card.</returns>
        /// <remarks>If the card deck is empty, shuffle.</remarks>
        public Card PickCard()
        {
            if (m_Deck.Count == 0)
                ShuffleCards(); return Pick(m_Deck);
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
