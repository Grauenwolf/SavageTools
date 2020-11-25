using SavageTools.Characters;
using static SavageTools.CardColor;
using static SavageTools.Rank;

namespace SavageTools.Utilities
{
    public class EtuDemonGenerator
    {
        CharacterGenerator m_CharacterGenerator;

        public EtuDemonGenerator(CharacterGenerator characterGenerator)
        {
            m_CharacterGenerator = characterGenerator;
        }

        public Character GenerateDemon(Dice dice)
        {
            var character = m_CharacterGenerator.CreateBlankCharacter();
            character.Race = "Demon";

            character.Spirit = Trait(dice);
            character.Vigor = Trait(dice);
            character.Agility = Trait(dice);
            character.Smarts = Trait(dice);
            character.Strength = Strength(dice);
            character.Fear = Fear(dice);

            //Demon Abilities
            character.Hindrances.Add("Bane", "A character may keep a demon at bay by displaying a holy item.");
            character.Hindrances.Add("Weakness (Cold Iron)", "Demons take +1d6 damage from pure iron weapons.");
            character.Hindrances.Add("Weakness (Holy Water)", "Demons contacted by holy water make a Spirit roll or become Shaken. Wild Card demons areFatigued as well.");
            character.Edges.Add("Immunity", "Demons are immune to poison and disease.");
            character.Edges.Add("Infernal Stamina", "Demons gain a +2 bonus to recover from being Shaken.");
            character.Edges.Add("Resistant to Normal Weapons", "Demons suffer only half - damage from non-magical attacks except for cold iron.");
            character.Edges.Add("Claws", "Damage: Str+d4");

            switch (dice.PickCard().Rank)
            {
                case Rank.Two:
                case Rank.Three:
                case Rank.Four:
                    character.Skills["Fighting"].Trait = 6;
                    character.Skills["Notice"].Trait = 4;
                    character.Skills["Stealth"].Trait = 6;
                    break;

                case Rank.Five:
                case Rank.Six:
                case Rank.Seven:
                    character.Skills["Fighting"].Trait = 6;
                    character.Skills["Notice"].Trait = 6;
                    character.Skills["Stealth"].Trait = 6;
                    character.Skills["Taunt"].Trait = 6;
                    break;

                case Rank.Eight:
                case Rank.Nine:
                case Rank.Ten:
                    character.Skills["Fighting"].Trait = 8;
                    character.Skills["Notice"].Trait = 6;
                    character.Skills["Intimidation"].Trait = 6;
                    character.Skills["Stealth"].Trait = 6;
                    character.Skills["Taunt"].Trait = 6;
                    break;

                case Rank.Jack:
                    character.Skills["Fighting"].Trait = 8;
                    character.Skills["Notice"].Trait = 8;
                    character.Skills["Intimidation"].Trait = 8;
                    character.Skills["Stealth"].Trait = 8;
                    character.Skills["Taunt"].Trait = 8;
                    break;

                case Rank.Queen:
                    character.Skills["Fighting"].Trait = 10;
                    character.Skills["Notice"].Trait = 8;
                    character.Skills["Intimidation"].Trait = 8;
                    character.Skills["Stealth"].Trait = 8;
                    break;

                case Rank.King:
                    character.Skills["Fighting"].Trait = 10;
                    character.Skills["Notice"].Trait = 10;
                    character.Skills["Intimidation"].Trait = 8;
                    character.Skills["Stealth"].Trait = 8;
                    break;

                case Rank.Ace:
                    character.Skills["Fighting"].Trait = 12;
                    character.Skills["Notice"].Trait = 10;
                    character.Skills["Intimidation"].Trait = 10;
                    character.Skills["Stealth"].Trait = 10;
                    break;

                case Rank.Joker:
                    character.Skills["Fighting"].Trait = 14;
                    character.Skills["Notice"].Trait = 10;
                    character.Skills["Intimidation"].Trait = 10;
                    character.Skills["Stealth"].Trait = 10;
                    break;
            }

            //SWADE Conversion
            character.Skills["Athletics"].Trait = character.Skills["Fighting"].Trait;

            var abilities = dice.D(4);
            for (var i = 1; i <= abilities; i++)
                AddSpecialAbilities(dice, character, m_CharacterGenerator);

            m_CharacterGenerator.AddPersonality(character, dice);
            character.Cleanup();
            return character;
        }

        int Fear(Dice dice)
        {
            switch (dice.PickCard().Rank)
            {
                case Rank.Two:
                case Rank.Three:
                case Rank.Four:
                case Rank.Five:
                case Rank.Six:
                case Rank.Seven: return -1;
                case Rank.Eight:
                case Rank.Nine:
                case Rank.Ten:
                case Rank.Jack: return -2;
                case Rank.Queen:
                case Rank.King: return -3;
                case Rank.Ace:
                case Rank.Joker: return -4;
            }
            return 0;
        }

        int Size(Dice dice)
        {
            switch (dice.PickCard().Rank)
            {
                case Rank.Two:
                case Rank.Three:
                case Rank.Four: return -2;
                case Rank.Five:
                case Rank.Six:
                case Rank.Seven: return -1;
                case Rank.Eight:
                case Rank.Nine:
                case Rank.Ten: return 0;
                case Rank.Jack: return 1;
                case Rank.Queen: return 2;
                case Rank.King: return 3;
                case Rank.Ace: return 4;
                case Rank.Joker: return 5;
            }
            return 0;
        }

        Trait Trait(Dice dice)
        {
            switch (dice.PickCard().Rank)
            {
                case Rank.Two:
                case Rank.Three:
                case Rank.Four:
                case Rank.Five:
                case Rank.Six:
                case Rank.Seven: return 4;
                case Rank.Eight:
                case Rank.Nine:
                case Rank.Ten: return 6;
                case Rank.Jack: return 8;
                case Rank.Queen: return 10;
                case Rank.King: return 12;
                case Rank.Ace: return 13;
                case Rank.Joker: return 14;
            }
            return 0;
        }

        Trait Strength(Dice dice)
        {
            switch (dice.PickCard().Rank)
            {
                case Rank.Two:
                case Rank.Three:
                case Rank.Four: return 4;
                case Rank.Five:
                case Rank.Six:
                case Rank.Seven:
                case Rank.Eight:
                case Rank.Nine:
                case Rank.Ten: return 6;
                case Rank.Jack: return 8;
                case Rank.Queen: return 10;
                case Rank.King: return 12;
                case Rank.Ace: return 14;
                case Rank.Joker: return 16;
            }
            return 0;
        }

        void AddSpecialAbilities(Dice dice, Character character, CharacterGenerator generator)
        {
            void AddEdge(string name, string description = null) => generator.ApplyEdge(character, dice, name, description);

            var card = dice.PickCard();

            if (card.Rank == Joker)
                character.Edges.Add("Spellcaster", "The demon knows 1d4 offensive spells such as bolt, blast, or burst.");  //TODO: Add spells
            else if (card.Color == Red)
                switch (card)
                {
                    case (_, Two): character.Armor += 4; break;
                    case (_, Three): character.Armor += 2; break;
                    case (_, Four): AddEdge("Burrowing"); break;
                    case (_, Five): AddEdge("Hardy"); break;
                    case (_, Six): AddEdge("Telekinesis", "Does not require power points."); break;
                    case (_, Seven): AddEdge("Invulnerability to Normal Weapons"); break;
                    case (_, Eight): AddEdge("Paralysis", "Vigor roll if Shaken or wounded to avoid being paralyzed for 2d4 rounds"); break;
                    case (_, Nine): AddEdge("Fast Regeneration"); break;
                    case (_, Ten): AddEdge("Stun"); break;
                    case (_, Jack):
                        character.Edges.RemoveAll(g => g.Name == "Claws");
                        AddEdge("Claws", "Damage: Str+d6");
                        AddEdge("Improved Frenzy");
                        break;

                    case (_, Queen): AddEdge("Flight", "Pace 12, Climb 1"); break;
                    case (_, King): AddEdge("Cone Attack", "Damage 2d8. Agility roll –2 to avoid."); break;
                    case (_, Ace):
                        switch (dice.D(6))
                        {
                            case 1:
                            case 2:
                                character.Features.Add($"Extra {dice.D(4)} arms");
                                break;

                            case 3:
                            case 4:
                                character.Features.Add($"Extra {dice.D(4)} legs"); break;

                            case 5:
                                character.Features.Add($"Extra tail"); break;

                            case 6:
                                character.Features.Add($"Extra head"); break;
                        }
                        break;
                }
            else
                switch (card)
                {
                    case (_, Two): AddEdge("Very Attractive"); break;
                    case (_, Three): AddEdge("Berserk"); break;
                    case (_, Four): AddEdge("Quick"); break;
                    case (_, Five): AddEdge("Dodge"); break;
                    case (_, Six): AddEdge("Fleet-Footed"); break;
                    case (_, Seven): AddEdge("Harder to Kill"); break;
                    case (_, Eight): AddEdge("First Strike"); break;
                    case (_, Nine): AddEdge("Mighty Blow"); break;
                    case (_, Ten): AddEdge("No Mercy"); break;
                    case (_, Jack): AddEdge("Bolts", "Range 2/4/8, Damage 2d6+2, RoF 3."); break;
                    case (_, Queen): AddEdge("Possession"); break;
                    case (_, King): character.Gear.Add("Weapon", "The demon is armed with a wicked weapon of some sort befitting its Strength."); break;
                    case (_, Ace): character.Features.Add("Minions: The demon has 2d4 lesser demons in attendance.Create separately.Each has a single Special Ability."); break;
                }
        }
    }
}
