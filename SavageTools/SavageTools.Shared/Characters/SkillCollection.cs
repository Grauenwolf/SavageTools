using System.Linq;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class SkillCollection : ChangeTrackingModelCollection<Skill>
    {
        public void AddSkill(string name, string attribute)
        {
            var skill = this.SingleOrDefault(s => s.Name == name);
            if (skill != null)
                skill.Trait += 1;
            else
                Add(new Skill(name, attribute) { Trait = 4 });
        }

        public Skill this[string name] => this.FirstOrDefault(s => s.Name == name);

    }

}

