using SavageTools.Characters;
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

        public ActionResult RiftsMission(string setting, int pace = 6, int eventFrequency = 3)
        {
            var generator = new RiftsMissionGenerator();
            var settings = new MissionGeneratorSettings() { Pace = pace, UseHtml = true , EventFrequency = eventFrequency };
            var mission = generator.CreateMission(new Dice(), settings);

            return View("~/Views/Home/Story.cshtml", (object)mission);
        }

        public ActionResult Squad(string setting, string archetype = null, string race = null, string rank = null, int squadCount = 1)
        {
            var generator = Globals.GetCharacterGeneratorForSetting(setting);
            var dice = new Dice();

            if (string.IsNullOrWhiteSpace(race))
                generator.RandomRace = true;
            else
                generator.SelectedRace = generator.Races.Single(x => string.Equals(x.Name, race, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(rank))
                generator.RandomRank = true;
            else
                generator.SelectedRank = generator.Ranks.Single(x => string.Equals(x.Name, rank, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(archetype))
                generator.RandomArchetype = true;
            else
                generator.SelectedArchetype = generator.Archetypes.Single(x => string.Equals(x.Name, archetype, StringComparison.OrdinalIgnoreCase));

            var models = new List<Character>();
            for (var i = 0; i < squadCount; i++)
                models.Add(generator.GenerateCharacter(dice));

            return View(models);
        }
    }
}