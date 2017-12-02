using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace SavageTools.Names
{




    public class LocalNameService
    {
        readonly ImmutableList<string> m_FemaleNames;
        readonly ImmutableList<string> m_LastNames;
        readonly ImmutableList<string> m_MaleNames;

        public LocalNameService(string dataPath, string prefix)
        {
            if (!string.IsNullOrWhiteSpace(prefix) && !prefix.EndsWith("-"))
                prefix += "-";

            var femaleFile = new FileInfo(Path.Combine(dataPath, prefix + "female-first.txt"));
            var lastFile = new FileInfo(Path.Combine(dataPath, prefix + "last.txt"));
            var maleFile = new FileInfo(Path.Combine(dataPath, prefix + "male-first.txt"));

            m_LastNames = File.ReadAllLines(maleFile.FullName).Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Substring(0, 1).ToUpper() + x.Substring(1)).Distinct().ToImmutableList();
            m_FemaleNames = File.ReadAllLines(femaleFile.FullName).Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Substring(0, 1).ToUpper() + x.Substring(1)).Distinct().ToImmutableList();
            m_MaleNames = File.ReadAllLines(maleFile.FullName).Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Substring(0, 1).ToUpper() + x.Substring(1)).Distinct().ToImmutableList();
        }

        public RandomPerson CreateRandomPerson(Dice random)
        {
            var isMale = random.NextBoolean();

            return new RandomPerson(
                 isMale ? random.Choose(m_LastNames) : random.Choose(m_FemaleNames),
                 random.Choose(m_LastNames),
                 isMale ? "M" : "F"
                );
        }
    }
}



