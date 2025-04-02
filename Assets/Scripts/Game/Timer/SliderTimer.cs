using UnityEngine;
using UnityEngine.UI;

namespace Game.Timer
{
    public class SliderTimer : Timer
    {
        [SerializeField] private Slider _timerSlider;

        public override void SetMaxTime(float maxTime)
        {
            _timerSlider.maxValue = maxTime;
            SetTime(maxTime);
        }

        public override void SetTime(float currentTime)
        {
            _timerSlider.value = currentTime;
        }
    }
}