using System.Collections.Generic;
using System.Linq;

namespace Global.SaveSystem.SavableObjects
{
    public class OpenSkills : ISavable
    {
        public List<SkillWithLevel> OpenedSkills = new()
        {
            new SkillWithLevel
            {
                Id = "AdditionalDamageSkill",
                Level = 1,
            }
        };

        public SkillWithLevel GetSkillWithLevel(string skillId)
        {
            return OpenedSkills.FirstOrDefault(skillWithLevel => skillWithLevel.Id == skillId);
        }
    }
}