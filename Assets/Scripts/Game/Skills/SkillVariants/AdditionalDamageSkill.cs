using Configs;
using Game.Enemy;

namespace Game.Skills.SkillVariants
{
    public class AdditionalDamageSkill : Skill
    {
        private EnemyController _enemyController;
        private SkillDataByLevel _skillDataByLevel;
        
        public override void Init(SkillScope scope, SkillDataByLevel skillDataByLevel)
        {
            _enemyController = scope.EnemyController;
            _skillDataByLevel = skillDataByLevel;
        }

        public override void SkillProcess()
        {
            _enemyController.DamageRandomEnemy(Elements.None, _skillDataByLevel.Value);
        }
    }
}