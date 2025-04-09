using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [Serializable]
    public struct SkillData
    {
        public string Id;
        public string Name;
        public string Description;
        public Sprite Icon;
        public List<SkillDataByLevel> SkillsLevel;
    }
}