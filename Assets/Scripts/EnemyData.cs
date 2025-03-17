using System;
using UnityEngine;

namespace ClickRPG
{
    [Serializable]
    public struct EnemyData
    {
        public string EnemyId;
        public Sprite EnemySprite;
        public float Health;
    }
}