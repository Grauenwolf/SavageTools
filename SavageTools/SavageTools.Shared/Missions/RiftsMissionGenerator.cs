using System;

namespace SavageTools
{
    public class RiftsMissionGenerator
    {
        public string CreateMission(Dice dice, MissionGeneratorSettings settings)
        {
            var story = new StoryBuilder(settings);

            var roll = dice.D(20);
            if (roll <= 2)
                Survey(story, dice);
            else if (roll <= 4)
                CommunicationLines(story, dice);
            else if (roll <= 6)
                ArcaneAnomalies(story, dice);
            else if (roll <= 10)
                CommunityOutreach(story, dice);
            else if (roll <= 12)
                EmergencyRelief(story, dice);
            else if (roll <= 14)
                Exploration(story, dice);
            else if (roll <= 17)
                SecurityPatrol(story, dice);
            else if (roll <= 19)
                Interdiction(story, dice);
            else if (roll <= 20)
                MonsterHunting(story, dice);


            return story.ToString();
        }

        public string CreateRift(Dice dice, MissionGeneratorSettings settings)
        {
            var story = new StoryBuilder(settings);
            Rift(story, dice);
            return story.ToString();
        }

        public string CreateLeyLineStorm(Dice dice, MissionGeneratorSettings settings)
        {
            var story = new StoryBuilder(settings);
            LeyLineStorm(story, dice);
            return story.ToString();
        }

        public string CreateTrouble(Dice dice, MissionGeneratorSettings settings)
        {
            var story = new StoryBuilder(settings);
            var riftInvolved = false;
            Trouble(story, dice, ref riftInvolved);
            return story.ToString();
        }

        void ArcaneAnomalies(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Arcane Anomalies");
            using (story.Indent())
            {

                var d1 = Distance(dice);
                story.AppendLine("Distance to site: " + DistanceText(d1));
                TravelMiles(story, dice, d1);

                var roll = dice.D(8);
                if (roll == 1)
                {
                    story.Append("Arcane Artifact: ");
                    var roll2 = dice.D(6);
                    if (roll2 < 3)
                        story.AppendLine("Nothing");
                    else if (roll2 <= 5)
                        story.AppendLine("Cheap TW gadget");
                    else
                        story.AppendLine("Unique and powerful");

                }
                else if (roll == 2)
                {
                    story.Append("Ghosts: ");
                    var roll2 = dice.D(6);
                    if (roll2 < 3)
                        story.AppendLine("Hoax");
                    else if (roll2 <= 5)
                        story.AppendLine("Real");
                    else
                        story.AppendLine("Horror (e..g Neuron Beast or a Witchling)");
                }
                else if (roll == 3)
                {
                    story.AppendLine("Fadetown");
                    using (story.Indent())
                    {
                        story.AppendLine("Roll for current flux effect. (page 66)");
                    }
                }
                else if (roll == 4) story.AppendLine("Nonviolent Entity");
                else if (roll == 5) story.AppendLine("Cache of Mystic Crystals");
                else if (roll == 6)
                {
                    story.AppendLine("Ley Line Examination");
                    using (story.Indent())
                    {
                        LeyLineSize(story, dice);

                        int d2 = dice.D(8);
                        story.AppendLine("Examination: " + d2 + " days. +1 day per failed Knowledge (Arcana) roll");
                        var riftInvolved = false;
                        Delay(story, dice, d2 + 10, ref riftInvolved);

                    }
                }
                else
                {
                    story.AppendLine("A Rift Opens");
                    using (story.Indent())
                    {
                        Rift(story, dice);
                    }
                }

            }
        }

        void CommunicationLines(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Communication Lines");
            using (story.Indent())
            {
                var d1 = Distance(dice);
                story.AppendLine("Distance to site: " + DistanceText(d1));

                TravelMiles(story, dice, d1);

                int d2 = dice.D(8);
                story.AppendLine("Repair duration: " + d2 + " days. +1 day per failed Knowledge (Electronics) or Repair roll");
                var riftInvolved = false;
                Delay(story, dice, d2 + 10, ref riftInvolved);
            }

        }

        void CommunityOutreach(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Community Outreach");
            using (story.Indent())
            {

                var d1 = Distance(dice);
                story.AppendLine("Distance to town: " + DistanceText(d1));
                TravelMiles(story, dice, d1);

                var roll = dice.D(6);
                int d2;
                if (roll <= 4)
                {
                    story.AppendLine("Town was previously visited");
                    d2 = dice.D(6);
                }
                else
                {
                    story.AppendLine("First contact");
                    d2 = dice.D(2, 6);
                }

                var riftInvolved = false;
                Delay(story, dice, d2, ref riftInvolved);

            }
        }

        void TravelDays(StoryBuilder story, Dice dice, int daysRemaining)
        {
            var day = 0;
            using (story.Indent())
            {
                do
                {
                    for (int i = 0; i < story.Settings.EventFrequency && daysRemaining > 0; i++)
                    {
                        day += 1;
                        daysRemaining -= 1;
                    }
                    story.AppendLine("Day " + day);
                    MaybeTrouble(story, dice);

                } while (daysRemaining > 0);
            }
        }

        void Delay(StoryBuilder story, Dice dice, int daysRemaining, ref bool riftInvolved)
        {
            var day = 0;
            using (story.Indent())
            {
                do
                {
                    for (int i = 0; i < story.Settings.EventFrequency && daysRemaining > 0; i++)
                    {
                        day += 1;
                        daysRemaining -= 1;
                    }
                    story.AppendLine("Day " + day);
                    MaybeTrouble(story, dice, ref riftInvolved);

                } while (daysRemaining > 0);
            }
        }

        void DelayAtRift(StoryBuilder story, Dice dice, int daysRemaining)
        {
            var riftInvolved = true;
            Delay(story, dice, daysRemaining, ref riftInvolved);
        }

        decimal Distance(Dice dice, int modifier = 0)
        {
            var roll = dice.D(20) + modifier;
            if (roll <= 4)
                return dice.D(4) * 10;
            else if (roll <= 12)
                return dice.D(2, 6) * 20;
            else if (roll <= 18)
                return dice.D(3, 6) * 50;
            else
                return dice.D(2, 10) * 100;
        }

        string DistanceText(decimal distance)
        {
            return distance.ToString("N0") + " Miles";
        }
        void EmergencyRelief(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Emergency Relief");

            using (story.Indent())
            {

                var d1 = Distance(dice);
                story.AppendLine("Distance to town: " + DistanceText(d1));
                TravelMiles(story, dice, d1);

                var riftInvolved = false;

                var roll = dice.D(20);
                if (roll <= 3)
                {
                    var d2 = dice.D(2, 8);
                    story.AppendLine("Wreckage. Repair duration: " + d2 + " days. +1 day per failed Knowledge(Engineering) or Repair roll");
                    Delay(story, dice, d2 + 10, ref riftInvolved);
                }
                else if (roll <= 5)
                {
                    var d2 = dice.D(6);
                    story.AppendLine("Starvation. Setup supply distribution duration: " + d2 + " days.");
                    Delay(story, dice, d2, ref riftInvolved);
                }
                else if (roll <= 7)
                {
                    var d2 = dice.D(2, 6);
                    story.AppendLine("Disease. Treatment duration: " + d2 + " days. +1 day per failed Knowledge(Medicine) roll.");
                    Delay(story, dice, d2 + 10, ref riftInvolved);
                }
                else if (roll <= 9)
                {
                    story.AppendLine("Political Strife.");
                    Delay(story, dice, 12, ref riftInvolved);
                }
                else if (roll <= 11)
                {
                    var d2 = dice.D(8);
                    story.AppendLine("Dying Livestock. Treatment duration: " + d2 + " days. +1 day per failed Knowledge(Medicine) roll.");
                    Delay(story, dice, d2 + 10, ref riftInvolved);
                    story.AppendLine("Failure triggers starvation mission.");
                }
                else if (roll <= 13)
                {
                    var d2 = dice.D(8);
                    story.AppendLine("Water Shortage. Repair duration: " + d2 + " days. +1 day per failed nowledge(Engineering) or Repair roll.");
                    Delay(story, dice, d2 + 10, ref riftInvolved);
                }
                else if (roll <= 15)
                {
                    var d2 = dice.D(4);
                    story.AppendLine("Slavers. Tracking duration: " + d2 + " days. +1 day per failed Tracking roll.");
                    TravelDays(story, dice, d2 + 10);

                    var d3 = dice.D(6);
                    if (d3 <= 4)
                        story.AppendLine("Slavers are Bandits");
                    else if (d3 <= 5)
                        story.AppendLine("Slavers are Black Market operation");
                    else if (d3 <= 6)
                        story.AppendLine("Slavers are Splugorth Slaver");
                }
                else if (roll <= 16)
                {
                    var d2 = dice.D(8);
                    story.AppendLine("Dying Livestock. Treatment duration: " + d2 + " days. +1 day per failed Survival or Common Knowledge (farming) roll.");
                    Delay(story, dice, d2 + 10, ref riftInvolved);
                    story.AppendLine("Failure triggers starvation mission.");
                }
                else if (roll <= 17)
                {
                    var d3 = dice.D(6);
                    if (d3 <= 4)
                        story.AppendLine("Cult, harmless");
                    else if (d3 <= 5)
                        story.AppendLine("Dark cult, powerless");
                    else if (d3 <= 6)
                        story.AppendLine("Darl cult, powerful. (e.g. a necromancer and her Grim Reaper Cult followers)");
                }
                else if (roll <= 20)
                {
                    story.AppendLine("Monsters");
                }

            }
        }

        void OppositionLeader(StoryBuilder story, Dice dice, int modifier = 0, bool usePrefix = true)
        {
            var roll = dice.D(6) + modifier;
            if (usePrefix)
                story.Append("Leader is ");
            if (roll <= 1)
                story.AppendLine("War Weary");
            else if (roll <= 3)
                story.AppendLine("Noncommittal");
            else if (roll <= 5)
                story.AppendLine("Wary");
            else
                story.AppendLine("Aggressive");
        }

        void OtherAuthorityFigure(StoryBuilder story, Dice dice, int modifier = 0)
        {
            var roll = dice.D(6) + modifier;
            story.Append("Other authority figure is ");
            if (roll <= 1)
                OppositionLeader(story, dice, usePrefix: false);
            else if (roll <= 2)
                story.AppendLine("Greedy");
            else if (roll <= 3)
                story.AppendLine("Ambitious");
            else if (roll <= 4)
                story.AppendLine("Cowardly");
            else if (roll <= 5)
                story.AppendLine("Apathetic");
            else
                story.AppendLine("Sympathetic");
        }

        void Encounter(StoryBuilder story, Dice dice)
        {

            var roll = dice.D(20);

            if (roll == 1)
            {
                ThingsBadEnough(story, dice);
                Encounter(story, dice);
            }
            else if (roll <= 3)
            {
                PlotThickens(story, dice);
                Encounter(story, dice);
            }
            else if (roll <= 5)
            {
                RisksAndRewards(story, dice);
                Encounter(story, dice);
            }
            else if (roll <= 7)
            {
                story.AppendLine("Bandits");

                using (story.Indent())
                {
                    OppositionLeader(story, dice);
                    OtherAuthorityFigure(story, dice);
                }
            }
            else if (roll <= 8)
            {
                story.AppendLine("Simvan Monster Riders");
            }
            else if (roll <= 10)
            {
                story.AppendLine("Coalition Scouting Party");

                using (story.Indent())
                {
                    OppositionLeader(story, dice);
                    OtherAuthorityFigure(story, dice);
                }
            }
            else if (roll <= 11)
            {
                story.AppendLine("Coalition Recon-in-Force");

                using (story.Indent())
                {
                    OppositionLeader(story, dice);
                    OtherAuthorityFigure(story, dice);
                }
            }
            else if (roll <= 12)
            {
                story.AppendLine("Xiticix");
            }
            else if (roll <= 13)
            {
                story.AppendLine("Wandering Monsters");
            }
            else if (roll <= 14)
            {
                story.AppendLine("Brodkil");

                using (story.Indent())
                {
                    OppositionLeader(story, dice, 2);
                }
            }
            else if (roll <= 15)
            {
                story.AppendLine("Black Market Smuggling Operation");

                using (story.Indent())
                {
                    OppositionLeader(story, dice);
                    OtherAuthorityFigure(story, dice);
                }
            }
            else if (roll <= 16)
            {
                story.AppendLine("Daemonix");

                using (story.Indent())
                {
                    OppositionLeader(story, dice, 2);
                }
            }
            else if (roll <= 17)
            {
                story.AppendLine("Grim Reaper Cult");

                using (story.Indent())
                {
                    OppositionLeader(story, dice);
                    OtherAuthorityFigure(story, dice);
                }
            }

            else if (roll <= 18)
            {
                story.AppendLine("Splugorth Slaver");

                using (story.Indent())
                {
                    story.AppendLine("Leader is Aggressive");
                    OtherAuthorityFigure(story, dice);
                }
            }
            else if (roll <= 19)
            {
                story.AppendLine("Wild Vampires");
            }
            else
            {
                ItCameFromTheRift(story, dice);
            }

        }

        void Exploration(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Exploration");

            using (story.Indent())
            {
                var d1 = Distance(dice);
                story.AppendLine("Distance to site: " + DistanceText(d1));

                TravelMiles(story, dice, d1);

                int d2 = dice.D(2, 12);
                story.AppendLine("Survey duration: " + d2 + " days. +1 day per failed survival roll");

                var riftInvolved = false;
                Delay(story, dice, d2 + 10, ref riftInvolved);
                RisksAndRewards(story, dice);
            }

        }

        void Interdiction(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Interdiction");
            using (story.Indent())
            {

                var d1 = Distance(dice);
                story.AppendLine("Distance to threat: " + DistanceText(d1));
                TravelMiles(story, dice, d1);

                Encounter(story, dice);
            }
        }

        void ItCameFromTheRift(StoryBuilder story, Dice dice)
        {
            story.AppendLine("It came from the rift = FINISH ME");
        }

        void LeyLineSize(StoryBuilder story, Dice dice)
        {
            story.Append("Ley line: ");
            var roll = dice.D(8);
            if (roll == 1) story.AppendLine("Tiny: 500 feet wide, 200 feet tall, 1 mile long.");
            else if (roll == 2) story.AppendLine("Small: 2,000 feet wide, 200 feet tall, one mile long.");
            else if (roll == 3) story.AppendLine("Medium: Quarter - mile wide, 500 feet tall, 10 miles long.");
            else if (roll == 4) story.AppendLine("Large: Half - a - mile wide, 1000 feet tall, 50 miles long.");
            else if (roll == 5) story.AppendLine("Very Large: Three - quarters of a mile wide, 2,000 feet tall, 75 miles long.");
            else if (roll == 6) story.AppendLine("Huge: One mile wide, 3000 feet tall, 100 miles long.");
            else if (roll == 7) story.AppendLine("Gargantuan: One - and - a - half miles wide, one mile tall, 250 miles long.");
            else story.AppendLine("Colossal: Two miles wide, three miles tall, 500 miles long.");
        }

        void LeyLineStorm(StoryBuilder story, Dice dice, int? duration = null)
        {
            story.AppendLine("Ley Line Storm");
            using (story.Indent())
            {
                LeyLineSize(story, dice);

                if (duration.HasValue)
                    story.AppendLine("Duration: " + duration + " hours");

                story.Append("Effect: ");

                var roll = dice.D(8);

                if (roll == 1) story.AppendLine("Ley Lightning");
                else if (roll == 2) story.AppendLine("Air Lift");
                else if (roll == 3) story.AppendLine("Rolling Thunder");
                else if (roll == 4) story.AppendLine("Euphoria");
                else if (roll == 5) story.AppendLine("Alien Interdimensional Effluvia");
                else if (roll == 6) story.AppendLine("Dimensional Flux");
                else if (roll == 7) story.AppendLine("Massive Ley Lightning Bolt");
                else Rift(story, dice);

                if (roll != 8)
                {
                    story.Append("Effect on magic: ");
                    var roll2 = dice.D(6);
                    if (roll2 <= 2) story.AppendLine("Negation");
                    else if (roll2 == 3) story.AppendLine("Surge");
                    else if (roll2 == 4) story.AppendLine("Diminish");
                    else if (roll2 == 5) story.AppendLine("Wild");
                    else story.AppendLine("Explosive");
                }
            }
        }

        void MaybeTrouble(StoryBuilder story, Dice dice)
        {
            var riftInvolved = false;
            MaybeTrouble(story, dice, ref riftInvolved);
        }

        void MaybeTrouble(StoryBuilder story, Dice dice, ref bool riftInvolved)
        {
            var card = dice.Card();
            using (story.Indent())
            {
                if (card.Rank >= Rank.Jack)
                {
                    Trouble(story, dice, ref riftInvolved);
                }
                else
                    story.AppendLine("No event");
            }
        }

        void MonsterHunting(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Monster Hunting");
            using (story.Indent())
            {

                var d1 = Distance(dice);
                story.AppendLine("Distance to sighting: " + DistanceText(d1));
                TravelMiles(story, dice, d1);

                Encounter(story, dice);
            }
        }

        void PlotThickens(StoryBuilder story, Dice dice)
        {
            var roll = dice.D(8);
            if (roll <= 1)
                story.AppendLine("An old enemy is involved.");
            else if (roll <= 2)
                story.AppendLine("A former love interest is mixed up in things somehow.");
            else if (roll <= 3)
                story.AppendLine("An ally is working undercover in the situation.");
            else if (roll <= 4)
                story.AppendLine("One of the enemy force members has a secret agenda at work.");
            else if (roll <= 5)
                story.AppendLine("Innocents are directly in the line of fire, held as captives, or are otherwise in danger.");
            else if (roll <= 6)
                story.AppendLine("Someone’s emotional or ethical Hindrance is directly engaged b");
            else if (roll <= 7)
                story.AppendLine("Someone’s social or behavioral Hindrance is directly engaged");
            else
                story.AppendLine("Someone’s physical Hindrance is directly engaged");
        }

        void Rift(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Rift");
            using (story.Indent())
            {
                var useDuration = true;
                var useEra = false;
                var useRiftsEarth = false;
                var useDestination = true;
                var notEarth = false;

                {
                    var roll = dice.D(12);
                    if (roll <= 8) story.AppendLine("Random Rift");
                    else if (roll <= 11) story.AppendLine("Periodic Rift");
                    else
                    {
                        story.AppendLine("Permanent Rift");
                        useDuration = false;
                    }
                }
                {
                    story.Append("Type: ");
                    var roll = dice.D(20);
                    if (roll <= 14) story.AppendLine("Dimensional");
                    else if (roll <= 15)
                    {
                        story.AppendLine("Time");
                        useEra = true;
                        useDestination = false;
                    }
                    else if (roll <= 16)
                    {
                        story.AppendLine("Space/Time");
                        useEra = true;
                    }
                    else if (roll <= 17)
                    {
                        story.AppendLine("Ley Line");
                        useRiftsEarth = true;
                        useDestination = false;
                    }
                    else if (roll <= 18) story.AppendLine("Other Side");
                    else if (roll <= 19) story.AppendLine("Mythic");
                    else story.AppendLine("Ethereal");
                }
                {
                    story.Append("Condition: ");
                    var roll = dice.D(20);
                    if (roll <= 2) story.AppendLine("Blind");
                    else if (roll <= 4) story.AppendLine("Consuming");
                    else if (roll <= 7) story.AppendLine("Diminishing");
                    else if (roll <= 9) story.AppendLine("Easy");
                    else if (roll <= 10) story.AppendLine("Exploding");
                    else if (roll <= 13) story.AppendLine("Partial");
                    else if (roll <= 16)
                    {
                        story.AppendLine("Pulsing");
                        story.Append("Switch rate: ");
                        var roll2 = dice.D(12);

                        if (roll2 == 1) story.AppendLine("Every three seconds");
                        else if (roll2 == 2) story.AppendLine("Every 10 seconds");
                        else if (roll2 == 3) story.AppendLine("Every 30 seconds");
                        else if (roll2 == 4) story.AppendLine("Every minute");
                        else if (roll2 == 5) story.AppendLine("Every five minutes");
                        else if (roll2 == 6) story.AppendLine("Every 10 minutes");
                        else if (roll2 == 7) story.AppendLine("Every 30 minutes");
                        else if (roll2 == 8) story.AppendLine("Every hour");
                        else if (roll2 == 9) story.AppendLine("Every 5 hours");
                        else if (roll2 == 10) story.AppendLine("Every 12 hours");
                        else if (roll2 == 11) story.AppendLine("Every 24 hours");
                        else if (roll2 == 12) story.AppendLine("Never");
                    }
                    else if (roll <= 19) story.AppendLine("Stable");
                    else story.AppendLine("Transparent");
                }
                {
                    story.Append("Size: ");
                    var roll = dice.D(6);
                    if (roll <= 3) story.AppendLine("Small (8–12 feet)");
                    else if (roll <= 5) story.AppendLine("Medium (50–100 feet)");
                    else story.AppendLine("Large (1,000 feet to one mile or more)");
                }
                if (useDuration)
                {
                    story.Append("Duration: ");
                    var roll2 = dice.D(8);

                    if (roll2 == 1) story.AppendLine(dice.D(5, 6) + " seconds");
                    else if (roll2 <= 3) story.AppendLine(dice.D(2, 6) + " minutes");
                    else if (roll2 <= 5) story.AppendLine(dice.D(3, 10) + " minutes");
                    else if (roll2 <= 6) story.AppendLine(dice.D(1, 4) * 20 + " minutes");
                    else if (roll2 <= 7) story.AppendLine(dice.D(1, 4) + " hours");
                    else story.AppendLine(dice.D(2, 12) + " hours");
                }


                if (useDestination && !useRiftsEarth)
                {
                    var roll2 = dice.D(8);

                    if (roll2 == 1)
                    {
                        useRiftsEarth = true;
                    }
                    else
                    {
                        story.Append("Destination: ");
                        if (roll2 <= 3) story.AppendLine("Parallel Earth");
                        else if (roll2 <= 5) story.AppendLine("Earthlike world");
                        else if (roll2 <= 7) story.AppendLine("Alien-but-relatable world");
                        else story.AppendLine("Very alien world");

                        useEra = true;
                        notEarth = true;
                    }
                }

                if (useDestination && useRiftsEarth)
                {
                    story.Append("Destination: ");

                    var roll2 = dice.D(6);

                    if (roll2 == 1) story.AppendLine("A few miles away");
                    else if (roll2 == 2) story.AppendLine("Dozens of miles away");
                    else if (roll2 == 3) story.AppendLine("Hundreds of miles away");
                    else if (roll2 == 4) story.AppendLine("Thousands of miles away");
                    else if (roll2 == 5) story.AppendLine("Another continent");
                    else if (roll2 == 6) story.AppendLine("Somewhere on an ocean");
                }


                if (useEra)
                {
                    story.Append("Era: ");

                    var roll2 = dice.D(12);

                    if (roll2 == 1) story.AppendLine("Prehistoric");
                    else if (roll2 == 2) story.AppendLine("Stone Age");
                    else if (roll2 == 3) story.AppendLine("Bronze / Iron Age");
                    else if (roll2 == 4) story.AppendLine("Roman Empire");
                    else if (roll2 == 5) story.AppendLine("Medieval Era");
                    else if (roll2 == 6) story.AppendLine("Post - Medieval");
                    else if (roll2 == 7) story.AppendLine("Early Industrial");
                    else if (roll2 == 8) story.AppendLine("Machine / Atomic / Space Age");
                    else if (roll2 == 9) story.AppendLine("Information / Digital Age");
                    else if (roll2 == 10) story.AppendLine("Post - 21st Century ");
                    else if (roll2 == 11) story.AppendLine("Post - Human Development");
                    else if (roll2 == 12) story.AppendLine("Interstellar Age");

                }

                if (notEarth)
                {
                    story.Append("Magic: ");

                    var roll2 = dice.D(6);

                    if (roll2 == 1) story.AppendLine("Null magic ");
                    else if (roll2 <= 3) story.AppendLine("Low magic");
                    else if (roll2 <= 5) story.AppendLine("Medium magic");
                    else story.AppendLine("High magic");
                }

                if (notEarth)
                {
                    story.Append("Psionics: ");

                    var roll2 = dice.D(6);

                    if (roll2 == 1) story.AppendLine("Null psionics ");
                    else if (roll2 <= 3) story.AppendLine("Low psionics");
                    else if (roll2 <= 5) story.AppendLine("Medium psionics");
                    else story.AppendLine("High psionics");
                }

                if (notEarth)
                {
                    story.Append("Era: ");

                    var roll2 = dice.D(10);

                    if (roll2 == 1) story.AppendLine("Apocalypse");
                    else if (roll2 == 2) story.AppendLine("World war");
                    else if (roll2 == 3) story.AppendLine("World peace");
                    else if (roll2 == 4) story.AppendLine("Hostile environment");
                    else if (roll2 == 5) story.AppendLine("Supernatural horror");
                    else if (roll2 == 6) story.AppendLine("Superheroes");
                    else if (roll2 == 7) story.AppendLine("Fiction-verse");
                    else if (roll2 == 8) story.AppendLine("Gateways");
                    else if (roll2 == 9) story.AppendLine("Doppelgangers");
                    else if (roll2 == 10) story.AppendLine("Established setting");
                }

            }
        }

        void Contact(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Contact - FINISH ME Page 78");
        }

        void RisksAndRewards(StoryBuilder story, Dice dice)
        {
            var roll = dice.D(6);
            story.Append("Potential reward: ");
            if (roll <= 1)
            {
                story.AppendLine("Valuable ally");
                using (story.Indent())
                {
                    Contact(story, dice);
                }
            }
            else if (roll <= 2)
                story.AppendLine("A cache of weapons and ammunition");
            else if (roll <= 3)
                story.AppendLine("A powerful magical artifact");
            else if (roll <= 4)
                story.AppendLine("A revelation of very important information.");
            else if (roll <= 5)
                story.AppendLine("A useful and safe location");
            else
                story.AppendLine("A large sum of credits");
        }

        void SecurityPatrol(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Security Patrol");

            using (story.Indent())
            {
                var d1 = Distance(dice, -5);
                story.AppendLine("Distance to patrol area: " + DistanceText(d1));

                TravelMiles(story, dice, d1);

                int d2 = dice.D(2, 6);
                story.AppendLine("Patrol duration: " + d2 + " days.");
                var riftInvolved = false;
                Delay(story, dice, d2, ref riftInvolved);
            }

        }

        void Survey(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Survey");
            using (story.Indent())
            {
                var d1 = Distance(dice);
                story.AppendLine("Distance to site: " + DistanceText(d1));

                TravelMiles(story, dice, d1);

                int d2 = dice.D(8);
                story.AppendLine("Survey duration: " + d2 + " days. +1 day per failed survival roll");
                var riftInvolved = false;
                Delay(story, dice, d2 + 10, ref riftInvolved);
            }

        }

        void ThingsBadEnough(StoryBuilder story, Dice dice)
        {
            var roll = dice.D(10);
            if (roll == 1) Rift(story, dice);
            else if (roll == 2) LeyLineSize(story, dice);
            else if (roll == 3) story.AppendLine("Bounty  hunters  arrive");
            else if (roll == 4) story.AppendLine("Someone  is  dealing  with  technical  difficulties.");
            else if (roll == 5) story.AppendLine("The Black Market is involved");
            else if (roll == 6) story.AppendLine("Refugees by the dozens, or even hundreds");
            else if (roll == 7) story.AppendLine("One or more supernatural creatures");
            else if (roll == 8) Encounter(story, dice);
            else if (roll == 9) story.AppendLine("A spy from an otherwise uninvolved faction is in the area.");
            else
            {
                ThingsBadEnough(story, dice);
                ThingsBadEnough(story, dice);
            }
        }
        void TravelMiles(StoryBuilder story, Dice dice, decimal distance)
        {
            var distanceRemaining = distance;
            var day = 0;
            using (story.Indent())
            {
                do
                {
                    //Add 1 to EventFrequency days
                    for (var i = 0; i < story.Settings.EventFrequency && distanceRemaining > 0; i++)
                    {
                        day += 1;
                        distanceRemaining -= story.Settings.DistancePerDay;
                    }

                    if (distanceRemaining < 0)
                        distanceRemaining = 0;

                    story.AppendLine("Day " + day + ", " + DistanceText(Math.Min(distance, day * story.Settings.DistancePerDay)));
                    var riftInvolved = false;
                    MaybeTrouble(story, dice, ref riftInvolved);

                } while (distanceRemaining > 0);

            }
        }

        void Trouble(StoryBuilder story, Dice dice, ref bool riftInvolved)
        {
            var roll = dice.D(20);
            if (roll <= 1)
            {
                if (riftInvolved)
                {
                    story.AppendLine("Lost in a Rift. ");

                    using (story.Indent())
                    {
                        var d1 = Distance(dice);
                        story.AppendLine("Distance to path: " + DistanceText(d1));

                        TravelMiles(story, dice, d1);

                    }


                }
                else
                    story.AppendLine("Lost. Roll Survival -4 daily to find path.");
            }
            else if (roll <= 3)
                story.AppendLine("Refugees");
            else if (roll <= 4)
            {

                var durationRoll = dice.D(6);
                if (durationRoll <= 4)
                    story.AppendLine("Storm: " + dice.D(6) + " hours");
                else if (durationRoll <= 5)
                    story.AppendLine("Storm: " + dice.D(2, 6) + " hours");
                else
                    story.AppendLine("Storm: " + dice.D(6) + " days");
            }
            else if (roll <= 6)
                story.AppendLine("Technical Difficulties");
            else if (roll <= 7)
                story.AppendLine("Disease");
            else if (roll <= 8)
                story.AppendLine("Fire");
            else if (roll <= 9)
            {
                LeyLineStorm(story, dice, dice.D(2, 6) * 10);
            }
            else if (roll <= 19)
                Encounter(story, dice);
            else
            {
                Trouble(story, dice, ref riftInvolved);
                Trouble(story, dice, ref riftInvolved);
            }
        }
    }
}
