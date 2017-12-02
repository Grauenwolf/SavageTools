namespace SavageTools
{
    public class Card
    {
        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }

        public Rank Rank { get; }
        public Suit Suit { get; }
    }
}


