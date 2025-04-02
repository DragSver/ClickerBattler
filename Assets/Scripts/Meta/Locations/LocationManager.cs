using System.Collections.Generic;
using Configs;
using Global.SaveSystem;
using Global.SaveSystem.SavableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Meta.Locations
{
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] private List<Location> _locations;
        
        [SerializeField] private LevelMapViewConfig _levelMapViewConfig;
        [SerializeField] private LevelMapViewController _levelMapViewController;

        private int _currentLocation;
        private SaveSystem _saveSystem;

        public void Init(Progress progress, UnityAction<int, int> startLevelCallback, SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _levelMapViewController.Init(ShowNextLocation, ShowPreviousLocation, null, null, null);
            _currentLocation = progress.CurrentLocation;
            _levelMapViewConfig.Init();
            FirstInitLocations(progress, startLevelCallback);
            FirstInitButtons();
        }

        private void ShowNextLocation()
        {
            _locations[_currentLocation].SetActive(false);
            _currentLocation++;
            SetViewLocation(_currentLocation);
            _locations[_currentLocation].SetActive(true);
            
            if (_currentLocation == _locations.Count-1 || _locations[_currentLocation+1].ProgressState == ProgressState.Closed)
                _levelMapViewController.NextLocationButton.gameObject.SetActive(false);
            if (_currentLocation == 1)
                _levelMapViewController.PreviousLocationButton.gameObject.SetActive(true);
        }
        private void ShowPreviousLocation()
        {
            _locations[_currentLocation].SetActive(false);
            _currentLocation--;
            SetViewLocation(_currentLocation);
            _locations[_currentLocation].SetActive(true);
            
            if (_currentLocation == _locations.Count-2)
                _levelMapViewController.NextLocationButton.gameObject.SetActive(true);
            if (_currentLocation == 0)
                _levelMapViewController.PreviousLocationButton.gameObject.SetActive(false);
        }

        private void FirstInitButtons()
        {
            if (_currentLocation == _locations.Count-1)
                _levelMapViewController.NextLocationButton.gameObject.SetActive(true);
            if (_currentLocation == 0)
                _levelMapViewController.PreviousLocationButton.gameObject.SetActive(false);
        }
        
        private void FirstInitLocations(Progress progress, UnityAction<int, int> startLevelCallback)
        {
            for (var i = 0; i < _locations.Count; i++)
            {
                var locationNum = i;

                var isLocationCompleted = locationNum < progress.CurrentLocation
                    ? ProgressState.Complete
                    : (locationNum == progress.CurrentLocation ? ProgressState.Current : ProgressState.Closed);
                
                _locations[locationNum].Init(isLocationCompleted, progress.CurrentLevel, level => startLevelCallback?.Invoke(locationNum, level));
                _locations[locationNum].SetActive(progress.CurrentLocation == locationNum);
            }
            SetViewLocation(_currentLocation);
        }

        private void SetViewLocation(int location)
        {
            var levelMapView = _levelMapViewConfig.GetLevelMapViewData(location);
            var coinsAmount = (Wallet)_saveSystem.GetData(SavableObjectType.Wallet);
            _levelMapViewController.SetLocation(levelMapView, coinsAmount.Coins);
        }
    }
}