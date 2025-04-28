using System;
using Global.Audio;
using Global.SaveSystem;
using Global.SaveSystem.SavableObjects;
using System.Linq;
using Configs;
using Configs.LevelConfigs;
using Datas.Game;
using Datas.Global;
using Game.AttackButtons;
using Game.CriticalHit;
using Game.Enemy;
using Game.Skills;
using Game.Timer;
using SceneManegement;
using SceneManegement.EnterParams;
using UnityEngine;
using UnityEngine.Events;

namespace Game {
    public class GameController : EntryPoint
    {
        [Header("Game")]
        [SerializeField] private EnemyController _enemyController;
        [SerializeField] private TimerController _timerController;
        [SerializeField] private AttackButtonController _attackButtonController;
        [SerializeField] private CriticalHitController _criticalHitController;

        [Header("UI")] 
        [SerializeField] private GameObject _pausePopup;
        [SerializeField] private LocationViewController _locationViewController;
        
        [Header("Configs")]
        [SerializeField] private LevelsConfig _levels;
        [SerializeField] private LevelsViewConfig _levelsViewConfig;
        [SerializeField] private SkillConfig _skillConfig;

        [Header("EndLevelScreen")]
        [SerializeField] private EndLevelScreenController _endLevelScreenController;
        [SerializeField] private EndLevelScreenData _victoryScreenData;
        [SerializeField] private EndLevelScreenData _loseScreenData;

        private LevelData _levelData;
        
        private GameEnterParams _gameEnterParams;
        private SaveSystem _saveSystem;
        private AudioManager _audioManager;
        private SkillSystem _skillSystem;
        
        private Progress _progress;
        private Wallet _wallet;
        private Stats _stats;


        private const string SCENE_LOADER_TAG = "SceneLoader";

        
        public override void Run(SceneEnterParams enterParams)
        {
            _saveSystem = FindFirstObjectByType<SaveSystem>();
            _audioManager = FindFirstObjectByType<AudioManager>();
            _audioManager.Play(AudioNames.Audio_Game_BG, false);
            _gameEnterParams = ReceiveGameEnterParams(enterParams);

            var openedSkills = (OpenSkills)_saveSystem.GetData(SavableObjectType.OpenSkills);
            _stats = (Stats)_saveSystem.GetData(SavableObjectType.Stats);
            _wallet = (Wallet)_saveSystem.GetData(SavableObjectType.Wallet);
            _progress = (Progress)_saveSystem.GetData(SavableObjectType.Progress);
            
            _enemyController.Init(_timerController, _stats, _saveSystem);
            _enemyController.OnLevelComplete += EndLevel;
            
            _skillSystem = new(openedSkills, _skillConfig, _enemyController, _criticalHitController);
            
            _levelsViewConfig.Init();

            StartLevel();
        }


        private void StartLevel()
        {
            _endLevelScreenController.HideEndLevelScreen();
            _timeOnLevel = 0;
            _levelData = _levels.GetLevel(_gameEnterParams.Location, _gameEnterParams.Level);

            var locationView = _levelsViewConfig.GetLocationViewData(_levelData.Location);
            _locationViewController.ClearLocationView();
            _locationViewController.SetLocationView(locationView,
                _gameEnterParams.Level+1, _levels.GetCountMainLevelOnLocation(_gameEnterParams.Location),
                _wallet.Coins, LoadMetaScene, null, () =>
                {
                    _timerController.SwitchPause();
                    _isPlaying = !_isPlaying;
                    _pausePopup.SetActive(!_isPlaying);
                });
            
            _attackButtonController = _locationViewController.AttackButtonController;
            _attackButtonController.Init(_locationViewController.Canvas);
            _criticalHitController.Init(_attackButtonController.GetComponent<RectTransform>());
            _attackButtonController.OnClick += AttackClick;
            
            _enemyController.StartLevel(_levelData);
            _isPlaying = true;
            _criticalHitController.StartGenerateCriticalPoint();
        }

        private void EndLevel(bool levelPassed)
        {
            _isPlaying = false;
            _timerController.Stop();
            _attackButtonController.Unsubscribe();
            _criticalHitController.StopGenerateCriticalPoint();

            if (levelPassed)
            {
                TrySaveEndLevelData();
                AddAndSaveRewards(_levelData.Rewards[0].Count);

                UnityAction continueClick;
                string continueButtonText;
                var gameEnterParams = GetNextLevelGameEnterParams();
                if (gameEnterParams == null)
                {
                    continueClick = LoadRestartLevel;
                    continueButtonText = "Переиграть уровень";
                }
                else
                {
                    continueClick = LoadNextLevel;
                    continueButtonText = "Продолжить путешествие";
                }
                
                _endLevelScreenController.CallEndLevelScreen(EditVictoryData(), continueClick, continueButtonText, LoadMetaScene, _levelData.Rewards, _wallet.Coins, true);
            }
            else
            {
                var loseData = _loseScreenData;
                var totalDeaths = _stats.DeathCount;
                loseData.FirstLabel = $"Вы погибли {totalDeaths} раз";
                _endLevelScreenController.CallEndLevelScreen(loseData, LoadRestartLevel, "Попробовать снова", LoadMetaScene, null, _wallet.Coins, false);
            }
        }
        
        
        private void AttackClick(Enemy.Enemy enemy, Elements element, float damage)
        {
            var multiplierDamage = _criticalHitController.GetDamageMultiplierPointerPosition(damage);
            // DamageEnemy(enemy, element, multiplierDamage);
            _skillSystem.InvokeTrigger(SkillTrigger.OnDamage);
            switch (element)
            {
                case Elements.Blood:
                    _skillSystem.InvokeDamageTrigger(SkillTrigger.OnBloodDamage, enemy);
                    break;
                case Elements.Water:
                    _skillSystem.InvokeDamageTrigger(SkillTrigger.OnWaterDamage, enemy);
                    break;
                case Elements.Sun:
                    _skillSystem.InvokeDamageTrigger(SkillTrigger.OnSunDamage, enemy);
                    break;
                case Elements.Moon:
                    _skillSystem.InvokeDamageTrigger(SkillTrigger.OnMoonDamage, enemy);
                    break;
                case Elements.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(element), element, null);
            }
        }
        private void DamageEnemy(Enemy.Enemy enemy, Elements element, float damage) =>
            _enemyController.DamageEnemy(enemy, element, damage);

        private void LoadRestartLevel()
        {
            _endLevelScreenController.OnContinueGameClick -= LoadRestartLevel;
            
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadGameplayScene(_gameEnterParams);
        }
        private void LoadNextLevel()
        {
            _endLevelScreenController.OnContinueGameClick -= LoadNextLevel;
            
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadGameplayScene(GetNextLevelGameEnterParams());
        }
        private void LoadMetaScene()
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadMetaScene();
        }

        
        private GameEnterParams ReceiveGameEnterParams(SceneEnterParams enterParams)
        {
            if (enterParams is not GameEnterParams)
            {
                Debug.LogError("Неправильная передача параметров сцены");
                return null;
            }

            return (GameEnterParams)enterParams;
        }
        private GameEnterParams GetNextLevelGameEnterParams()
        {
            var currentLevel = _levelData.LevelNumber;
            var currentLocation = _levelData.Location;
            var maxLevel = _levels.GetCountMainLevelOnLocation(currentLocation);
            if (currentLevel+1 >= maxLevel)
            {
                if (currentLocation + 1 < _levels.Locations.Count && _levels.Locations[currentLocation+1].Levels.Count > 0)
                {
                    currentLevel = 0;
                    currentLocation++;
                }
                else
                    return null;
            }
            else
                currentLevel++;
            
            return new GameEnterParams(level: currentLevel, location: currentLocation);
        }
        private void TrySaveEndLevelData()
        {
            if (_gameEnterParams.Location == _progress.CurrentLocation &&
                _gameEnterParams.Level == _progress.CurrentLevel)
            {
                var maxLevel = _levels.GetCountMainLevelOnLocation(_progress.CurrentLocation);
                if (_progress.CurrentLevel+1 >= maxLevel)
                {
                    _progress.CurrentLevel = 0;
                    _progress.CurrentLocation++;
                }
                else _progress.CurrentLevel++;
                _saveSystem.SaveData(SavableObjectType.Progress);
            }
        }
        private void AddAndSaveRewards(int coins)
        {
            _wallet.Coins += coins;
            _saveSystem.SaveData(SavableObjectType.Wallet);
        }
        
        private EndLevelScreenData EditVictoryData()
        {
            var victoryData = _victoryScreenData;
            
            var totalKills = _stats.KillsCount;
            var currentTime = _timeOnLevel;
            var bestTime = GameStats.SaveBestTimeOnID(currentTime, $"{_levelData.Location}:{_levelData.LevelNumber}");
            victoryData.FirstLabel = $"Вы одолели {totalKills} врагов!";
            victoryData.SecondLabel = currentTime.ToString("00:00.000s");
            victoryData.ThirdLabel = bestTime.ToString("00:00.000s");
            return victoryData;
        }

        private float _timeOnLevel = 0;
        private bool _isPlaying = false;
        private void FixedUpdate()
        {
            if (!_isPlaying) return;

            var deltaTime = Time.fixedDeltaTime;
            _timeOnLevel += deltaTime;
        }
    }
}
