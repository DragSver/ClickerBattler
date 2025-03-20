using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ClickRPG.Meta.Locations
{
    public class Location : MonoBehaviour
    {
        [SerializeField] private List<PinView> _pinViews;
        [SerializeField] private TextMeshProUGUI _locationName;

        [SerializeField] private int _currentLevel = 3;

        public void Init(UnityAction<int> levelStartCallback)
        {
            for (int i = 0; i < _pinViews.Count; i++)
            {
                var levelNumber = i + 1;
                PinType pinType;
                if (levelNumber < _currentLevel + 1) pinType = PinType.Complete;
                else if (levelNumber== _currentLevel + 1) pinType = PinType.Current;
                else pinType = PinType.Closed;
                
                _pinViews[i].Init(levelNumber, pinType, () => levelStartCallback?.Invoke(levelNumber));
            }
        }

        public void SetActive(bool active) => gameObject.SetActive(active);
    }
}