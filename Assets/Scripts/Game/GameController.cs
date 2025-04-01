using ClickRPG.CriticalHit;
using ClickRPG.Meta;
using ClickRPG.SceneManagement;
using Game.Configs.Levels;
using Global.Audio;
using Global.SaveSystem;
using Global.SaveSystem.SavableObjects;
using UnityEngine;

namespace ClickRPG {
    public class GameController : EntryPoint
    {
        [Header("Enemy")]
        [SerializeField] private EnemyController _enemyController;
        
        [Header("GameScreen")]
        [SerializeField] private ButtonController _clickAttackButtonController;
        [SerializeField] private CriticalHitController _criticalHitController;
        [SerializeField] private LevelsConfig _levels;
        [SerializeField] private Timer _timer;
        [SerializeField] private float _maxLevelTime = 10;
        [SerializeField] private float _damage = 1f;
        [SerializeField] private ButtonData _clickAttackButtonData;

        [Header("EndLevelScreen")]
        [SerializeField] private EndLevelScreenController _endLevelScreenController;
        [SerializeField] private EndLevelScreenData _victoryScreenData;
        [SerializeField] private EndLevelScreenData _loseScreenData;

        private GameEnterParams _gameEnterParams;
        private SaveSystem _saveSystem;
        private AudioManager _audioManager;
        
        private Progress _progress;

        private const string SCENE_LOADER_TAG = "SceneLoader";

        
        public override void Run(SceneEnterParams enterParams)
        {
            _saveSystem = FindFirstObjectByType<SaveSystem>();
            _audioManager = FindFirstObjectByType<AudioManager>();
            _audioManager.Play(AudioNames.Audio_Game_BG, false);
            
            if (enterParams is not GameEnterParams)
            {
                Debug.LogError("Неправильная передача параметров сцены");
                return;
            }
            var gameEnterParams = (GameEnterParams)enterParams;
            _gameEnterParams = gameEnterParams;
            
            _criticalHitController.Init();
            _enemyController.Init(_timer);
            _clickAttackButtonController.Init(_clickAttackButtonData);
            _clickAttackButtonController.OnClick += () => DamageEnemy(_criticalHitController.GetDamageMultiplierPointerPosition(_damage));

            StartLevel();
        }

        private void StartLevel()
        {
            _endLevelScreenController.HideEndLevelScreen();
            
            var levelData = _levels.GetLevel(_gameEnterParams.Location, _gameEnterParams.Level);
            _enemyController.StartLevel(levelData);
            _enemyController.OnLevelPassed += EndLevel;
            _criticalHitController.StartGenerateCriticalPoint();
        }

        private void EndLevel(bool levelPassed)
        {
            _timer.Stop();
            _criticalHitController.StopGenerateCriticalPoint();
            
            _endLevelScreenController.OnMapButtonClick += LoadMetaScene;

            if (levelPassed)
            {
                _progress = (Progress)_saveSystem.GetData(SavableObjectType.Progress);
                if (_gameEnterParams.Location == _progress.CurrentLocation &&
                    _gameEnterParams.Level == _progress.CurrentLevel)
                {
                    var maxLevel = _levels.GetMaxLevelOnLocation(_progress.CurrentLocation);
                    if (_progress.CurrentLevel >= maxLevel)
                    {
                        _progress.CurrentLevel = 1;
                        _progress.CurrentLocation++;
                    }
                    else _progress.CurrentLevel++;
                    _saveSystem.SaveData(SavableObjectType.Progress);
                }
                
                var currentTime = _maxLevelTime - _timer.CurrentTime;
                var bestTime = GameStats.SaveBestTime(currentTime);
                var totalKills = GameStats.AddKills();
                
                var victoryData = _victoryScreenData;
                victoryData.KillTimeText = currentTime.ToString("00:00.000s");
                victoryData.BestKillTimeText = bestTime.ToString("00:00.000s");
                victoryData.StatisticText = victoryData.StatisticText.Replace("N", totalKills.ToString());
                
                _endLevelScreenController.OnContinueGameClick += NextLevel;

                _endLevelScreenController.CallEndLevelScreen(victoryData);
            }
            else
            {
                var totalDeaths = GameStats.AddDeaths();
                
                var loseData = _loseScreenData;
                loseData.StatisticText = loseData.StatisticText.Replace("N", totalDeaths.ToString());
                
                _endLevelScreenController.OnContinueGameClick += RestartLevel;
                
                _endLevelScreenController.CallEndLevelScreen(loseData);
            }
        }

        private void DamageEnemy(float damage) => _enemyController.DamageCurrentEnemy(damage);

        private void RestartLevel()
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadGameplayScene(_gameEnterParams);
            _endLevelScreenController.OnContinueGameClick -= RestartLevel;
        }
        private void NextLevel()
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadGameplayScene(new GameEnterParams(level: _progress.CurrentLevel, location: _progress.CurrentLocation));
            _endLevelScreenController.OnContinueGameClick -= NextLevel;
        }
        
        private void LoadMetaScene()
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadMetaScene();
        }
    }
}
