using System;
using System.Collections.Generic;

namespace Configs
{
    [Serializable]
    public struct SkillData
    {
        public string Id;
        public List<SkillDataByLevel> SkillsLevel;
    }
}