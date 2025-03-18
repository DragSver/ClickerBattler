using TMPro;
using UnityEngine;

namespace ClickRPG
{
    public class TextTimer : Timer
    {
        [SerializeField] private TextMeshProUGUI _timerText;

        protected override void SetTime(float currentTime)
        {
            _timerText.text = currentTime.ToString("00.000");
        }

    }
}