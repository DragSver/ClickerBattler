using System.Collections.Generic;
using System.Linq;

namespace Global.SaveSystem.SavableObjects
{
    public class OpenSkills : ISavable
    {
        public List<SkillWithLevel> OpenedSkills;

        public SkillWithLevel GetSkillWithLevel(string skillId)
        {
            if (OpenedSkills is null || OpenedSkills.Count == 0)
                OpenedSkills = new()
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
            
            return OpenedSkills.FirstOrDefault(skillWithLevel => skillWithLevel.Id == skillId);
        }
    }
}