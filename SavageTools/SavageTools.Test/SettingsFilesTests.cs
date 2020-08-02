using Microsoft.VisualStudio.TestTools.UnitTesting;
using SavageTools.Characters;
using SavageTools.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SavageTools.Test
{
    [TestClass]
    public class SettingsFilesTests
    {
        static readonly XmlSerializer SettingXmlSerializer = new XmlSerializer(typeof(Setting));
        static readonly DirectoryInfo Root = new DirectoryInfo(@".\Settings");

        [TestMethod]
        public void LoadAllSettingFiles()
        {
            //var settings = new Dictionary<string, FileInfo>(StringComparer.OrdinalIgnoreCase);
            var characterGenerators = new Dictionary<string, CharacterGenerator>(StringComparer.OrdinalIgnoreCase);

            foreach (var file in Root.GetFiles("*.savage-setting"))
            {
                Debug.WriteLine($"Loading {file.Name}");
                Setting book;
                // Open document
                using (var stream = file.OpenRead())
                    book = (Setting)SettingXmlSerializer.Deserialize(stream);

                //if (book.ShowSetting)
                //    settings.Add(book.Name, file);
                characterGenerators.Add(book.Name, new CharacterGenerator(file));
            }
        }

        [DataRow("Core.savage-setting")]
        [TestMethod]
        public void ApplyAllMajorHindrances(string settingName)
        {
            var dice = new Dice(0);
            var file = new FileInfo(Path.Combine(Root.FullName, settingName));
            var characterGenerator = new CharacterGenerator(file);
            var character = new Character()
            {
                Strength = 6,
                Agility = 6,
                Smarts = 6,
                Spirit = 6,
                Vigor = 6
            };

            foreach (var hindrance in characterGenerator.MajorHindrances)
                CharacterGenerator.ApplyHindrance(character, hindrance, 2, dice);

            Debug.WriteLine(character.ToCompactString(false));
        }

        [DataRow("Core.savage-setting")]
        [TestMethod]
        public void ApplyAllMinorHindrances(string settingName)
        {
            var dice = new Dice(0);
            var file = new FileInfo(Path.Combine(Root.FullName, settingName));
            var characterGenerator = new CharacterGenerator(file);
            var character = new Character()
            {
                Strength = 6,
                Agility = 6,
                Smarts = 6,
                Spirit = 6,
                Vigor = 6
            };

            foreach (var hindrance in characterGenerator.MinorHindrances)
                CharacterGenerator.ApplyHindrance(character, hindrance, 1, dice);

            Debug.WriteLine(character.ToCompactString(false));
        }

        [DataRow("Core.savage-setting")]
        [TestMethod]
        public void ApplyAllEdges(string settingName)
        {
            var dice = new Dice(0);
            var file = new FileInfo(Path.Combine(Root.FullName, settingName));
            var characterGenerator = new CharacterGenerator(file);
            var character = new Character()
            {
                Strength = 6,
                Agility = 6,
                Smarts = 6,
                Spirit = 6,
                Vigor = 6
            };

            foreach (var edge in characterGenerator.Edges)
                CharacterGenerator.ApplyEdge(character, edge, dice);

            Debug.WriteLine(character.ToCompactString(false));
        }

        const int Iterations = 100;

        [DataRow("Core.savage-setting", "(None)")]
        [TestMethod]
        public void MakeArchetype(string settingName, string selectedArchetype)
        {
            var dice = new Dice(0);
            var file = new FileInfo(Path.Combine(Root.FullName, settingName));
            var characterGenerator = new CharacterGenerator(file);

            for (var i = 0; i < Iterations; i++)
            {
                Character character = MakeCharacter(characterGenerator, dice, selectedArchetype, null, null);
                Debug.WriteLine(character.ToCompactString(false));
            }
        }

        static Character MakeCharacter(CharacterGenerator characterGenerator, Dice dice,
            string selectedArchetype, string selectedRank, string selectedRace)
        {
            var options = new CharacterOptions(characterGenerator);

            //Archetypes
            if (selectedArchetype == null)
                options.SelectedArchetype = null;
            else
                options.SelectedArchetype = characterGenerator.Archetypes.SingleOrDefault(a => a.Name == selectedArchetype);

            //Ranks
            if (selectedRank == null)
                options.SelectedRank = null;
            else
                options.SelectedRank = characterGenerator.Ranks.SingleOrDefault(a => a.Name == selectedRank);

            //Race
            if (selectedRace == null)
                options.SelectedRace = null;
            else
                options.SelectedRace = characterGenerator.Races.SingleOrDefault(a => a.Name == selectedRace);

            return characterGenerator.GenerateCharacter(options, dice);
        }
    }
}
