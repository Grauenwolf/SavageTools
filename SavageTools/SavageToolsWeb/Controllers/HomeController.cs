using SavageTools.Characters;
using SavageTools.Missions;
using SavageTools.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SavageTools.Web.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View(new HomeIndexViewModel());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RiftsMission(int pace = 6, int eventFrequency = 3, string type = "mission")
        {
            var generator = new RiftsMissionGenerator(new RiftsDemonGenerator(Globals.GetCharacterGeneratorForSetting("Rifts")));
            var settings = new MissionOptions() { Pace = pace, UseHtml = true, EventFrequency = eventFrequency };

            string result = null;
            switch (type.ToLowerInvariant())
            {
                case "mission": result = generator.CreateMission(new Dice(), settings); break;
                case "rift": result = generator.CreateRift(new Dice(), settings); break;
                case "leylinestorm": result = generator.CreateLeyLineStorm(new Dice(), settings); break;
                case "trouble": result = generator.CreateTrouble(new Dice(), settings); break;
            }


            return View("~/Views/Home/Story.cshtml", (object)result);
        }
        public ActionResult RiftsDemon()
        {
            var generator = new RiftsDemonGenerator(Globals.GetCharacterGeneratorForSetting("Rifts"));

            var result = generator.GenerateCharacter() ?? new List<Character>();

            return View("~/Views/Home/Squad.cshtml", result);
        }

        public ActionResult Squad(string setting, string archetype = null, string race = null, string rank = null, int squadCount = 1)
        {
            var generator = Globals.GetCharacterGeneratorForSetting(setting);
            var settings = new CharacterOptions(generator);

            if (string.IsNullOrWhiteSpace(race))
                settings.RandomRace = true;
            else
                settings.SelectedRace = generator.Races.Single(x => string.Equals(x.Name, race, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(rank))
                settings.RandomRank = true;
            else
                settings.SelectedRank = generator.Ranks.Single(x => string.Equals(x.Name, rank, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(archetype))
                settings.RandomArchetype = true;
            else
                settings.SelectedArchetype = generator.Archetypes.Single(x => string.Equals(x.Name, archetype, StringComparison.OrdinalIgnoreCase));

            var models = new List<Character>();
            for (var i = 0; i < squadCount; i++)
                models.Add(settings.GenerateCharacter());

            return View(models);
        }
    }
}