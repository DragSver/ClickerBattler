using System.Collections.Generic;
using Global.SaveSystem.SavableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ClickRPG.Meta.Locations
{
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] private List<Location> _locations;
        [SerializeField] private Button _nextLocationButton;
        [SerializeField] private Button _previousLocationButton;

        private int _currentLocation;

        public void Init(Progress progress, UnityAction<int, int> startLevelCallback)
        {
            _currentLocation = progress.CurrentLocation;
            FirstInitLocations(progress, startLevelCallback);
            FirstInitButtons();
        }

        private void ShowNextLocation()
        {
            _locations[_currentLocation].SetActive(false);
            _currentLocation++;
            _locations[_currentLocation].SetActive(true);
            
            if (_currentLocation == _locations.Count-1)
                _nextLocationButton.gameObject.SetActive(false);
            if (_currentLocation == 1)
                _previousLocationButton.gameObject.SetActive(true);
        }
        private void ShowPreviousLocation()
        {
            _locations[_currentLocation].SetActive(false);
            _currentLocation--;
            _locations[_currentLocation].SetActive(true);
            
            if (_currentLocation == _locations.Count-2)
                _nextLocationButton.gameObject.SetActive(true);
            if (_currentLocation == 0)
                _previousLocationButton.gameObject.SetActive(false);
        }

        private void FirstInitButtons()
        {
            _nextLocationButton.onClick.AddListener(ShowNextLocation);
            _previousLocationButton.onClick.AddListener(ShowPreviousLocation);
            
            if (_currentLocation == _locations.Count-1)
                _nextLocationButton.gameObject.SetActive(true);
            if (_currentLocation == 0)
                _previousLocationButton.gameObject.SetActive(false);
        }
        
        private void FirstInitLocations(Progress progress, UnityAction<int, int> startLevelCallback)
        {
            for (var i = 0; i < _locations.Count; i++)
            {
                var locationNum = i;

                var isLocationPassed = locationNum > progress.CurrentLocation
                    ? ProgressState.Complete
                    : (locationNum == progress.CurrentLocation ? ProgressState.Current : ProgressState.Closed);
                var currentLevel = progress.CurrentLocation;
                
                _locations[locationNum].Init(isLocationPassed, currentLevel, level => startLevelCallback?.Invoke(locationNum, level));
                _locations[locationNum].SetActive(progress.CurrentLocation == locationNum);
            }
        }
    }
}