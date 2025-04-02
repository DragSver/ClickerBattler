using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Meta.Locations
{
    public class Location : MonoBehaviour
    {
        [SerializeField] private List<PinView> _pinViews;

        public ProgressState ProgressState => _progressState;
        private ProgressState _progressState;


        public void Init(ProgressState locationState, int currentLevel, UnityAction<int> levelStartCallback)
        {
            _progressState = locationState;
            for (int i = 0; i < _pinViews.Count; i++)
            {
                var levelNumber = i;

                var pinType = locationState switch
                {
                    ProgressState.Complete => ProgressState.Complete,
                    ProgressState.Closed => ProgressState.Closed,
                    ProgressState.Current => levelNumber > currentLevel ? ProgressState.Closed :
                        (levelNumber == currentLevel ? ProgressState.Current : ProgressState.Complete),
                    _ => throw new ArgumentOutOfRangeException(nameof(locationState), locationState, null)
                };
                
                _pinViews[i].Init(levelNumber, pinType, () => levelStartCallback?.Invoke(levelNumber));
            }
        }

        public void SetActive(bool active) => gameObject.SetActive(active);
    }
}