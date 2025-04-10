using Configs;

namespace Game.Skills
{
    public abstract class Skill
    {
        public virtual void Init(SkillScope scope, SkillDataByLevel skillDataByLevel){}
        public virtual void OnSkillRegister(){}
        public virtual void SkillProcess(){}
    }
    
    public abstract class DamageSkill : Skill
    {
        public virtual void SkillProcess(Enemy.Enemy enemy){}
    }
}