using System;
using System.Collections.Generic;
using System.Linq;

namespace SavageTools
{

    public class Table<T> : System.Collections.Generic.List<KeyValuePair<T, int>> //Using a KeyValuePair because its cheaper than a Tuple
    {
        public void Add(T item, int odds) => Add(new KeyValuePair<T, int>(item, odds));

        public T RandomChoose(Dice dice)
        {
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
            throw new Exception("This cannot happen");
        }

        public T RandomPick(Dice dice)
        {
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
            throw new Exception("This cannot happen");
        }
    }


}


