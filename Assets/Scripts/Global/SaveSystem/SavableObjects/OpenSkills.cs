using System.Collections.Generic;
using System.Linq;

namespace Global.SaveSystem.SavableObjects
{
    public class OpenSkills : ISavable
    {
        public List<SkillWithLevel> OpenedSkills = new List<SkillWithLevel>
        {
            new SkillWithLevel
            {
                Id = "BaseMoonDamageSkill",
                Level = 1,
            },
            new SkillWithLevel
            {
                Id = "BaseBloodDamageSkill",
                Level = 1,
            },
            new SkillWithLevel
            {
                Id = "BaseWaterDamageSkill",
                Level = 1,
            },
            new SkillWithLevel
            {
                Id = "BaseSunDamageSkill",
                Level = 1,
            }
        };

        public SkillWithLevel GetSkillWithLevel(string skillId)
        {
            return OpenedSkills.FirstOrDefault(skillWithLevel => skillWithLevel.Id == skillId);
        }
    }
}