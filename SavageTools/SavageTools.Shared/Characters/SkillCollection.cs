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
                Add(new Skill() { Name = name, Attribute = attribute, Trait = 4 });
        }

    }

}

