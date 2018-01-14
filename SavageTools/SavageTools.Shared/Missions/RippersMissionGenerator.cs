using System;
using System.Diagnostics.CodeAnalysis;

namespace SavageTools.Missions
{
    public class RippersMissionGenerator
    {

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public string CreateMission(Dice dice, MissionOptions options = null, RipperMissionType missionType = RipperMissionType.Any)
        {
            if (dice == null)
                throw new ArgumentNullException(nameof(dice), $"{nameof(dice)} is null.");

            var story = new StoryBuilder(options);

            if (missionType == RipperMissionType.Any)
                missionType = (RipperMissionType)dice.D(5);

            if (missionType == RipperMissionType.Hunting) Hunting(story, dice);
            if (missionType == RipperMissionType.Investigation) Investigation(story, dice);
            if (missionType == RipperMissionType.Recruitment) Recruitment(story, dice);
            if (missionType == RipperMissionType.Research) Research(story, dice);
            if (missionType == RipperMissionType.Socializing) Socializing(story, dice);


            return story.ToString();
        }

        static void Hunting(StoryBuilder storyBuilder, Dice dice)
        {
            storyBuilder.AppendLine("Hunting");
            using (storyBuilder.Indent())
            {
                {
                    var roll = dice.D(8);
                    if (roll == 1) storyBuilder.AppendLine("Location: Wilderness");
                    else if (roll <= 3) storyBuilder.AppendLine("Location: Remote");
                    else if (roll <= 5) storyBuilder.AppendLine("Location: Rural");
                    else if (roll <= 7) storyBuilder.AppendLine("Location: Urban");
                    else if (roll <= 8) storyBuilder.AppendLine("Location: City");
                }
                {
                    var roll = dice.D(4);
                    if (roll == 1) storyBuilder.AppendLine("Victim: Innocent");
                    if (roll == 2) storyBuilder.AppendLine("Victim: Socialite");
                    if (roll == 3) storyBuilder.AppendLine("Victim: Relative");
                    if (roll == 4) storyBuilder.AppendLine("Victim: Associate");
                }

            }

        }

        static void Investigation(StoryBuilder storyBuilder, Dice dice)
        {

            storyBuilder.AppendLine("Investigation");
            using (storyBuilder.Indent())
            {
                {
                    var roll = dice.D(6);
                    if (roll == 1) storyBuilder.AppendLine("Source: Newspaper Report");
                    if (roll == 2) storyBuilder.AppendLine("Source: Society Gossip");
                    if (roll == 3) storyBuilder.AppendLine("Source: Myth");
                    if (roll == 4) storyBuilder.AppendLine("Source: Criminal Case");
                    if (roll == 5) storyBuilder.AppendLine("Source: Orders");
                    if (roll == 6) storyBuilder.AppendLine("Source: Consulted");
                }
                {
                    var roll = dice.D(6);
                    if (roll == 1) storyBuilder.AppendLine("Problem: Missing Person");
                    if (roll == 2) storyBuilder.AppendLine("Problem: Crime");
                    if (roll == 3) storyBuilder.AppendLine("Problem: Scandal");
                    if (roll == 4) storyBuilder.AppendLine("Problem: Betrayal");
                    if (roll == 5)
                    {
                        storyBuilder.Append("Problem: Mysterious Creature, ");
                        var danger = dice.D(4);
                        if (danger == 1) storyBuilder.AppendLine("Haunting");
                        if (danger == 2) storyBuilder.AppendLine("Cabal Creature");
                        if (danger == 3) storyBuilder.AppendLine("Animal");
                        if (danger == 4) storyBuilder.AppendLine("Unaffiliated Monster");
                    }
                    if (roll == 6) storyBuilder.AppendLine("Problem: Strange Phenomenon");
                }
                {
                    var roll = dice.D(6);
                    if (roll == 1) storyBuilder.AppendLine("Cause: Accident");
                    if (roll == 2) storyBuilder.AppendLine("Cause: Misunderstanding");
                    if (roll == 3) storyBuilder.AppendLine("Cause: Common Criminals");
                    if (roll == 4) storyBuilder.AppendLine("Cause: Rippers Rival");
                    if (roll == 5) storyBuilder.AppendLine("Cause: Secret Society");
                    if (roll == 6) storyBuilder.AppendLine("Cause: Cabal Activity");
                }
                {
                    var roll = dice.D(6);
                    if (roll == 1) storyBuilder.AppendLine("Complication: Bad to Worse");
                    if (roll == 2) storyBuilder.AppendLine("Complication: Ripper Accused");
                    if (roll == 3) storyBuilder.AppendLine("Complication: Unwanted Attention");
                    if (roll == 4) storyBuilder.AppendLine("Complication: Innocents Imperiled");
                    if (roll == 5) storyBuilder.AppendLine("Complication: Disaster Strikes");
                    if (roll == 6) storyBuilder.AppendLine("Complication: Escalation");
                }
            }
        }

        static void Recruitment(StoryBuilder storyBuilder, Dice dice)
        {
            //Task-4: Random character creator
            storyBuilder.AppendLine("Recruitment");
            using (storyBuilder.Indent())
            {
                int sponsorBonus = 0;
                {
                    var roll = dice.D(10);
                    if (roll <= 2) storyBuilder.AppendLine("Person: Scholar");
                    else if (roll <= 3) storyBuilder.AppendLine("Person: Detective");
                    else if (roll <= 4) storyBuilder.AppendLine("Person: Alienist");
                    else if (roll <= 6) storyBuilder.AppendLine("Person: Officer");
                    else if (roll <= 7) storyBuilder.AppendLine("Person: Doctor");
                    else if (roll <= 8) storyBuilder.AppendLine("Person: Scientist");
                    else if (roll <= 10)
                    {
                        storyBuilder.AppendLine("Person: Sponsor");
                        sponsorBonus += 2;
                    }
                }
                {
                    var roll = dice.D(8) + sponsorBonus;
                    if (roll <= 2) storyBuilder.AppendLine("Quality: Sensitive");
                    else if (roll <= 4) storyBuilder.AppendLine("Quality: Tough");
                    else if (roll <= 6)
                    {
                        storyBuilder.Append("Quality: Connected, ");
                        var connections = dice.D(6);
                        if (connections == 1) storyBuilder.AppendLine("Priest");
                        if (connections == 2) storyBuilder.AppendLine("Policeman");
                        if (connections == 3) storyBuilder.AppendLine("Soldier");
                        if (connections == 4) storyBuilder.AppendLine("Aristocrat");
                        if (connections == 5) storyBuilder.AppendLine("Industrialist");
                        if (connections == 6) storyBuilder.AppendLine("Secret Society");


                    }
                    else if (roll <= 8) storyBuilder.AppendLine("Quality: Wealthy");
                }
                {
                    var roll = dice.D(6);
                    if (roll == 1) storyBuilder.AppendLine("Complication: Servant of the Cabal");
                    if (roll == 2) storyBuilder.AppendLine("Complication: Ridicule");
                    if (roll == 3) storyBuilder.AppendLine("Complication: Insanity");
                    if (roll == 4) storyBuilder.AppendLine("Complication: Charlatan");
                    if (roll == 5) storyBuilder.AppendLine("Complication: Attack");
                    if (roll == 6) storyBuilder.AppendLine("Complication: Missing");
                }

            }
        }

        static void Research(StoryBuilder storyBuilder, Dice dice)
        {
            storyBuilder.AppendLine("Research");
            using (storyBuilder.Indent())
            {
                var requirements = 1;
                while (requirements > 0)
                {
                    requirements -= 1;
                    var roll = dice.D(6);
                    if (roll == 1)
                    {
                        //storyBuilder.AppendLine("Requirements: Additional Information");
                        requirements += 2;
                    }
                    if (roll == 2) storyBuilder.AppendLine("Requirements: Better Facilities, -2 if not obtained");
                    if (roll == 3) storyBuilder.AppendLine("Requirements: Scholarly Insight, -2 if not obtained");
                    if (roll == 4) storyBuilder.AppendLine("Requirements: Specialist Knowledge, -2 if not obtained");
                    if (roll == 5) storyBuilder.AppendLine("Requirements: More Time, double the time needed for research");
                    if (roll == 6) storyBuilder.AppendLine("Requirements: Unique Material, -8 if not found.");
                }
                var results = 0;
                {
                    var roll = dice.D(6);
                    if (roll == 1)
                    {
                        storyBuilder.AppendLine("Findings: Interconnectedness of Things");
                        results += 1;
                    }
                    if (roll == 2) storyBuilder.AppendLine("Findings: Dire Warning");
                    if (roll == 3) storyBuilder.AppendLine("Findings: Forbidden Knowledge");
                    if (roll == 4) storyBuilder.AppendLine("Findings: Secret History");
                    if (roll == 5) storyBuilder.AppendLine("Findings: Scientific Discovery");
                    if (roll == 6) storyBuilder.AppendLine("Findings: Magical Lore");
                }
                {
                    var roll = dice.D(6) + results;
                    if (roll == 1) storyBuilder.AppendLine("Results: Disappointment, -4 to roll");
                    if (roll == 2) storyBuilder.AppendLine("Results: New Tangent");
                    if (roll == 3) storyBuilder.AppendLine("Results: Forgotten Wisdom");
                    if (roll == 4) storyBuilder.AppendLine("Results: Deeper Understanding");
                    if (roll == 5) storyBuilder.AppendLine("Results: Revelation");
                    if (roll == 6) storyBuilder.AppendLine("Results: Success, +2 to roll");
                }
            }
        }
        static void Socializing(StoryBuilder storyBuilder, Dice dice)
        {
            storyBuilder.AppendLine("Socializing");
            using (storyBuilder.Indent())
            {
                {
                    var roll = dice.D(12);
                    if (roll == 1) storyBuilder.AppendLine("Event: Sporting");
                    else if (roll <= 3) storyBuilder.AppendLine("Event: At Home");
                    else if (roll <= 6) storyBuilder.AppendLine("Event: Party");
                    else if (roll <= 9) storyBuilder.AppendLine("Event: Club");
                    else if (roll <= 11) storyBuilder.AppendLine("Event: Concert");
                    else if (roll <= 12) storyBuilder.AppendLine("Event: Ball");
                }

                for (var i = 0; i < storyBuilder.Settings.NumberOfCharacters; i++)
                {
                    var roll = dice.D(4);
                    if (roll == 1) storyBuilder.AppendLine("Results: Faux Pas");
                    if (roll == 2) storyBuilder.AppendLine("Results: Status");
                    if (roll == 3) storyBuilder.AppendLine("Results: Crime");
                    if (roll == 4) storyBuilder.AppendLine("Results: Allies");
                    if (roll == 5) storyBuilder.AppendLine("Results: Information");
                    if (roll == 6) storyBuilder.AppendLine("Results: Benefactor");
                }

                {
                    var roll = dice.D(20);
                    if (roll == 1) storyBuilder.AppendLine("Details: Sudden Death");
                    else if (roll <= 2) storyBuilder.AppendLine("Results: Hidden Purpose");
                    else if (roll <= 3) storyBuilder.AppendLine("Results: Villainous Guest");
                    else if (roll <= 5) storyBuilder.AppendLine("Results: Unexpected Offer");
                    else if (roll <= 7) storyBuilder.AppendLine("Results: Amazing Collection");
                    else if (roll <= 9) storyBuilder.AppendLine("Results: Scandal");
                    else if (roll <= 11) storyBuilder.AppendLine("Results: Embarrassing Mishap");
                    else if (roll <= 13) storyBuilder.AppendLine("Results: Secret Admirer");
                    else if (roll <= 15) storyBuilder.AppendLine("Results: Inside Information");
                    else if (roll <= 17) storyBuilder.AppendLine("Results: New Invitation");
                    else if (roll <= 18) storyBuilder.AppendLine("Results: Bad Company");
                    else if (roll <= 19) storyBuilder.AppendLine("Results: Smitten");
                    else if (roll <= 20) storyBuilder.AppendLine("Results: Royalty");
                }
            }
        }
    }
}
