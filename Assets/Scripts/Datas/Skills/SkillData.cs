using System;
using System.Collections.Generic;
using Game.Skills;
using UnityEngine;

namespace Configs
{
    [Serializable]
    public class SkillData
    {
        public string Id;
        public string Name;
        public string Description;
        public Sprite Icon;
        public SkillTrigger Trigger;
        public List<SkillDataByLevel> SkillsLevel;
    }
}