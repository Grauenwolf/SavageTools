
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tortuga.Anchor;

namespace SavageTools
{
    public class Dice : RandomExtended
    {
        ImmutableArray<Card> m_Deck;
        public Dice()
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

        public int D(int count, int die)
        {
            var result = 0;
            for (var i = 0; i < count; i++)
                result += Next(1, die + 1);

            return result;
        }

        public int D(int die) => D(1, die);

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
                    else if (expression == "")
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

        //public T ChooseWithOdds<T>(ICollection<T> list) where T : IHasOdds
        //{
        //    var max = list.Sum(option => option.Odds);
        //    var roll = Next(1, max + 1);
        //    foreach (var option in list)
        //    {
        //        roll -= option.Odds;
        //        if (roll <= 0)
        //            return option;
        //    }
        //    throw new Exception("This cannot happen");
        //}

        //public T ChooseByRoll<T>(ICollection<T> list, string dieCode) where T : ITablePick
        //{
        //    var roll = D(dieCode);

        //    var result = list.SingleOrDefault(x => x.IsMatch(roll));
        //    if (result == null)
        //        throw new Exception($"No entry for a roll of {roll}");
        //    return result;
        //}
    }
}


