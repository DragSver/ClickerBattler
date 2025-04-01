using System;
using UnityEngine;

namespace Game.Enemy
{
    [Serializable]
    public struct EnemyViewData
    {
        public string Id;
        public string Name;
        public Sprite Sprite;
        public Elements Element;
    }
}