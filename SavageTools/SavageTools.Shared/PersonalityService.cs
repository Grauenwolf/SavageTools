using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace SavageTools.Names
{
    public class PersonalityService
    {

        readonly ImmutableArray<string> m_Personalities;

        public PersonalityService(string dataPath)
        {
            var file = new FileInfo(Path.Combine(dataPath, "personality.txt"));
            m_Personalities = File.ReadAllLines(file.FullName).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToImmutableArray();
        }
        public string CreateRandomPersonality(Dice random)
        {
            return random.Choose(m_Personalities);
        }
    }
}



