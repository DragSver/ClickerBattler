using ClickRPG.SceneManagment;
using UnityEngine;
using UnityEngine.UI;

namespace ClickRPG.Meta
{
    public class MetaEntryPoint : EntryPoint
    {
        [SerializeField] private Button _startLevelButton;

        private const string SCENE_LOADER_TAG = "SceneLoader";
        
        public override void Run(SceneEnterParams enterParams)
        {
            _startLevelButton.onClick.AddListener(StartLevel);
        }

        private void StartLevel()
        {
            var sceneLoader = GameObject.FindWithTag(SCENE_LOADER_TAG).GetComponent<SceneLoader>();
            sceneLoader.LoadGameplayScene();
        }
    }
}