using System;

namespace SavageTools
{
    public abstract class MissionGenerator
    {
        public abstract string CreateMission(Dice dice, MissionGeneratorSettings settings);
    }
}
