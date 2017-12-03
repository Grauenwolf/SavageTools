using System.Collections.Generic;
using System.Linq;

namespace SavageTools.Characters
{
    public class RiftsDemonGenerator
    {
        private readonly CharacterGenerator m_Generator;

        public RiftsDemonGenerator(CharacterGenerator generator)
        {
            m_Generator = generator;
        }

        enum Size
        {
            Gargantuan,
            Huge,
            Large,
            Normal,
            Small,
            Swarm
        }

        List<Character> SingleEntity(Dice dice)
        {
            var result = new Character();

            result.Name = "It Came From the Rift";

            int sizeRoll = dice.D(6);
            Size size;

            if (sizeRoll == 1)
            {
                result.Features.Add("Gargantuan");
                size = Size.Gargantuan; result.Size = dice.D(4) + 8;
            }
            else if (sizeRoll == 2)
            {
                result.Features.Add("Huge");
                size = Size.Huge; result.Size = 8;
            }
            else if (sizeRoll == 3)
            {
                result.Features.Add("Large");
                size = Size.Large; result.Size = dice.D(4) + 3;
            }
            else if (sizeRoll <= 4)
            {
                size = Size.Normal; result.Size = dice.D(4) + -1;
            }
            else
            {
                result.Features.Add("Small");

                if (dice.D(6) <= 4) result.Size = -1;
                else result.Size = -2;
                size = Size.Small;
            }

            RollAttributes(result, dice, size);

            result.Features.Add(Nature(dice));

            var appearanceRoll = dice.D(8);
            if (appearanceRoll == 1)
            {
                result.Features.Add("Horrifically ugly and terrifying");
                result.Fear = -2;
            }
            else if (appearanceRoll == 2)
            {
                result.Features.Add("Bizarre, alien, and disturbing");
                result.Fear = 0;
            }
            else if (appearanceRoll == 3)
            {
                result.Features.Add("Odd, somewhat alien, off-putting");
                result.Charisma -= 2;
            }
            else if (appearanceRoll <= 6)
            {
                result.Features.Add("Humanoid and relatively normal");
            }
            else if (appearanceRoll == 7)
            {
                result.Features.Add("Attractive, alluring, captivating");
                result.Charisma += 2;
            }
            else
            {
                result.Features.Add("Unearthly beautiful and mesmerizing");
                result.Charisma += 4;
            }

            Role(result, dice);

            var abilities = dice.D(4) + 1;
            for (int i = 0; i < abilities; i++)
                SpecialAbilities(result, dice);

            return new List<Character> { result };
        }

        void RollAttributes(Character result, Dice dice, Size size)
        {
            result.Agility = RollAttribute(dice);
            result.Smarts = RollAttribute(dice);
            result.Spirit = RollAttribute(dice);

            var vigorMod = 0;
            if (size == Size.Gargantuan) vigorMod = 6;
            if (size == Size.Huge) vigorMod = 4;
            if (size == Size.Large) vigorMod = 2;

            result.Vigor = RollAttribute(dice, vigorMod);

            switch (size)
            {
                case Size.Gargantuan: result.Strength = new Trait("d12+" + dice.D(2, 8)); break;
                case Size.Huge: result.Strength = new Trait("d12+" + dice.D(2, 6)); break;
                case Size.Large: result.Strength = new Trait("d12+" + dice.D(1, 6)); break;
                default: result.Strength = RollAttribute(dice, vigorMod); break;
            }

        }

        void SpecialAbilities(Character result, Dice dice)
        {
            var roll = dice.D(20);

            if (roll == 1) m_Generator.ApplyEdge(result, dice, "Aquatic");
            else if (roll == 2)
            {
                result.Features.Add("Natrual Armor");
                result.Armor += dice.D(4) + 1;
            }
            else if (roll == 3) result.Features.Add("Burrowing 12\"");
            else if (roll == 4) m_Generator.ApplyEdge(result, dice, "Construct");
            else if (roll == 5)
            {
                var roll2 = dice.D(4);
                if (roll2 == 1) result.Features.Add("Elemental (Air)");
                else if (roll2 == 2) result.Features.Add("Elemental (Earth)");
                else if (roll2 == 3) result.Features.Add("Elemental (Fire)");
                else result.Features.Add("Elemental (Water)");
            }
            else if (roll == 6)
            {
                var roll2 = dice.D(6);
                if (roll2 <= 4) result.Features.Add("Ethereal (at will)");
                else result.Features.Add("Ethereal (permanent)");
            }
            else if (roll == 7) m_Generator.ApplyEdge(result, dice, "Fearless");
            else if (roll == 8) result.Features.Add("Flight 12\", Climb 0");
            else if (roll == 9) m_Generator.ApplyEdge(result, dice, "Hardy");
            else if (roll == 10) m_Generator.ApplyEdge(result, dice, "Infravision");
            else if (roll == 11) result.Edges.Add("Immunity", "Choose one or more conditions");
            else if (roll == 12) result.Edges.Add("Invulnerability", "Must choose at least one Weakness");
            else if (roll == 13) m_Generator.ApplyEdge(result, dice, "Low Light Vision");
            else if (roll == 14) m_Generator.ApplyEdge(result, dice, "Paralysis");
            else if (roll == 15) result.Edges.Add("Poison", "Choose one from the Hazards section of Savage Worlds");
            else if (roll == 16)
            {
                var roll2 = dice.D(6);
                if (roll2 <= 4) m_Generator.ApplyEdge(result, dice, "Slow Regeneration");
                else m_Generator.ApplyEdge(result, dice, "Fast Regeneration");
            }
            else if (roll == 17) m_Generator.ApplyEdge(result, dice, "Stun");
            else if (roll == 18) m_Generator.ApplyEdge(result, dice, "Undead");
            else if (roll == 19) m_Generator.ApplyEdge(result, dice, "Wall Walker");
            else m_Generator.PickEdge(result, dice, bornAHero: true);

        }

        void MindlessIntruder(Character result, Dice dice)
        {
            result.Personality.Add("Mindless Intruder");
            result.Smarts = 4;
            result.Skills.AddSkill("Fighting", "Agility", result.Agility);
            result.Skills.AddSkill("Fighting", "Agility", 6); //sets a min score
            result.Skills.AddSkill("Notice", "Smarts", 6);

            m_Generator.ApplyEdge(result, dice, "Frenzy");
            m_Generator.ApplyEdge(result, dice, "Sweep");

            result.Gear.Add("Natural ranged attack", "Range 20/40/80, Damage 3d8, RoF 1, AP 2, Mega Damage");

            var abilities = dice.D(4) + 1;
            for (int i = 0; i < abilities; i++)
                SpecialAbilities(result, dice);
        }
        void Commander(Character result, Dice dice)
        {
            result.Skills.AddSkill("Fighting", "Agility", 8);
            result.Skills.AddSkill("Knowledge (Battle)", "Smarts", 8);
            result.Skills.AddSkill("Notice", "Smarts", 8);
            result.Skills.AddSkill("Shooting", "Agility", 8);

            m_Generator.ApplyEdge(result, dice, "Command");
            m_Generator.ApplyEdge(result, dice, "Command Presence");
            m_Generator.ApplyEdge(result, dice, "Hold the Line!");

        }
        void Explorer(Character result, Dice dice)
        {
            result.Personality.Add("Explorer");
            if (result.Smarts < 8) result.Smarts = 8;
            result.Skills.AddSkill("Climbing", "Strength", 6);
            result.Skills.AddSkill("Fighting", "Agility", 6);
            result.Skills.AddSkill("Healing", "Smarts", 6);
            result.Skills.AddSkill("Notice", "Smarts", 8);
            result.Skills.AddSkill("Shooting", "Agility", 6);
            result.Skills.AddSkill("Stealth", "Agility", 6);
            result.Skills.AddSkill("Survival", "Smarts", 8);
            result.Skills.AddSkill("Tracking", "Smarts", 8);

            m_Generator.ApplyEdge(result, dice, "Woodsman");

        }

        void Psionic(Character result, Dice dice)
        {
            result.Personality.Add("Psionic");
            if (result.Smarts < 8) result.Smarts = 8;
            if (result.Spirit < 8) result.Spirit = 8;
            result.Skills.AddSkill("Fighting", "Agility", 6);
            result.Skills.AddSkill("Notice", "Smarts", 8);
            result.Skills.AddSkill("Psionics", "Smarts", result.Smarts);

            m_Generator.ApplyEdge(result, dice, "Arcane Background (Psionics)");
            m_Generator.ApplyEdge(result, dice, "Energy Control (Electricity)");
            m_Generator.ApplyEdge(result, dice, "Major Psionic");
            m_Generator.ApplyEdge(result, dice, "Master Psionic");

            m_Generator.AddPower(result, "Psionics", "Armor", dice);
            m_Generator.AddPower(result, "Psionics", "Detect / Conceal Arcana", dice);
            m_Generator.AddPower(result, "Psionics", "Mind Reading", dice);
            m_Generator.AddPower(result, "Psionics", "Puppet", dice);
            m_Generator.AddPower(result, "Psionics", "Speak Languages", dice);
            m_Generator.AddPower(result, "Psionics", "Telekinesis", dice);
            m_Generator.AddPower(result, "Psionics", "Telepathy", dice);

            result.PowerGroups["Psionics"].PowerPoints = 30;

        }

        void ScientistScholar(Character result, Dice dice)
        {
            result.Personality.Add("Scientist/Scholar");
            if (result.Smarts < 8) result.Smarts = 8;
            result.Skills.AddSkill("Healing", "Smarts", result.Smarts);

            var skills = m_Generator.KnowledgeSkills().ToList();
            for (var i = 0; i < 3; i++)
            {
                var skill = dice.Pick(skills);
                result.Skills.AddSkill(skill.Name, skill.Attribute);
            }

            result.Skills.AddSkill("Notice", "Smarts", result.Smarts);
            result.Skills.AddSkill("Repair", "Smarts", result.Smarts);
            result.Skills.AddSkill("Survival", "Smarts", 6);

            m_Generator.ApplyEdge(result, dice, "Scholar");
        }

        void Spellcaster(Character result, Dice dice)
        {
            result.Personality.Add("Spellcaster");
            if (result.Smarts < 8) result.Smarts = 8;
            if (result.Spirit < 8) result.Spirit = 8;
            result.Skills.AddSkill("Fighting", "Agility", 6);
            result.Skills.AddSkill("Notice", "Smarts", 8);
            result.Skills.AddSkill("Spellcasting", "Smarts", result.Smarts);

            m_Generator.ApplyEdge(result, dice, "Arcane Background (Magic)");
            m_Generator.ApplyEdge(result, dice, "Master of Magic");
            m_Generator.ApplyEdge(result, dice, "Wizard");

            m_Generator.AddPower(result, "Magic", "Armor", dice);
            m_Generator.AddPower(result, "Magic", "Bolt", dice);
            m_Generator.AddPower(result, "Magic", "Detect / Conceal Arcana", dice);
            m_Generator.AddPower(result, "Magic", "Dispel", dice);
            m_Generator.AddPower(result, "Magic", "Invisibility", dice);
            m_Generator.AddPower(result, "Magic", "Slumber", dice);
            m_Generator.AddPower(result, "Magic", "Speak languages", dice);

            result.PowerGroups["Magic"].PowerPoints = 30;
        }

        void Trader(Character result, Dice dice)
        {
            result.Personality.Add("Trader");
            if (result.Smarts < 6) result.Smarts = 6;
            if (result.Spirit < 8) result.Spirit = 8;
            result.Skills.AddSkill("Gambling", "Smarts", result.Smarts);
            result.Skills.AddSkill("Persuasion", "Spirit", result.Spirit);
            result.Skills.AddSkill("Streetwise", "Smarts", result.Smarts);

            m_Generator.ApplyEdge(result, dice, "Charismatic");
        }
        void Warrior(Character result, Dice dice)
        {
            result.Personality.Add("Warrior");
            if (result.Agility < 8) result.Agility = 8;
            if (result.Strength < 8) result.Strength = 8;
            if (result.Vigor < 8) result.Vigor = 8;
            result.Skills.AddSkill("Fighting", "Agility", result.Agility);
            result.Skills.AddSkill("Healing", "Smarts", 6);
            result.Skills.AddSkill("Shooting", "Agility", result.Agility);
            result.Skills.AddSkill("Survival", "Smarts", 6);
            result.Skills.AddSkill("Throwing", "Agility", 6);

            m_Generator.ApplyEdge(result, dice, "Frenzy");
            m_Generator.ApplyEdge(result, dice, "Marksman");
        }

        void Role(Character result, Dice dice)
        {
            var roll = dice.D(12);

            if (roll <= 2) MindlessIntruder(result, dice);
            else if (roll <= 4) Commander(result, dice);
            else if (roll <= 5) Explorer(result, dice);
            else if (roll <= 6) Psionic(result, dice);
            else if (roll <= 7) ScientistScholar(result, dice);
            else if (roll <= 8) Spellcaster(result, dice);
            else if (roll <= 9) Trader(result, dice);
            else Warrior(result, dice);
        }

        Trait RollAttribute(Dice dice, int modifier = 0)
        {
            var roll = dice.D(12) + modifier;
            if (roll == 1) return new Trait("d4");
            else if (roll <= 5) return new Trait("d6");
            else if (roll <= 8) return new Trait("d8");
            else if (roll <= 9) return new Trait("d10");
            else if (roll <= 10) return new Trait("d12");

            return new Trait("d12+" + (roll - 10));
        }

        private static string Nature(Dice dice)
        {
            string nature;
            var natureRoll = dice.D(3);
            if (natureRoll == 1) nature = "Natural";
            else if (natureRoll == 2) nature = "Magical";
            else nature = "Technological";
            return nature;
        }

        List<Character> SmallGroup(Dice dice)
        {
            return null;

        }
        List<Character> LargeGroup(Dice dice)
        {

            return null;
        }
        List<Character> Horde(Dice dice)
        {
            return null;
        }

        public List<Character> GenerateCharacter(Dice dice = null)
        {
            dice = dice ?? new Dice();

            List<Character> result;

            if (dice.D(6) <= 2) result = SingleEntity(dice);
            else if (dice.D(6) <= 4) result = SmallGroup(dice);
            else if (dice.D(6) == 5) result = LargeGroup(dice);
            else result = Horde(dice);

            return result;
        }
    }
}
