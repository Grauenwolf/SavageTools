namespace SavageTools.Utilities
{
    public record NameDescriptionPair(Card? Card, string Name, string Description)
    {
        public NameDescriptionPair(Card? card, string name, string description, NameDescriptionPair linkedItem)
            : this(card, name, description)
            => LinkedItem = linkedItem;

        public NameDescriptionPair(string name, string description)
            : this(null, name, description) { }

        public NameDescriptionPair(string name, string description, NameDescriptionPair linkedItem)
            : this(null, name, description)
            => LinkedItem = linkedItem;

        public NameDescriptionPair? LinkedItem { get; private set; }
        public bool IsMarkdown { get; init; }

        public void AddLinkedItem(NameDescriptionPair item)
        {
            var current = this;
            while (current.LinkedItem != null)
                current = current.LinkedItem;
            //Adds the item to the end of the chain;
            current.LinkedItem = item;
        }
    }
}
