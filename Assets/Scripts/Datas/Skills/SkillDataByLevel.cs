using System;
using Game.Skills;

namespace Configs
{
    [Serializable]
    public struct SkillDataByLevel
    {
        public int Level;
        public float Value;
        public SkillTrigger Trigger;
        public float TriggerValue;
        public int Price;
    }
}