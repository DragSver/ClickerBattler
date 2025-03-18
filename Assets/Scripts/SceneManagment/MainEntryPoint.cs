using UnityEngine;

namespace ClickRPG.SceneManagment
{
    public class MainEntryPoint : MonoBehaviour
    {
        private const string SCENE_LOADER_TAG = "SceneLoader";


        public void Awake()
        {
            if (GameObject.FindGameObjectWithTag(SCENE_LOADER_TAG)) return;

            var sceneLoaderPrefab = Resources.Load<SceneLoader>("SceneLoader");
            var sceneLoader = Instantiate(sceneLoaderPrefab);
            DontDestroyOnLoad(sceneLoader);
            
            sceneLoader.LoadMetaScene();
        }
    }
}