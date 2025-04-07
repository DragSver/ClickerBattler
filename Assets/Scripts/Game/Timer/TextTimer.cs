using TMPro;
using UnityEngine;

namespace Game.Timer
{
    public class TextTimer : Timer
    {
        [SerializeField] private TextMeshProUGUI _timerText;

        public override void SetMaxTime(float maxTime) => SetTime(maxTime);

        public override void SetTime(float currentTime)
        {
            _timerText.text = currentTime.ToString("00.000") + " s.";
        }

    }
}