using System.Collections;
using Global.Audio;
using SceneManegement.EnterParams;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManegement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;
        private AudioManager _audioManager;


        public void Init(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        public void LoadMetaScene(SceneEnterParams enterParams = null)
        {
            StartCoroutine(LoadAndStartMeta(enterParams));
        }
        public void LoadGameplayScene(GameEnterParams enterParams)
        {
            StartCoroutine(LoadAndStartGameplay(enterParams));
        }

        private IEnumerator LoadAndStartGameplay(GameEnterParams enterParams)
        {
            _loadingScreen.SetActive(true);

            yield return LoadScene(Scenes.Boostrap);
            yield return LoadScene(Scenes.GameScene);

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
            _audioManager.Load(sceneName);
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}