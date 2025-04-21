using System;
using Game.Skills;

namespace Configs
{
    [Serializable]
    public class SkillDataByLevel
    {
        public int Level;
        public float Value;
        public float TriggerValue;
        public int Price;
    }
}