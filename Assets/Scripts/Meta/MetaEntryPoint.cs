using Configs;
using Global.Audio;
using Global.SaveSystem;
using Global.SaveSystem.SavableObjects;
using Meta.Locations;
using Meta.Shop;
using SceneManegement;
using SceneManegement.EnterParams;
using UnityEngine;
using YG;

namespace Meta
{
    public class MetaEntryPoint : EntryPoint
    {
        [Header("Controllers")]
        [SerializeField] private LevelMap _levelMap;
        [SerializeField] private ShopWindow _shopWindow;
        
        [Header("Configs")]
        [SerializeField] private SkillConfig _skillConfig;
        
        private SaveSystem _saveSystem;
        private AudioManager _audioManager;
        private Wallet _wallet;

        private const string SCENE_LOADER_TAG = "SceneLoader";
        
        
        public override void Run(SceneEnterParams enterParams){
            YG2.InterstitialAdvShow();
            YG2.optionalPlatform.LoadRewardedAdv();
            
            _saveSystem = FindFirstObjectByType<SaveSystem>();
            _audioManager = FindFirstObjectByType<AudioManager>();
            _audioManager.Play(AudioNames.Audio_Meta_BG, false);

            var progress = (Progress)_saveSystem.GetData(SavableObjectType.Progress);
            _wallet = (Wallet)_saveSystem.GetData(SavableObjectType.Wallet);
            
            _levelMap.Init(progress, _wallet.Coins, StartLevel, CallShop);
            _shopWindow.Init(_saveSystem, _skillConfig, CallLevelMap);
        }

        private void StartLevel(int location, int level)
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadGameplayScene(new GameEnterParams(location, level));
        }

        private void CallShop()
        {
            _shopWindow.ActivateShopView(true);
            _levelMap.HideLevelMap();
        }

        private void CallLevelMap()
        {
            _levelMap.ActivateLevelMap(_wallet.Coins);
            _shopWindow.ActivateShopView(false);
        }
    }
}