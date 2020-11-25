using static SavageTools.CardColor;
using static SavageTools.Rank;
using static SavageTools.Suit;

namespace SavageTools.Utilities
{
    public class EtuAdventureGenerator
    {
        public EtuAdventure GenerateAdventure()
        {
            var dice = new Dice();

            return new EtuAdventure(Who(dice), What(dice), Why(dice), Where(dice), Complications(dice));
        }

        NameDescriptionPair Where(Dice dice)
        {
            var c = dice.PickCard();
            switch (c)
            {
                case (_, Two):
                case (_, Three):
                case (_, Four):
                case (_, Five):
                    switch (c.Suit)
                    {
                        case Spade: return new("Campus", "The incident occurs in the facilities.");
                        case Club: return new("Campus", "The incident occurs in the classrooms.");
                        default: return new("Campus", "The incident occurs in the dorms.");
                    }

                case (_, Six):
                case (_, Seven):
                case (_, Eight):
                    if (c.Color == Red)
                        return new("Pinebox", "The incident occurs at an ongoing business such as the Pizza Barn or Sanctuary Comics.");
                    else
                        return new("Pinebox", "The incident occurs when the business was closed or the building was empty or abandoned.");

                case (Spade, Nine) or (Spade, Ten):
                    return new("Home", "The incident occurs one of the older homes or ranches from Pinebox’s early days.");

                case (Club, Nine) or (Club, Ten):
                    return new("Home", "The incident occurs in an apartment.");

                case (Heart, Nine) or (Heart, Ten):
                    return new("Home", "The incident occurs in a small home.");

                case (Diamond, Nine) or (Diamond, Ten):
                    return new("Home", "The incident occurs in a larger home or mansion.");

                case (_, Jack):
                case (_, Queen):
                case (_, King):
                    return Environs(dice);

                case (Spade, Ace):
                case (Club, Ace):
                    return new("Out of Town", "The incident occurs in a nearby city such as Houston.");

                case (Heart, Ace):
                    return new("Out of Town", "The incident happens across the border in Mexico.");

                case (Diamond, Ace):
                    return new("Out of Town", "The incident occurs in another US town or city outside of Texas.");

                case (_, Joker):
                    return new("Hero’s Home", "The event happens—or will happen—at one of the hero’s homes!");
            }
            return new("", "");
        }

        NameDescriptionPair Who(Dice dice)
        {
            switch (dice.PickCard())
            {
                case (_, Two, Red): return new("Student", "A fraternity or sorority member (and perhaps some companions) are up to no good.");
                case (_, Two): return new("Student", "A classmate (and perhaps some companions) are up to no good.");

                case (_, Three, Red): return new("Faculty", "One of ETU’s professors that a character knows.");
                case (_, Three): return new("Faculty", "One of ETU’s professors that none of the characters know.");

                case (Heart, Four): return new("Townies", "The townies from Blackburn are up to mischief.");
                case (Diamond, Four): return new("Townies", "The townies from Morganville are up to mischief.");
                case (_, Four): return new("Townies", "The locals are up to mischief.");

                case (_, Five, Red): return new("Government", "The trouble comes from the local authorities.");
                case (Club, Five): return new("Government", "The trouble comes from the military.");
                case (Spade, Five): return new("Government", "The trouble comes from a secret government group.");

                case (_, Six, Red): return new("Cult", "The bad guys are students in a weird cult looking to bring about some major change in the world—or destroy it!");
                case (Club, Six): return new("Cult", "The bad guys are townies in a weird cult looking to bring about some major change in the world—or destroy it!");
                case (Spade, Six): return new("Cult", "The bad guys are outsiders in a weird cult looking to bring about some major change in the world—or destroy it!");

                case (_, Seven, Red): return new("Animal", "An animal that may be natural but somehow forced into the mystery by other circumstances");
                case (_, Seven): return new("Animal", "An animal that has have been altered somehow—perhaps by drinking from weird chemicals or as the result of a miscast ritual");

                case (_, Eight): return new("Scientist", "A technically-minded person is messing with forces beyond her control.");

                case (_, Nine): return new("Corporation", "A company (or at least part of it) is the primary actor.");

                case (_, Ten): return new("Criminal/Organized Crime", "A criminal or group of criminals, or gang, are involved.");

                case (_, Jack):
                case (_, Queen):
                case (_, King):
                case (_, Ace):
                    return new("Supernatural", "A creature, ghost, demon, or human with supernatural abilities is on the prowl.", Supernatural(dice));

                case (_, Joker):
                    var result = Who(dice);
                    result.AddLinkedItem(Who(dice));
                    return result;
            }
            return new("", "");
        }

        NameDescriptionPair Environs(Dice dice)
        {
            switch (dice.PickCard().Rank)
            {
                case Two:
                case Three:
                    return new("Devil Pig Swamp", "");

                case Four: return new("Kestrell Lake", "");
                case Five: return new("Lake Greystone", "");
                case Six: return new("Old Mill Creek", "");
                case Seven: return new("Base X", "");
                case Eight: return new("The Burn", "");
                case Nine: return new("Indian Mounds State Park", "");
                case Ten: return new("Wilson Quarry", "");
                case Jack:
                case Queen:
                case King:
                case Ace: return new("The Big Thicket", "");
                case Joker: return new("A new area created by the Dean", "");
            }
            return new("", "");
        }

        NameDescriptionPair DemonGenerator(Dice dice)
        {
            return new("Demon Generator", ""); //TODO
        }

        NameDescriptionPair Supernatural(Dice dice)
        {
            switch (dice.PickCard())
            {
                case (_, Two):
                case (_, Three):
                case (_, Four): return new("Cryptid", "A natural but unproven creature such as a chupacabra, skunk ape, or night panther.");
                case (_, Five): return new("Lycanthrope", "A werewolf or other hybrid.");
                case (_, Six, Red): return new("Witch/Warlock", "The perpetrator is a master of the black arts.");
                case (_, Six): return new("Witch/Warlock", "The perpetrator is apprentice, new recruit, or dabbler in the black arts.");
                case (Heart, Seven): return new("Vampire", "A old or ancient fiend with a network of lessers ");
                case (_, Seven): return new("Vampire", "A bloodsucking fiend or similar parasitic humanoid has surfaced.");
                case (_, Eight, Red): return new("Ghost", "Some unfortunate soul still lingers in our world. It was violent in life and continues its rage in the afterlife.");
                case (_, Eight): return new("Ghost", "Some unfortunate soul still lingers in our world. The ghost was a victim and might only attack if threatened or confused.");
                case (_, Nine): return new("Demon", "A vile creature from the pits of Hell roams the earth.", DemonGenerator(dice));
                case (_, Ten): return new("Anomaly", "A completely unknown creature—perhaps from some other Savage Worlds setting—has somehow ended up in Pinebox.");
                case (_, Jack): return new("Undead", "Some sort of undead (other than a vampire) rises. This could be a pack of zombies, a mummy, a horrible wight, and so on.");
                case (_, Queen): return new("Animal", "A normal animal altered by science or sorcery is behind the event.");
                case (_, King): return new("Xeno", "The horror is extraterrestrial in nature! It could be grays, a hunter from the stars, or even the servant of some otherworldly deity.");
                case (_, Ace): return new("Slough Creature", "Something corrupt has animated the muck and slime of a swampy slough into a human - shaped creature.");
                case (_, Joker):
                    var result = Supernatural(dice);
                    result.AddLinkedItem(Supernatural(dice));
                    return result;
            }
            return new("", "");
        }

        NameDescriptionPair HighStrangeness(Dice dice)
        {
            switch (dice.PickCard())
            {
                case (Club, Two): return new("", "It begins raining rust-colored rain drops.");
                case (Club, Three): return new("", "Water turns to blood.");
                case (Club, Four): return new("", "An animal yelps in pain. Its carcass is found with the head turned backwards.");
                case (Club, Five): return new("", "The sun or moon unexpectedly eclipses. No one else finds it particularly noteworthy.");
                case (Club, Six): return new("", "A cat or dog approaches the characters and lays a small heart at their feet. Moments later it drops dead. A heart is missing from its body.");
                case (Club, Seven): return new("", "A thick fog grows unnaturally and covers the entire town for 1d4 days.");
                case (Club, Eight): return new("", "The wind picks up and the sounds of unintelligible whispered voices are heard in it.");
                case (Club, Nine): return new("", "A strange mist fills a Large Burst Template area centered on the heroes or triggering source.");
                case (Club, Ten): return new("", "Everything in a Large Burst Template area becomes as sticky as molasses: people, plants, objects, even the ground itself.");
                case (Club, Jack): return new("", "Earthworms, grubworms, and maggots begin crawling from a nearby patch of ground.");
                case (Club, Queen): return new("", $"A pet turns feral and vicious for {dice.D("1d10")} rounds.");
                case (Club, King): return new("", "There is a sudden, very loud, thunderclap.");
                case (Club, Ace): return new("", $"Every person in the next public area suddenly develops hiccups lasting {dice.D("1d10")} minutes.");

                case (Heart, Two): return new("", "Somewhere nearby the awful screeching and crunching of a massive car wreck sounds, but no such accident can be found.");
                case (Heart, Three): return new("", "Bark flakes off of a tree, revealing flesh. Exposed to air, the flesh toughens into bark.");
                case (Heart, Four): return new("", "The heroes notice bloody footprints ending right behind them. They can be backtracked only twenty paces before mysteriously appearing as if in mid-stride.");
                case (Heart, Five): return new("", $"Silence reigns as no sound escapes a Large Burst Template area for {dice.D("1d4")} minutes.");
                case (Heart, Six): return new("", "An insect can be heard screaming for help from a spider web.");
                case (Heart, Seven): return new("", "A siren’s song comes from a nearby patch of woods but stops when investigated.");
                case (Heart, Eight): return new("", "The heroes discover tortillas with small holes punched through them all over the ground.");
                case (Heart, Nine): return new("", "All pets and animals flee in terror from the heroes.");
                case (Heart, Ten): return new("", "Shadows cast by the heroes move of their own accord.");
                case (Heart, Jack): return new("", "An unusually large black bird seemingly follows one of the heroes. It continues to do so unless it is shot, killed, or captured.");
                case (Heart, Queen): return new("", $"All cell phones in Golan County suddenly stop working for {dice.D("1d4")} days and no explanation is ever given.");
                case (Heart, King): return new("", "Birds suddenly fill the sky by the thousands, all flying east.");
                case (Heart, Ace): return new("", "One of the heroes vomits uncontrollably, producing a foul, purplish mass that seems to whine and squeal for a few seconds, then “dies.”");

                case (Diamond, Two): return new("", "A series of knocks from interior walls have no visible source.");
                case (Diamond, Three): return new("", "Someone screams for help from a closet, but no one is inside.");
                case (Diamond, Four): return new("", "A nearby mirror splinters and crashes to the ground.");
                case (Diamond, Five): return new("", $"{dice.D("1d6")} mice suddenly appear and scamper in the area looking for a place to hide.");
                case (Diamond, Six): return new("", "All sounds in a Large Burst Template area become “screwed” so they are slow and deep and difficult to understand.");
                case (Diamond, Seven): return new("", "All food or drink sours immediately.");
                case (Diamond, Eight): return new("", "The water draining down a faucet begins to circle in the opposite direction.");
                case (Diamond, Nine): return new("", $"For {dice.D("1d4")} rounds roaches and other creepy crawlies suddenly swarm in a Small Burst emplate.");
                case (Diamond, Ten): return new("", "Something scratches at a door or window, but no cause is ever found.");
                case (Diamond, Jack): return new("", "A bucket of human teeth is found nearby.");
                case (Diamond, Queen): return new("", "A hero seemingly causes any device with a speaker to burst into static at her approach.");
                case (Diamond, King): return new("", "Roll d100. The resulting number becomes a recurring theme for the heroes. It is everywhere they look—doors, phone numbers, ticket numbers, etc.");
                case (Diamond, Ace): return new("", "A homeless person fixates on one of the heroes and begins following her. He is not mean or evil, only very curious and mysteriously drawn to the hero.");

                case (Spade, Two): return new("", "A large owl flies to a nearby window, pecks the glass until it cracks, then flies away.");
                case (Spade, Three): return new("", "An object suddenly falls from a shelf.");
                case (Spade, Four): return new("", "A beverage (beer, wine, water, soda) suddenly boils.");
                case (Spade, Five): return new("", "A beverage suddenly turns to solid ice.");
                case (Spade, Six): return new("", "All glass (including eyeglasses) in a Large Burst Template area suddenly crack.");
                case (Spade, Seven): return new("", "Nearby sinks and toilets turn on or flush.");
                case (Spade, Eight): return new("", "A baby’s cry is heard nearby, but cannot be found.");
                case (Spade, Nine): return new("", $"All clocks stop and cannot be restarted for {dice.D("1d6")} minutes.");
                case (Spade, Ten): return new("", $"The heroes suddenly experience missing time. Roll {dice.D("1d100")} for the number of minutes. They have no memory of what just happened, but all their watches and time pieces are exactly the same and when compared to others they show the indicated time lapse.");
                case (Spade, Jack): return new("", "A hero hears someone making a “Shhh!” noise, but no one did.");
                case (Spade, Queen): return new("", "Characters controlled by the Dean only speak in questions. When this is pointed out to one of them, he screams in agony and blacks out. The phenomena stops afterward.");
                case (Spade, King): return new("", "The heroes experience a moment of déjà vu. End the current scene, then repeat it.");
                case (Spade, Ace): return new("", $"A hero begins having migraine headaches that cause him to suffer a –2 to all rolls for {dice.D("1d6")} hours. However, on a successful Spirit roll, he also has a brief glimpse of the future.");
            }

            //Redraw in the case of a Joker
            return HighStrangeness(dice);
        }

        NameDescriptionPair Complications(Dice dice)
        {
            var card = dice.PickCard();
            if (card.Color == Red)
                switch (card)
                {
                    case (_, Two): return new("Mistaken Identity", "One of the students or the target is the victim of mistaken identity.");
                    case (_, Three): return new("Crime", "The students or the target become victims of a crime or may have to perpetrate one.");
                    case (_, Four): return new("Misdirection", "The students believe the adventure is about one thing, but the problem changes and the gang must adjust. Generate a second complication.", Complications(dice));
                    case (_, Five): return new("Timed Travel/Race", "The student’s time is limited and they must hurry to accomplish their goals.");
                    case (_, Six): return new("Off the Grid", "Someone involved in the case goes missing for a while. Turns out he or she was just busy—but it happens at a very inopportune time.");
                    case (_, Seven): return new("Heartbreak Hotel", "One of the characters with a romantic relationship hits a glitch. The Significant Other might demand more time or be losing interest. If the card was a Heart, he or she is cheating on the hero with someone else.");
                    case (_, Eight): return new("Breakdown", $"A character’s car breaks down and must be repaired. Mom and Dad take care of the expensive fix, but it’s in the shop for { dice.D(6) } days.");
                    case (Heart, Nine): return new("Teacher’s Pet", "One of the heroes (selected at random) gets noticed by one of his key professors. Adds +1 to his next Exam roll");
                    case (Diamond, Nine): return new("Teacher’s Pain", "One of the heroes (selected at random) gets noticed by one of his key professors. The two don’t click and he suffers a –1 penalty for the next Exam roll.");
                    case (_, Ten): return new("Haters Gonna Hate", $"The student gets involved in something ugly—like a protracted flame war on social media, a fight with Mom and Dad, or behaving embarrassingly badly in front of friends or teammates. He suffers a –1 penalty to all Smarts and Spirit rolls, and linked skills, for { dice.D(6) } days.");
                    case (Heart, Jack): return new("Ambush", "The bad guy(s) know the students are coming and lay in wait to ambush them when the time is right. They’re still carrying a grudge from a previous event.");
                    case (Diamond, Jack): return new("Ambush", "The bad guy(s) know the students are coming and lay in wait to ambush them when the time is right. They’re connected to the current mystery.");
                    case (_, Queen): return new("Can You Hear Me Now?", $"Sometime during the adventure, at the worst possible time, mobile phone coverage inexplicably goes out for { dice.D("2d6")} hours.");
                    case (_, King): return new("Betrayal", "Someone the students know betrays their loyalty. Maybe someone they got information from tells on them or informs the police that a bunch of “armed lunatics are off chasing make-believe monsters.” Or perhaps someone inadvertently spills the wrong beans.");
                    case (Heart, Ace): return new("Weather", "A massive thunderstorm strikes.");
                    case (Diamond, Ace): return new("Weather", "A tornado, wildfire, or flood.");
                }
            else
                switch (card)
                {
                    case (_, Two): return new("Pop Quiz", "A big test comes up in one of the student’s classes, determined randomly. Sometime in the next week of classes, he must make a roll as if taking Exams. If he fails, he suffers a –2 penalty to his actual Exams for his shortcomings. If he succeeds, there’s no penalty. If he gets a raise, he gets a +2 bonus on the next Exam.");
                    case (_, Three): return new("Money Trouble", $"When it rains, it pours. Everyone has an unexpected expense of some sort (the player can describe what it is). The expense is equal to $10 × D6, times the character’s wealth level (Poor=×1, Normal=×2, Rich=×4, Filthy Rich=×6).");
                    case (Club, Four): return new("Media Attention", "A nosy reporter with the Raven’s Report smells a story concerning one of the party’s previous deeds.");
                    case (Spade, Four): return new("Media Attention", "A nosy reporter from out of town smells a story concerning one of the party’s previous deeds.");
                    case (Club, Five): return new("The Sniffles", "Is this just seasonal allergies or something more serious? Congestion and cold symptoms");
                    case (Spade, Five): return new("The Sniffles", "Is this just seasonal allergies or something more serious? Digestive tract problems—such as a queasy stomach or loose bowels");
                    case (_, Six): return new("Crime Doesn’t Pay", "The police pick up on something the gang did and start a quiet investigation. They watch the heroes like hungry hawks.");
                    case (_, Seven): return new("Rock Out", "A super popular band a couple of the heroes are hot for comes to town. Of course the big concert happens right in the middle of their investigation, so they really need to hurry things up if they want to make the show.");
                    case (_, Eight): return new("WeirdTV", "A group of students has put together a “ghost hunters” type show. One of them knows about the heroes somehow and wants to accompany them on their next mission–and film it!");
                    case (_, Nine): return new("Escaped Fugitive", "A prisoner or violent offender is loose in the area where the main action takes place. The police are everywhere—and so is the perp!");
                    case (_, Ten): return new("Mom and Dad", "One of the heroes’ parents decide to visit, most likely unexpectedly.");
                    case (_, Jack): return new("Madness Reigns", "Someone who experienced something terrible goes completely insane.");
                    case (_, Queen): return new("Men in Black", "A group of government operatives in black suits, black ties, white shirts, and black hats show up and ask lots of questions.");
                    case (_, King): return new("The Cavalry", "The player characters aren’t the first class of heroes to graduate from ETU. A small group of alumni who went through similar challenges notices what the group has been up to and keeps an eye on them.");
                    case (_, Ace):
                        var result = HighStrangeness(dice);
                        result.AddLinkedItem(HighStrangeness(dice));
                        result.AddLinkedItem(HighStrangeness(dice));
                        return result;

                    case (_, Joker): return new("All Hell Breaks Loose", "Magic or Ritualism gone awry causes a dimensional rift. A number of creatures enter our world and run wild. Most go into hiding quickly, but a few may rage if given the opportunity.");
                }

            return new("", "");
        }

        NameDescriptionPair Victim(Dice dice)
        {
            switch (dice.PickCard())
            {
                case (_, Two):
                case (_, Three):
                case (_, Four): return new("Student", "A fellow ETU student is in peril.");
                case (_, Five, Red): return new("Faculty", "One of character's professors is the victim.");
                case (_, Five): return new("Faculty", "One of ETU’s faculty is the victim.");
                case (_, Six, Red): return new("Townies", "The danger is in a nearby town such as Blackburn or outlying ranches");
                case (_, Six): return new("Townies", "The locals are in danger.");

                case (_, Seven, Red): return new("Government", "A local government official is in danger.");
                case (_, Seven): return new("Government", "A state or federal official is in danger.");
                case (_, Eight): return new("Love Interest", "One of the characters’ love interests (or close friend) is in danger.");
                case (_, Nine): return new("Animal", "Wildlife or domesticated animals are the victims of this particular tale.");
                case (_, Ten): return new("Scientist", "A team of researchers, doctors, or scientists are in danger.");
                case (_, Jack): return new("Corporation", "Someone or some thing has targeted a company and its employees.");
                case (_, Queen): return new("Criminal/Organized Crime", "Bad things are about to happen to bad people. Should the heroes help? Or let it happen?");
                case (_, King): return new("The Heroes", "One of the heroes is the target!");
                case (_, Ace): return new("Paranormal", "The victim is a supernatural creature.", Supernatural(dice));

                case (_, Joker):
                    var result = Supernatural(dice);
                    result.AddLinkedItem(Supernatural(dice));
                    return result;
            }
            return new("", "");
        }

        NameDescriptionPair Why(Dice dice)
        {
            switch (dice.PickCard())
            {
                case (_, Two):
                case (_, Three):
                case (_, Four):
                    return new("Secret", "The antagonists seek forbidden or secret knowledge.");

                case (_, Five):
                case (_, Six):
                case (_, Seven):
                    return new("Power", "The antagonists are after power of some kind.");

                case (_, Eight):
                case (_, Nine):
                    return new("Chaos", "The antagonists seek purely to sew mischief and mayhem.");

                case (_, Ten): return new("Sacrifice", "The bad guys are attempting to cause misery or harm to others to appease some dark power they believe will grant them favors.");
                case (_, Jack): return new("Love", "The antagonists carry out their ill-placed dark deeds for romantic reasons");
                case (_, Queen): return new("Greed", "It’s all about money.");
                case (_, King): return new("Ritual", "The bad guys must perform a deed as part of a ritual or ceremony.");
                case (_, Ace): return new("Accident", "The antagonists didn’t mean for anything to happen.");

                case (_, Joker):
                    var result = Why(dice);
                    result.AddLinkedItem(Why(dice));
                    return result;
            }
            return new("", "");
        }

        NameDescriptionPair What(Dice dice)
        {
            switch (dice.PickCard())
            {
                case (_, Two):
                case (_, Three):
                case (_, Four):
                case (_, Five):
                    return new("Missing person", "Someone is missing.", Victim(dice));

                case (_, Six, Red):
                case (_, Seven, Red):
                case (_, Eight, Red):
                    return new("Death", "Someone has died.", Victim(dice));

                case (_, Six):
                case (_, Seven):
                case (_, Eight):
                    return new("Death", "Someone is going to die.", Victim(dice));

                case (_, Nine):
                case (_, Ten):
                    return new("Accident", "There’s been an accident of some sort, perhaps incidental to the bad guys’ actual plan.", Victim(dice));

                case (_, Jack): return new("Sighting", "Someone saw something strange and tells the students.");

                case (_, Queen, Red):
                    return new("Criminal Activity", "A crime has been commited against...", Victim(dice));

                case (_, Queen):
                    return new("Criminal Activity", "A crime is going to be committed against...", Victim(dice));

                case (_, King, Red):
                case (_, Ace, Red):
                    return new("Theft", "A theft has been commited against...", Victim(dice));

                case (_, King):
                case (_, Ace):
                    return new("Theft", "A theft is going to be commited against...", Victim(dice));

                case (_, Joker): return new("Mass Event", "Something BIG is going to happen that could affect the whole town");
            }
            return new("", "");
        }
    }

    public record EtuAdventure(NameDescriptionPair Who, NameDescriptionPair What, NameDescriptionPair Why, NameDescriptionPair Where, NameDescriptionPair Complications);

    public record NameDescriptionPair(string Name, string Description)
    {
        public NameDescriptionPair(string name, string description, NameDescriptionPair linkedItem)
            : this(name, description)
            => LinkedItem = linkedItem;

        public NameDescriptionPair LinkedItem { get; private set; }

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
