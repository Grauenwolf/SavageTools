using System;
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

        [Flags]
        private enum FollowerTypes
        {
            Civilians = 1,
            Commandos = 2,
            MindlessMinions = 4,
            Soldiers = 8,
            Any = -1
        }

        private enum Size
        {
            Gargantuan,
            Huge,
            Large,
            Normal,
            Small,
            Swarm
        }

        public List<Character> GenerateCharacter(Dice dice = null)
        {
            dice = dice ?? new Dice();

            List<Character> result;

            if (dice.D(6) <= 2) result = SingleEntity(dice);
            else if (dice.D(6) <= 4) result = Group(dice, dice.D(2, 6));
            else if (dice.D(6) == 5) result = Group(dice, dice.D(3, 12));
            else result = Horde(dice);

            return result;
        }

        private static void Civilians(Character result)
        {
            result.Skills.Add("Driving", "Agility", 4);
            result.Skills.Add("Fighting", "Agility", 4);
            result.Skills.Add("Notice", "Smarts", 6);
            result.Skills.Add("Shooting", "Agility", 4);
            result.Skills.Add("Stealth", "Agility", 4);
            result.Skills.Add("Survival", "Smarts", 4);
            result.Skills.Add("Athletics", "Agility", 4);

            result.Gear.Add("Basic melee weapon", "Str+d4");
            result.Gear.Add("Basic ranged weapon", "Range 12/24/48, Damage 2d6");
        }

        private static void Commandos(Character result)
        {
            result.Skills.Add("Athletics", "Strength", 6);
            result.Skills.Add("Driving", "Agility", 6);
            result.Skills.Add("Fighting", "Agility", 8);
            result.Skills.Add("Healing", "Smarts", 6);
            result.Skills.Add("Notice", "Smarts", 8);
            result.Skills.Add("Shooting", "Agility", 8);
            result.Skills.Add("Stealth", "Agility", 8);
            result.Skills.Add("Streetwise", "Smarts", 6);
            result.Skills.Add("Survival", "Smarts", 8);
            result.Skills.Add("Tracking", "Smarts", 8);
            result.Gear.Add("Huntsman Armor");
            result.Toughness += 1;
            result.Armor += 5;
            result.Gear.Add("Advanced melee weapon", "Str+d8, AP 2, Mega Damage");
            result.Gear.Add("Advanced ranged weapon", "Range 15/30/60, Damage 3d6, RoF 1, AP 2");
            result.Gear.Add("Advanced heavy ranged weapon", "Range  18/36/72, Damage 3d6 + 2, RoF 1, AP 2, Mega Damage");
        }

        private static void EssentialNature(Character result, Dice dice)
        {
            var natureRoll = dice.D(3);
            if (natureRoll == 1) result.Archetype = "Natural";
            else if (natureRoll == 2) result.Archetype = "Magical";
            else result.Archetype = "Technological";
        }

        private static void GeneralAppearance(Character result, Dice dice)
        {
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
        }

        private static void MindlessMinions(Character result)
        {
            result.Skills.Add("Athletics", "Strength", 4);
            result.Skills.Add("Fighting", "Agility", 8);
            result.Skills.Add("Notice", "Smarts", 6);
            result.Skills.Add("Shooting", "Agility", 6);
            result.Skills.Add("Stealth", "Agility", 6);
            result.Gear.Add("Basic  melee  weapon", "Str+d6");
            result.Gear.Add("Basic  ranged weapon", "Range 12/24/48, Damage 2d8");
            result.Edges.Add("Natural Armor");
            result.Armor += 2;
            result.Edges.Add("Claws/Bite", "Str+d4");
        }

        private static Trait RollAttribute(Dice dice, int modifier = 0)
        {
            var roll = dice.D(12) + modifier;
            if (roll == 1) return new Trait("d4");
            else if (roll <= 5) return new Trait("d6");
            else if (roll <= 8) return new Trait("d8");
            else if (roll <= 9) return new Trait("d10");
            else if (roll <= 10) return new Trait("d12");

            return new Trait("d12+" + (roll - 10));
        }

        private static void RollAttributes(Character result, Dice dice, Size size)
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

        private static void Soldiers(Character result)
        {
            result.Skills.Add("Driving", "Agility", 4);
            result.Skills.Add("Fighting", "Agility", 6);
            result.Skills.Add("Notice", "Smarts", 6);
            result.Skills.Add("Shooting", "Agility", 8);
            result.Skills.Add("Stealth", "Agility", 6);
            result.Skills.Add("Survival", "Smarts", 4);
            result.Skills.Add("Athletics", "Agility", 6);
            result.Gear.Add("Huntsman Armor");
            result.Toughness += 1;
            result.Armor += 5;
            result.Gear.Add("Advanced melee weapon", "Str+d6, AP 2, Mega Damage");
            result.Gear.Add("Advanced ranged weapon", "Range 15/30/60, Damage 3d6, RoF 1, AP 2");
            result.Gear.Add("Grenades (2)", "Range 5/10/20, Damage 3d6, Mega Damage, LBT");
        }

        private FollowerTypes Commander(Character result, Dice dice)
        {
            result.Skills.Add("Fighting", "Agility", 8);
            result.Skills.Add("Knowledge (Battle)", "Smarts", 8);
            result.Skills.Add("Notice", "Smarts", 8);
            result.Skills.Add("Shooting", "Agility", 8);

            m_Generator.ApplyEdge(result, dice, "Command");
            m_Generator.ApplyEdge(result, dice, "Command Presence");
            m_Generator.ApplyEdge(result, dice, "Hold the Line!");

            return FollowerTypes.Commandos | FollowerTypes.Soldiers;
        }

        private FollowerTypes Explorer(Character result, Dice dice)
        {
            result.Personality.Add("Explorer");
            if (result.Smarts < 8) result.Smarts = 8;
            result.Skills.Add("Athletics", "Strength", 6);
            result.Skills.Add("Fighting", "Agility", 6);
            result.Skills.Add("Healing", "Smarts", 6);
            result.Skills.Add("Notice", "Smarts", 8);
            result.Skills.Add("Shooting", "Agility", 6);
            result.Skills.Add("Stealth", "Agility", 6);
            result.Skills.Add("Survival", "Smarts", 8);
            result.Skills.Add("Tracking", "Smarts", 8);

            m_Generator.ApplyEdge(result, dice, "Woodsman");

            return FollowerTypes.Any;
        }

        private void FollowerRole(Character result, Dice dice, FollowerTypes types)
        {
            var options = new List<Action>();

            if (types.HasFlag(FollowerTypes.Civilians))
                options.Add(() => Civilians(result));
            if (types.HasFlag(FollowerTypes.Commandos))
                options.Add(() => Commandos(result));
            if (types.HasFlag(FollowerTypes.MindlessMinions))
                options.Add(() => MindlessMinions(result));
            if (types.HasFlag(FollowerTypes.Soldiers))
                options.Add(() => Soldiers(result));

            if (options.Count == 0)
                FollowerRole(result, dice, FollowerTypes.Any);
            else
                dice.Choose(options).Invoke();
        }

        private List<Character> Group(Dice dice, int quantity)
        {
            var leader = new Character() { Name = "It Came From the Rift" };

            int sizeRoll = dice.D(6);
            Size size;

            if (sizeRoll == 1)
            {
                leader.Features.Add("Huge");
                size = Size.Huge; leader.Size = 8;
            }
            else if (sizeRoll == 2)
            {
                leader.Features.Add("Large");
                size = Size.Large; leader.Size = dice.D(4) + 3;
            }
            else if (sizeRoll <= 5)
            {
                size = Size.Normal; leader.Size = dice.D(4) + -1;
            }
            else
            {
                leader.Features.Add("Small");

                if (dice.D(6) <= 4) leader.Size = -1;
                else leader.Size = -2;
                size = Size.Small;
            }

            RollAttributes(leader, dice, size);
            EssentialNature(leader, dice);
            GeneralAppearance(leader, dice);

            var abilities = dice.D(4) + 1;
            for (int i = 0; i < abilities; i++)
                SpecialAbilities(leader, dice);

            var followers = leader.Clone();
            followers.Name = $"Followers ({quantity})";

            var followerType = Role(leader, dice);

            FollowerRole(leader, dice, followerType);

            return new List<Character> { leader, followers };
        }

        private List<Character> Horde(Dice dice)
        {
            var result = new Character() { Name = "A Horde Came From the Rift" };

            int sizeRoll = dice.D(6);
            Size size;

            if (sizeRoll == 1)
            {
                result.Features.Add("Large");
                size = Size.Large; result.Size = dice.D(4) + 3;
            }
            else if (sizeRoll <= 3)
            {
                size = Size.Normal; result.Size = dice.D(4) + -1;
            }
            else if (sizeRoll <= 5)
            {
                result.Features.Add("Small");

                if (dice.D(6) <= 4) result.Size = -1;
                else result.Size = -2;
                size = Size.Small;
            }
            else
            {
                result.Size = -10;
                size = Size.Swarm;
            }

            RollAttributes(result, dice, size);
            EssentialNature(result, dice);
            GeneralAppearance(result, dice);

            var abilities = dice.D(4) + 1;
            for (int i = 0; i < abilities; i++)
                SpecialAbilities(result, dice);

            return new List<Character> { result };
        }

        private FollowerTypes MindlessIntruder(Character result, Dice dice)
        {
            result.Personality.Add("Mindless Intruder");
            result.Smarts = 4;
            result.Skills.Add("Fighting", "Agility", result.Agility);
            result.Skills.Add("Fighting", "Agility", 6); //sets a min score
            result.Skills.Add("Notice", "Smarts", 6);

            m_Generator.ApplyEdge(result, dice, "Frenzy");
            m_Generator.ApplyEdge(result, dice, "Sweep");

            result.Gear.Add("Natural ranged attack", "Range 20/40/80, Damage 3d8, RoF 1, AP 2, Mega Damage");

            var abilities = dice.D(4) + 1;
            for (int i = 0; i < abilities; i++)
                SpecialAbilities(result, dice);

            return FollowerTypes.MindlessMinions;
        }

        private FollowerTypes Psionic(Character result, Dice dice)
        {
            result.Personality.Add("Psionic");
            if (result.Smarts < 8) result.Smarts = 8;
            if (result.Spirit < 8) result.Spirit = 8;
            result.Skills.Add("Fighting", "Agility", 6);
            result.Skills.Add("Notice", "Smarts", 8);
            result.Skills.Add("Psionics", "Smarts", result.Smarts);

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

            return FollowerTypes.Any;
        }

        private FollowerTypes Role(Character result, Dice dice)
        {
            var roll = dice.D(12);

            if (roll <= 2) return MindlessIntruder(result, dice);
            else if (roll <= 4) return Commander(result, dice);
            else if (roll <= 5) return Explorer(result, dice);
            else if (roll <= 6) return Psionic(result, dice);
            else if (roll <= 7) return ScientistScholar(result, dice);
            else if (roll <= 8) return Spellcaster(result, dice);
            else if (roll <= 9) return Trader(result, dice);
            else return Warrior(result, dice);
        }

        private FollowerTypes ScientistScholar(Character result, Dice dice)
        {
            result.Personality.Add("Scientist/Scholar");
            if (result.Smarts < 8) result.Smarts = 8;
            result.Skills.Add("Healing", "Smarts", result.Smarts);

            var skills = m_Generator.KnowledgeSkills().ToList();
            for (var i = 0; i < 3; i++)
            {
                var skill = dice.Pick(skills);
                result.Skills.Add(skill.Name, skill.Attribute);
            }

            result.Skills.Add("Notice", "Smarts", result.Smarts);
            result.Skills.Add("Repair", "Smarts", result.Smarts);
            result.Skills.Add("Survival", "Smarts", 6);

            m_Generator.ApplyEdge(result, dice, "Scholar");

            return FollowerTypes.Any;
        }

        private List<Character> SingleEntity(Dice dice)
        {
            var result = new Character() { Name = "It Came From the Rift" };

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
            EssentialNature(result, dice);
            GeneralAppearance(result, dice);

            var abilities = dice.D(4) + 1;
            for (int i = 0; i < abilities; i++)
                SpecialAbilities(result, dice);

            Role(result, dice);

            return new List<Character> { result };
        }

        private void SpecialAbilities(Character result, Dice dice)
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

        private FollowerTypes Spellcaster(Character result, Dice dice)
        {
            result.Personality.Add("Spellcaster");
            if (result.Smarts < 8) result.Smarts = 8;
            if (result.Spirit < 8) result.Spirit = 8;
            result.Skills.Add("Fighting", "Agility", 6);
            result.Skills.Add("Notice", "Smarts", 8);
            result.Skills.Add("Spellcasting", "Smarts", result.Smarts);

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
            return FollowerTypes.Any;
        }

        private FollowerTypes Trader(Character result, Dice dice)
        {
            result.Personality.Add("Trader");
            if (result.Smarts < 6) result.Smarts = 6;
            if (result.Spirit < 8) result.Spirit = 8;
            result.Skills.Add("Gambling", "Smarts", result.Smarts);
            result.Skills.Add("Persuasion", "Spirit", result.Spirit);
            result.Skills.Add("Streetwise", "Smarts", result.Smarts);

            m_Generator.ApplyEdge(result, dice, "Charismatic");
            return FollowerTypes.Any;
        }

        private FollowerTypes Warrior(Character result, Dice dice)
        {
            result.Personality.Add("Warrior");
            if (result.Agility < 8) result.Agility = 8;
            if (result.Strength < 8) result.Strength = 8;
            if (result.Vigor < 8) result.Vigor = 8;
            result.Skills.Add("Fighting", "Agility", result.Agility);
            result.Skills.Add("Healing", "Smarts", 6);
            result.Skills.Add("Shooting", "Agility", result.Agility);
            result.Skills.Add("Survival", "Smarts", 6);
            result.Skills.Add("Athletics", "Agility", 6);

            m_Generator.ApplyEdge(result, dice, "Frenzy");
            m_Generator.ApplyEdge(result, dice, "Marksman");
            return FollowerTypes.Any;
        }
    }
}