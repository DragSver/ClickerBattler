using ClickRPG.Meta.Locations;
using ClickRPG.SceneManagment;
using Unity.VisualScripting;
using UnityEngine;

namespace ClickRPG.Meta
{
    public class MetaEntryPoint : EntryPoint
    {
        [SerializeField] private LocationManager _locationManager;
        [SerializeField] private int _currentLocation = 1;

        private const string SCENE_LOADER_TAG = "SceneLoader";
        
        
        public override void Run(SceneEnterParams enterParams){
            _locationManager.Init(_currentLocation, StartLevel);
            // _startLevelButton.onClick.AddListener(StartLevel);
        }

        private void StartLevel(Vector2Int locationLevel)
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadGameplayScene();
        }
    }
}