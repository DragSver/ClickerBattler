using System;
using UnityEngine;
using UnityEngine.UI;

namespace Button
{
    [Serializable]
    public struct ButtonData
    {
        public Sprite DefaultSprite;
        public string Text;
        public Image.Type ImageType;
        public ColorBlock ButtonColors;
    }
}