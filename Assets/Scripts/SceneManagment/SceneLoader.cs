using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClickRPG.SceneManagment
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;


        public void LoadMetaScene(SceneEnterParams enterParams = null)
        {
            StartCoroutine(LoadAndStartMeta(enterParams));
        }
        public void LoadGameplayScene(SceneEnterParams enterParams = null)
        {
            StartCoroutine(LoadAndStartGameplay(enterParams));
        }

        private IEnumerator LoadAndStartGameplay(SceneEnterParams enterParams = null)
        {
            _loadingScreen.SetActive(true);

            yield return LoadScene(Scenes.Boostrap);
            yield return LoadScene(Scenes.LevelScene);

            var sceneEntryPoint = FindFirstObjectByType<EntryPoint>();
            sceneEntryPoint.Run(enterParams);
            
            _loadingScreen.SetActive(false);
        }

        private IEnumerator LoadAndStartMeta(SceneEnterParams enterParams)
        {
            _loadingScreen.SetActive(true);

            yield return LoadScene(Scenes.Boostrap);
            yield return LoadScene(Scenes.MetaScene);

            var sceneEntryPoint = FindFirstObjectByType<EntryPoint>();
            sceneEntryPoint.Run(enterParams);
            
            _loadingScreen.SetActive(false);
        }
        
        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}