using System;
using Game.AttackButtons;
using UnityEngine;

namespace Datas.Game
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