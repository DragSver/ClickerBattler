using Configs;

namespace Game.Skills
{
    public abstract class Skill
    {
        public virtual void Init(SkillScope scope, SkillDataByLevel skillDataByLevel){}
        public virtual void OnSkillRegister(){}
        public virtual void SkillProcess(){}
    }
}