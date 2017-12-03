using Microsoft.VisualStudio.TestTools.UnitTesting;
using SavageTools;
using SavageTools.Missions;
using System;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var story = new RiftsMissionGenerator();
            var settings = new MissionOptions();
            var dice = new Dice();

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(story.CreateMission(dice, settings));
                Console.WriteLine();
            }
        }
    }


}
