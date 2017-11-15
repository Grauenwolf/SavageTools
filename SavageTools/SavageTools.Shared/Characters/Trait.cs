using System;

namespace SavageTools.Characters
{

    public struct Trait : IEquatable<Trait>, IComparable, IComparable<Trait>
    {
        public Trait(int score) => Score = score;

        public Trait(string dieCode)
        {
            dieCode = dieCode.ToUpperInvariant();
            switch (dieCode)
            {
                case "2D": Score = 2; return;
                case "4D": Score = 4; return;
                case "6D": Score = 6; return;
                case "8D": Score = 8; return;
                case "10D": Score = 10; return;
                case "12D": Score = 12; return;
            }
            if (dieCode.StartsWith("12D+"))
            {
                Score = 12 + int.Parse(dieCode.Substring(4));
                return;
            }
            throw new ArgumentException($"Cannot parse die code \"{dieCode}\" as a trait.");

        }

        public int Score { get; }

        public int HalfScore => (int)Math.Floor(Score / 2M);

        public override string ToString()
        {
            if (Score == 0)
                return "";
            if (Score < 4)
                return $"d4{Score - 4 }"; //Example: Score 2 would be 4d-2
            if (Score <= 12)
                return $"d{Score}";
            return $"d12+{Score - 12}";
        }

        public static Trait operator +(Trait original, int bonus)
        {
            //The math is a bit wonky because of the combination of a die code and a bonus 
            var score = original.Score;

            //Adds
            while (bonus > 0)
            {
                if (score < 12)
                    score += 2;
                else
                    score += 1;
                bonus -= 1;
            }
            //Subtracts
            while (bonus < 0)
            {
                if (score > 12)
                    score -= 1;
                else
                    score -= 2;
                bonus += 1;
            }

            return new Trait(score);
        }

        public static Trait operator -(Trait original, int bonus) => original + (bonus * -1);

        public static implicit operator Trait(int score) { return new Trait(score); }

        public static implicit operator Trait(string dieCode) { return new Trait(dieCode); }

        public override bool Equals(object obj)
        {
            if (obj is Trait)
                return Equals((Trait)obj);
            return base.Equals(obj);
        }

        public static bool operator ==(Trait first, Trait second) => first.Equals(second);

        public static bool operator !=(Trait first, Trait second) => !(first == second);

        public bool Equals(Trait other) => Score == other.Score;

        public override int GetHashCode() => Score;

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            var other = obj as Trait?;
            if (other == null)
                throw new ArgumentException(nameof(obj) + " is not a " + nameof(Trait));
            return CompareTo(other.Value);
        }

        public int CompareTo(Trait other)
        {
            if (other == null)
                return 1;
            return Score.CompareTo(other.Score);
        }

        public static bool operator >(Trait first, Trait second) => first.Score > second.Score;
        public static bool operator <(Trait first, Trait second) => first.Score < second.Score;
        public static bool operator >=(Trait first, Trait second) => first.Score >= second.Score;
        public static bool operator <=(Trait first, Trait second) => first.Score <= second.Score;
    }
}

