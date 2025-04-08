using System.Collections.Generic;
using UnityEngine;

namespace Configs 
{
    [CreateAssetMenu(menuName="Configs/SkillConfig", fileName = "SkillConfig")]
    public class SkillConfig : ScriptableObject
    {
        [SerializeField] private List<SkillData> _skillDatas;
        private Dictionary<string, Dictionary<int, SkillDataByLevel>> _skillDataByLevel;
        
        public SkillDataByLevel GetSkillData(string skillId, int level)
        {
            if (_skillDataByLevel == null || _skillDataByLevel.Count == 0)
                FillSkillDataMap();

            return _skillDataByLevel[skillId][level];
        }

        private void FillSkillDataMap()
        {
            _skillDataByLevel = new();
            foreach (var skillData in _skillDatas)
            {
                if (!_skillDataByLevel.ContainsKey(skillData.Id))
                {
                    _skillDataByLevel[skillData.Id] = new();
                }
                
                foreach (var skillDataByLevel in skillData.SkillsLevel)
                {
                    _skillDataByLevel[skillData.Id][skillDataByLevel.Level] = skillDataByLevel;
                }
            }
        }
    }
}