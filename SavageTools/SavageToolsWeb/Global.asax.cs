using SavageTools.Settings;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml.Serialization;

namespace SavageTools.Web
{
    public class Globals : HttpApplication
    {
        static readonly XmlSerializer SettingXmlSerializer = new XmlSerializer(typeof(Setting));

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            s_AppDataPath = Server.MapPath("~/bin/Settings");

            var settings = new Dictionary<string, FileInfo>(StringComparer.OrdinalIgnoreCase);

            var root = new DirectoryInfo(AppDataPath);
            foreach (var file in root.GetFiles("*.savage-setting"))
            {
                Setting book;
                // Open document
                using (var stream = file.OpenRead())
                    book = (Setting)SettingXmlSerializer.Deserialize(stream);

                settings.Add(book.Name, file);
            }
            s_SettingFiles = settings;

        }

        static string s_AppDataPath;

        public static string AppDataPath { get => s_AppDataPath; }

        static Dictionary<string, FileInfo> s_SettingFiles;

        public static IReadOnlyList<string> GetSettingNames()
        {
            return s_SettingFiles.Keys.ToImmutableArray();
        }

        public static CharacterGenerator GetCharacterGeneratorForSetting(string settingName)
        {
            //CharacterGenerator isn't threadsafe so we need a new one each time.
            //We can make it thread-safe, but only if we change the Windows version of the UI to match.
            return new CharacterGenerator(s_SettingFiles[settingName]);
        }



    }
}
