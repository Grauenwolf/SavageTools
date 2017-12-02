using System.Linq;

namespace SavageTools.Names
{
    public class RandomPerson
    {
        internal RandomPerson(string first, string last, string gender)
        {
            FirstName = first;
            LastName = last;
            Gender = gender;
        }

        public string FirstName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string Gender { get; set; }
        public string LastName { get; set; }
    }
}



