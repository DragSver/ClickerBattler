using System.Collections.Generic;
using UnityEngine;

namespace Configs 
{
    [CreateAssetMenu(menuName="Configs/SkillConfig", fileName = "SkillConfig")]
    public class SkillConfig : ScriptableObject
    {
        [SerializeField] private List<SkillData> _skillDatas;
        private Dictionary<string, Dictionary<int, SkillDataByLevel>> _skillDataByLevelMap;
        private Dictionary<string, SkillData> _skillDataMap;

        public bool NextLevelExist(string skillId, int level)
        {
            return _skillDataByLevelMap.TryGetValue(skillId, out var skillDataMap) && skillDataMap.TryGetValue(level, out var skillData);
        }        
        public SkillDataByLevel GetSkillDataByLevel(string skillId, int level)
        {
            if (_skillDataByLevelMap == null || _skillDataByLevelMap.Count == 0)
                FillSkillDataMaps();

            return _skillDataByLevelMap[skillId][level];
        }

        public SkillData GetSkillData(string skillId)
        {
            if (_skillDataMap == null || _skillDataMap.Count == 0)
                FillSkillDataMaps();
            
            return _skillDataMap[skillId];
        }

        private void FillSkillDataMaps()
        {
            _skillDataByLevelMap = new Dictionary<string, Dictionary<int, SkillDataByLevel>>();
            _skillDataMap = new Dictionary<string, SkillData>();
            foreach (var skillData in _skillDatas)
            {
                if (!_skillDataByLevelMap.ContainsKey(skillData.Id))
                    _skillDataByLevelMap[skillData.Id] = new Dictionary<int, SkillDataByLevel>();
                _skillDataMap.TryAdd(skillData.Id, skillData);
                
                foreach (var skillDataByLevel in skillData.SkillsLevel)
                    _skillDataByLevelMap[skillData.Id][skillDataByLevel.Level] = skillDataByLevel;
            }
        }
    }
}