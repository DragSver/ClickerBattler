using System.Collections.Generic;
using Configs;
using Global.SaveSystem.SavableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Meta.Locations
{
    public class LevelMap : MonoBehaviour
    {
        [SerializeField] private RectTransform _map;
        
        [SerializeField] private List<Location> _locations;
        
        [SerializeField] private LevelMapViewConfig _levelMapViewConfig;
        [SerializeField] private LevelMapViewController _levelMapViewController;
        
        private int _currentLocation;
        

        public void Init(Progress progress, int wallet, UnityAction<int, int> startLevelCallback, UnityAction onShop)
        {
            _currentLocation = progress.CurrentLocation;
            
            _levelMapViewController.Init(ShowNextLocation, ShowPreviousLocation, null, null, onShop, _currentLocation, _locations.Count, wallet);
            FirstInitNavigationButtons();
            FirstInitLocations(progress, startLevelCallback);
            SetViewLocation(_currentLocation);
            
        }

        private void FirstInitNavigationButtons()
        {
            if (_currentLocation == _locations.Count-1 || _locations[_currentLocation+1].ProgressState == ProgressState.Closed)
                _levelMapViewController.NextLocationButton.gameObject.SetActive(true);
            if (_currentLocation == 0)
                _levelMapViewController.PreviousLocationButton.gameObject.SetActive(false);
        }
        private void ShowNextLocation()
        {
            _locations[_currentLocation].SetActive(false);
            _currentLocation++;
            SetViewLocation(_currentLocation);
            _locations[_currentLocation].SetActive(true);
            
            if (_currentLocation == _locations.Count-1 || _locations[_currentLocation+1].ProgressState == ProgressState.Closed)
                _levelMapViewController.NextLocationButton.gameObject.SetActive(false);
            if (_currentLocation >= 1)
                _levelMapViewController.PreviousLocationButton.gameObject.SetActive(true);
        }
        private void ShowPreviousLocation()
        {
            _locations[_currentLocation].SetActive(false);
            _currentLocation--;
            SetViewLocation(_currentLocation);
            _locations[_currentLocation].SetActive(true);
            
            if (_currentLocation <= _locations.Count-2)
                _levelMapViewController.NextLocationButton.gameObject.SetActive(true);
            if (_currentLocation == 0)
                _levelMapViewController.PreviousLocationButton.gameObject.SetActive(false);
        }
        private void FirstInitLocations(Progress progress, UnityAction<int, int> startLevelCallback)
        {
            for (var i = 0; i < _locations.Count; i++)
            {
                var locationNum = i;

                ProgressState isLocationCompleted;
                if (locationNum < progress.CurrentLocation)
                    isLocationCompleted = ProgressState.Complete;
                else if (locationNum == progress.CurrentLocation)
                    isLocationCompleted = ProgressState.Current;
                else 
                    isLocationCompleted = ProgressState.Closed;
                
                _locations[locationNum].Init(isLocationCompleted, progress.CurrentLevel, level => startLevelCallback?.Invoke(locationNum, level));
                _locations[locationNum].SetActive(progress.CurrentLocation == locationNum);
            }
        }

        private void SetViewLocation(int location)
        {
            var levelMapView = _levelMapViewConfig.GetLevelMapViewData(location);
            _levelMapViewController.SetLocation(levelMapView);
        }

        
        public void ActivateLevelMap(int wallet)
        {
            SetViewLocation(_currentLocation);
            _levelMapViewController.SetWallet(wallet);
            _map.gameObject.SetActive(true);
        }
        public void HideLevelMap()
        {
            _map.gameObject.SetActive(false);
        }
    }
}