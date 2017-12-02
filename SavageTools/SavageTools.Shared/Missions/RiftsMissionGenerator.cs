using System;

namespace SavageTools
{
    public class RiftsMissionGenerator : MissionGenerator
    {
        public override string CreateMission(Dice dice, MissionGeneratorSettings settings)
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

        void ArcaneAnomalies(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Arcane Anomalies");
            story.Indent();

            var d1 = Distance(dice);
            story.AppendLine("Distance to site: " + DistanceText(d1));
            Travel(story, dice, d1);

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
                    story.AppendLine("Horror (e..g  Neuron Beast or a Witchling)");
            }
            else if (roll == 3)
            {
                story.AppendLine("Fadetown");
                story.Indent();
                story.AppendLine("Roll for current flux effect. (page 66)");
                story.Dedent();
            }
            else if (roll == 4) story.AppendLine("Nonviolent Entity");
            else if (roll == 5) story.AppendLine("Cache of Mystic Crystals");
            else if (roll == 6)
            {
                story.AppendLine("Ley Line Examination");
                story.Indent();
                LeyLineSize(story, dice);

                int d2 = dice.D(8);
                story.AppendLine("Examination: " + d2 + " days. +1 day per failed Knowledge (Arcana) roll");
                var riftInvolved = false;
                Delay(story, dice, d2 + 10, ref riftInvolved);

                story.Dedent();
            }
            else
            {
                story.AppendLine("A Rift Opens");
                story.Indent();
                Rift(story, dice);
                story.Dedent();
            }

            story.Dedent();
        }

        void CommunicationLines(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Communication Lines");
            story.Indent();
            var d1 = Distance(dice);
            story.AppendLine("Distance to site: " + DistanceText(d1));

            Travel(story, dice, d1);

            int d2 = dice.D(8);
            story.AppendLine("Repair duration: " + d2 + " days. +1 day per failed Knowledge (Electronics) or Repair roll");
            var riftInvolved = false;
            Delay(story, dice, d2 + 10, ref riftInvolved);
            story.Dedent();

        }

        void CommunityOutreach(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Community Outreach");
            story.Indent();

            var d1 = Distance(dice);
            story.AppendLine("Distance to town: " + DistanceText(d1));
            Travel(story, dice, d1);

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

            story.Dedent();
        }

        void Delay(StoryBuilder story, Dice dice, int daysRemaining, ref bool riftInvolved)
        {
            var day = 0;
            story.Indent();
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
            story.Dedent();
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

            story.Indent();

            var d1 = Distance(dice);
            story.AppendLine("Distance to town: " + DistanceText(d1));
            Travel(story, dice, d1);

            story.AppendLine("Page 67 - FINISH ME");

            story.Dedent();
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
            else if (roll == 20) ItCameFromTheRift(story, dice);
            else
                story.AppendLine("Encounter #" + roll + " - FINISH ME");
        }

        void Exploration(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Exploration");

            story.Indent();
            var d1 = Distance(dice);
            story.AppendLine("Distance to site: " + DistanceText(d1));

            Travel(story, dice, d1);

            int d2 = dice.D(2, 12);
            story.AppendLine("Survey duration: " + d2 + " days. +1 day per failed survival roll");

            var riftInvolved = false;
            Delay(story, dice, d2 + 10, ref riftInvolved);
            RisksAndRewards(story, dice);
            story.Dedent();

        }

        void Interdiction(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Interdiction");
            story.Indent();

            var d1 = Distance(dice);
            story.AppendLine("Distance to threat: " + DistanceText(d1));
            Travel(story, dice, d1);

            story.AppendLine("Page 66-67 - FINISH ME");

            story.Dedent();
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

        void LeyLineStorm(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Ley Line Storm - FINISH ME");
        }

        void MaybeTrouble(StoryBuilder story, Dice dice, ref bool riftInvolved)
        {
            var card = dice.Card();
            story.Indent();
            if (card.Rank >= Rank.Jack)
            {
                Trouble(story, dice, ref riftInvolved);
            }
            else
                story.AppendLine("No event");
            story.Dedent();
        }

        void MonsterHunting(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Monster Hunting");
            story.Indent();

            var d1 = Distance(dice);
            story.AppendLine("Distance to sighting: " + DistanceText(d1));
            Travel(story, dice, d1);

            story.AppendLine("Page 67 - FINISH ME");

            story.Dedent();
        }

        void PlotThickens(StoryBuilder story, Dice dice)
        {
            story.AppendLine("The plot thickens - FINISH ME");
        }

        void Rift(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Rift - FINISH ME");
        }

        void RisksAndRewards(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Risks and Rewards - FINISH ME");
        }

        void SecurityPatrol(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Security Patrol");

            story.Indent();
            var d1 = Distance(dice, -5);
            story.AppendLine("Distance to patrol area: " + DistanceText(d1));

            Travel(story, dice, d1);

            int d2 = dice.D(2, 6);
            story.AppendLine("Patrol duration: " + d2 + " days.");
            var riftInvolved = false;
            Delay(story, dice, d2, ref riftInvolved);
            story.Dedent();

        }

        void Survey(StoryBuilder story, Dice dice)
        {
            story.AppendLine("Survey");
            story.Indent();
            var d1 = Distance(dice);
            story.AppendLine("Distance to site: " + DistanceText(d1));

            Travel(story, dice, d1);

            int d2 = dice.D(8);
            story.AppendLine("Survey duration: " + d2 + " days. +1 day per failed survival roll");
            var riftInvolved = false;
            Delay(story, dice, d2 + 10, ref riftInvolved);
            story.Dedent();

        }

        void ThingsBadEnough(StoryBuilder story, Dice dice)
        {
            story.AppendLine("If things we're bad enough - FINISH ME");
        }
        private void Travel(StoryBuilder story, Dice dice, decimal distance)
        {
            var distanceRemaining = distance;
            var day = 0;
            story.Indent();
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

            story.Dedent();
        }

        void Trouble(StoryBuilder story, Dice dice, ref bool riftInvolved)
        {
            var roll = dice.D(20);
            if (roll <= 1)
            {
                if (riftInvolved)
                {
                    story.AppendLine("Lost in a Rift. ");

                    story.Indent();
                    var d1 = Distance(dice);
                    story.AppendLine("Distance to path: " + DistanceText(d1));

                    Travel(story, dice, d1);

                    story.Dedent();


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
                LeyLineStorm(story, dice);
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
