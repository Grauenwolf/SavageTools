using SavageTools.Settings;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace SavageTools.Web.Models
{
    public class HomeIndexViewModel
    {
        static readonly XmlSerializer SettingXmlSerializer = new XmlSerializer(typeof(Setting));
        static List<Setting> s_Settings;

        public HomeIndexViewModel()
        {
            if (s_Settings == null)
            {
                var settings = new List<Setting>();

                var root = new System.IO.DirectoryInfo(MvcApplication.AppDataPath);
                foreach (var file in root.GetFiles("*.savage-setting"))
                {
                    Setting book;
                    // Open document
                    using (var stream = file.OpenRead())
                        book = (Setting)SettingXmlSerializer.Deserialize(stream);

                    settings.Add(book);
                }
                s_Settings = settings;
            }
        }

        public IEnumerable<SelectListItem> SettingList()
        {
            //yield return new SelectListItem() { Text = "", Value = "" };
            foreach (var setting in s_Settings)
                yield return new SelectListItem() { Text = setting.Name, Value = setting.Name };

        }

        public IEnumerable<SelectListItem> RankList()
        {
            //yield return new SelectListItem() { Text = "", Value = "" };
            foreach (var setting in s_Settings)
                yield return new SelectListItem() { Text = setting.Name, Value = setting.Name };

        }
    }
}