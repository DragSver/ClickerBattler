using Global.Audio;
using Global.SaveSystem;
using Global.SaveSystem.SavableObjects;
using Meta.Locations;
using SceneManegement;
using SceneManegement.EnterParams;
using UnityEngine;

namespace Meta
{
    public class MetaEntryPoint : EntryPoint
    {
        [SerializeField] private LocationManager _locationManager;
        private SaveSystem _saveSystem;
        private AudioManager _audioManager;
        
        [SerializeField] private int _currentLocation = 1;

        private const string SCENE_LOADER_TAG = "SceneLoader";
        
        
        public override void Run(SceneEnterParams enterParams){
            _saveSystem = FindFirstObjectByType<SaveSystem>();
            _audioManager = FindFirstObjectByType<AudioManager>();
            _audioManager.Play(AudioNames.Audio_Meta_BG, false);

            var progress = (Progress)_saveSystem.GetData(SavableObjectType.Progress);
            
            _locationManager.Init(progress, StartLevel, _saveSystem);
            // _startLevelButton.onClick.AddListener(StartLevel);
        }

        private void StartLevel(int location, int level)
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadGameplayScene(new GameEnterParams(location, level));
        }
    }
}