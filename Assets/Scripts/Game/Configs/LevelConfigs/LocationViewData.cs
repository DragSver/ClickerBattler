using System;
using Button;
using UnityEngine;

namespace Game.Configs.LevelConfigs
{
    [Serializable]
    public struct LocationViewData
    {
        public int Id;
        public Sprite Background;
        public AttackButtonController AttackButtonControllerPrefab;
        public string Name;
        public Color TimerColor;
    }
}