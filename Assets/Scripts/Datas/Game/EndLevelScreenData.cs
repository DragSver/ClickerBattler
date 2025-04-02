using System;
using UnityEngine;

namespace Datas.Game
{
    [Serializable]
    public struct EndLevelScreenData
    {
        public Sprite Background;
        public Sprite Flag;

        public Color ColorMainTextHolder;

        public string MainText;
        public Color ColorMainText;

        public string FirstLabel;
        public string SecondLabel;
        public string ThirdLabel;
        public Color ColorAdviceText;
    }
}