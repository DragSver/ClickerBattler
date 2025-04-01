using Global.Audio;
using Global.SaveSystem;
using UnityEngine;

namespace SceneManegement
{
    public class MainEntryPoint : MonoBehaviour
    {
        private const string SCENE_LOADER_TAG = "SceneLoader";


        public void Awake()
        {
            if (GameObject.FindGameObjectWithTag(SCENE_LOADER_TAG)) return;

            var sceneLoaderPrefab = Resources.Load<SceneLoader>("Prefabs/SceneLoader");
            var sceneLoader = Instantiate(sceneLoaderPrefab);
            DontDestroyOnLoad(sceneLoader);

            var saveSystem = new GameObject().AddComponent<SaveSystem>();
            saveSystem.Initialize();
            DontDestroyOnLoad(saveSystem);

            var audioManagerPrefab = Resources.Load<AudioManager>("Prefabs/AudioManager");
            var audioManager = Instantiate(audioManagerPrefab);
            audioManager.LoadOnce();
            DontDestroyOnLoad(audioManager);

            sceneLoader.Init(audioManager);
            
            
            sceneLoader.LoadMetaScene();
        }
    }
}