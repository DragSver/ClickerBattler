using System;
using System.Collections.Generic;
using Configs;
using Game.CriticalHit;
using Game.Enemy;
using Global.SaveSystem.SavableObjects;

namespace Game.Skills
{
    public class SkillSystem
    {
        private SkillScope _scope;
        private SkillConfig _skillConfig;

        private Dictionary<SkillTrigger, List<Skill>> _skillByTrigger;
        
        
        public SkillSystem(OpenSkills openSkills, SkillConfig skillConfig, EnemyController enemyController, CriticalHitController criticalHitController)
        {
            _scope = new ()
            {
                EnemyController = enemyController,
                CriticalHitController = criticalHitController,
            };
            _skillByTrigger = new();
            _skillConfig = skillConfig;
            
            foreach (var openSkill in openSkills.OpenedSkills)
            {
                RegisterSkill(openSkill);
            }
        }

        public void InvokeTrigger(SkillTrigger trigger)
        {
            if (!_skillByTrigger.ContainsKey(trigger)) return;

            var skillToActivate = _skillByTrigger[trigger];
            foreach (var skill in skillToActivate)
            {
                skill.SkillProcess();
            }
        }
        public void InvokeDamageTrigger(SkillTrigger trigger, Enemy.Enemy enemy)
        {
            if (!_skillByTrigger.ContainsKey(trigger)) return;

            var skillToActivate = _skillByTrigger[trigger];
            foreach (var skill in skillToActivate)
            {
                if (skill is DamageSkill damageSkill)
                    damageSkill.SkillProcess(enemy);
            }
        }
        
        private void RegisterSkill(SkillWithLevel skill)
        {
            var skillData = _skillConfig.GetSkillDataByLevel(skill.Id, skill.Level);

            var skillType = Type.GetType($"Game.Skills.SkillVariants.{skill.Id}");
            if (skillType == null)
                throw new Exception($"Skill with id {skill.Id} hot found");
            
            if (Activator.CreateInstance(skillType) is not Skill skillInstance)
                throw new Exception($"Can not create skill with id {skill.Id}");
                
            skillInstance.Init(_scope, skillData);

            if (!_skillByTrigger.ContainsKey(skillData.Trigger))
            {
                _skillByTrigger[skillData.Trigger] = new();
            }
            _skillByTrigger[skillData.Trigger].Add(skillInstance);
        }
    }
}