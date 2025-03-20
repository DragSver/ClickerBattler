using System.Collections.Generic;
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

        public void Init(int currentLocation, UnityAction<Vector2Int> startLevelCallback)
        {
            _currentLocation = currentLocation;
            FirstInitLocations(currentLocation, startLevelCallback);
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
        
        private void FirstInitLocations(int currentLocation, UnityAction<Vector2Int> startLevelCallback)
        {
            for (var i = 0; i < _locations.Count; i++)
            {
                _locations[i].Init(level => startLevelCallback?.Invoke(new(i, level)));
                _locations[i].SetActive(currentLocation == i);
            }
        }
    }
}