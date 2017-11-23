using SavageTools.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace SavageTools.Web.Controllers
{
    [RoutePrefix("SettingApi")]
    public class SettingApiController : ApiController
    {

        [HttpGet]
        [Route("Archetypes")]
        public IReadOnlyList<NamedItem> Archetypes(string setting)
        {
            return Globals.GetGeneratorForSetting(setting).Archetypes.Select(a => new NamedItem(a.Name)).ToList();
        }

        [HttpGet]
        [Route("Ranks")]
        public IReadOnlyList<NamedItem> Ranks(string setting)
        {
            return Globals.GetGeneratorForSetting(setting).Ranks.Select(a => new NamedItem(a.Name)).ToList();
        }

        [HttpGet]
        [Route("Races")]
        public IReadOnlyList<NamedItem> Races(string setting)
        {
            return Globals.GetGeneratorForSetting(setting).Races.Select(a => new NamedItem(a.Name)).ToList();
        }



    }
}