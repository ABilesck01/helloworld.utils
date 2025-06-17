using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HelloWorld.Utils
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        [Header("Loading (opcional)")]
        [SerializeField] private string loadingSceneName = "Loading";
        [SerializeField] private float minLoadingTime = 1.5f;

        private string currentLevelScene; // para controle do cenário carregado

        /// <summary>
        /// Descarrega o level atual (cena de cenário).
        /// </summary>
        public void UnloadCurrentLevel()
        {
            if (!string.IsNullOrEmpty(currentLevelScene))
            {
                SceneManager.UnloadSceneAsync(currentLevelScene);
                currentLevelScene = null;
            }
        }

        private IEnumerator LoadLevelFlow(string mainScene, string levelScene, Action onLoaded)
        {
            // Carrega cena de loading
            if (!string.IsNullOrEmpty(loadingSceneName) && loadingSceneName != mainScene)
                SceneManager.LoadScene(loadingSceneName);

            yield return new WaitForSeconds(minLoadingTime);

            // Carrega a cena base (MainScene)
            AsyncOperation mainLoad = SceneManager.LoadSceneAsync(mainScene, LoadSceneMode.Single);
            while (!mainLoad.isDone)
                yield return null;

            yield return null; // dá tempo de iniciar managers, caso precise

            // Carrega a cena do cenário como additive
            AsyncOperation levelLoad = SceneManager.LoadSceneAsync(levelScene, LoadSceneMode.Additive);
            while (!levelLoad.isDone)
                yield return null;

            currentLevelScene = levelScene;
            onLoaded?.Invoke();
        }

        private IEnumerator LoadSceneSingleRoutine(string targetScene)
        {
            if (!string.IsNullOrEmpty(loadingSceneName) && loadingSceneName != targetScene)
                SceneManager.LoadScene(loadingSceneName);

            yield return new WaitForSeconds(minLoadingTime);

            //UnloadCurrentLevel();

            AsyncOperation load = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
            while (!load.isDone)
                yield return null;
        }

        /// <summary>
        /// Carrega uma cena única (ex: Menu, Seleção de Fase).
        /// </summary>
        public void LoadFullScene(string sceneName)
        {
            StartCoroutine(LoadSceneSingleRoutine(sceneName));
        }

        /// <summary>
        /// Carrega a MainScene e depois a cena do level additive.
        /// </summary>
        public void LoadGameplayLevel(string mainScene, string levelScene, Action onLoaded = null)
        {
            StartCoroutine(LoadLevelFlow(mainScene, levelScene, onLoaded));
        }
    }
}