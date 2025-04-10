using Configs;
using Game.CriticalHit;
using Game.Enemy;

namespace Game.Skills.SkillVariants
{
    public class WaterDamageSkill : DamageSkill
    {
        private EnemyController _enemyController;
        private CriticalHitController _criticalHitController;
        private SkillDataByLevel _skillDataByLevel;
        
        public override void Init(SkillScope scope, SkillDataByLevel skillDataByLevel)
        {
            _enemyController = scope.EnemyController;
            _criticalHitController = scope.CriticalHitController;
            _skillDataByLevel = skillDataByLevel;
        }

        public override void SkillProcess(Enemy.Enemy enemy)
        {
            var multiplierDamage = _criticalHitController.GetDamageMultiplierPointerPosition(_skillDataByLevel.Value);
            _enemyController.DamageEnemy(enemy,Elements.Water, multiplierDamage);
        }
    }
}