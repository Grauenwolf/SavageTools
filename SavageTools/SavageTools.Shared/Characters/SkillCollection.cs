using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class SkillCollection : ChangeTrackingModelCollection<Skill>
    {
        public Skill this[string name] => this.FirstOrDefault(s => s.Name == name);

        public void Add(string name, string attribute)
        {
            var skill = this.SingleOrDefault(s => s.Name == name);
            if (skill != null)
                skill.Trait += 1;
            else
                Add(new Skill(name, attribute) { Trait = 4 });
        }

        public void Add(string name, string attribute, Trait minLevel)
        {
            var skill = this.SingleOrDefault(s => s.Name == name);
            if (skill != null)
            {
                if (skill.Trait < minLevel)
                    skill.Trait = minLevel;
            }
            else
                Add(new Skill(name, attribute) { Trait = minLevel });
        }
    }

}

