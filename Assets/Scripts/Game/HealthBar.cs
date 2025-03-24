using UnityEngine;
using UnityEngine.UI;

namespace ClickRPG
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _backgroundSliderImage;
        [SerializeField] private Image _fillAreaSliderImage;

        public void Init()
        {
            
        }

        public void SetMaxValue(float value)
        {
            _slider.maxValue = value;
            _slider.value = value;
        }

        public void DecreaseValue(float value)
        {
            _slider.value -= value;
        }
    }
}