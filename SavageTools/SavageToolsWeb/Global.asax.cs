using SavageTools.Characters;
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
            var characterGenerators = new Dictionary<string, CharacterGenerator>(StringComparer.OrdinalIgnoreCase);

            var root = new DirectoryInfo(AppDataPath);
            foreach (var file in root.GetFiles("*.savage-setting"))
            {
                Setting book;
                // Open document
                using (var stream = file.OpenRead())
                    book = (Setting)SettingXmlSerializer.Deserialize(stream);

                settings.Add(book.Name, file);
                characterGenerators.Add(book.Name, new CharacterGenerator(file));
            }
            s_SettingFiles = settings;
            s_CharacterGenerators = characterGenerators;

        }

        static string s_AppDataPath;

        public static string AppDataPath { get => s_AppDataPath; }

        static Dictionary<string, FileInfo> s_SettingFiles;
        static Dictionary<string, CharacterGenerator> s_CharacterGenerators;

        public static IReadOnlyList<string> GetSettingNames()
        {
            return s_SettingFiles.Keys.ToImmutableArray();
        }

        public static CharacterGenerator GetCharacterGeneratorForSetting(string settingName)
        {
            return s_CharacterGenerators[settingName];
        }



    }
}
