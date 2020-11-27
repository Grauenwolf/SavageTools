namespace SavageTools
{
    public record Card(Suit Suit, Rank Rank)
    {
        public CardColor Color => (CardColor)(Suit & Suit.RedJ);

        public void Deconstruct(out Suit suit, out Rank rank) => (suit, rank) = (Suit, Rank);
        public void Deconstruct(out Suit suit, out Rank rank, out CardColor color) => (suit, rank, color) = (Suit, Rank, Color);
        //public void Deconstruct(out Rank rank, out CardColor color) => (rank, color) = (Rank, Color);

        //public BlackRedCard ToBlackRed => new(Color, Rank);

        public static implicit operator Suit(Card c) => c.Suit;
        public static implicit operator Rank(Card c) => c.Rank;
        public static implicit operator CardColor(Card c) => c.Color;

        public override string ToString()
        {
            if (Rank == Rank.Joker)
                return $"{Color} {Rank}";

            return $"{Rank} of {Suit}";
        }
    }

    //public record BlackRedCard(CardColor color, Rank Rank);
}
