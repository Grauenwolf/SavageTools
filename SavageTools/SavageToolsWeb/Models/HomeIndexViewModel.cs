using System.Collections.Generic;
using System.Web.Mvc;

namespace SavageTools.Web.Models
{
    public class HomeIndexViewModel
    {


        public IEnumerable<SelectListItem> SettingList()
        {
            yield return new SelectListItem() { Text = "", Value = "" };

            foreach (var setting in Globals.GetSettingNames())
                yield return new SelectListItem() { Text = setting, Value = setting };

        }
    }
}