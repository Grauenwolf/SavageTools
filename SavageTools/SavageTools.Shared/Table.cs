using System;
using System.Collections.Generic;
using System.Linq;

namespace SavageTools
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class Table<T> : List<KeyValuePair<T, int>> //Using a KeyValuePair because its cheaper than a Tuple
    {
        public void Add(T item, int odds) => Add(new KeyValuePair<T, int>(item, odds));

        public T RandomChoose(Dice dice)
        {
            if (dice == null)
                throw new ArgumentNullException(nameof(dice), $"{nameof(dice)} is null.");

            if (Count == 0)
                throw new InvalidOperationException("The list is empty.");

            var max = this.Sum(option => option.Value);
            var roll = dice.Next(1, max + 1);
            foreach (var option in this)
            {
                roll -= option.Value;
                if (roll <= 0)
                    return option.Key;
            }
            throw new InvalidOperationException("This cannot happen");
        }

        public T RandomPick(Dice dice)
        {
            if (dice == null)
                throw new ArgumentNullException(nameof(dice), $"{nameof(dice)} is null.");

            if (Count == 0)
                throw new InvalidOperationException("The list is empty.");

            var max = this.Sum(option => option.Value);
            var roll = dice.Next(1, max + 1);
            foreach (var option in this)
            {
                roll -= option.Value;
                if (roll <= 0)
                {
                    Remove(option);
                    return option.Key;
                }
            }
            throw new InvalidOperationException("This cannot happen");
        }
    }


}


