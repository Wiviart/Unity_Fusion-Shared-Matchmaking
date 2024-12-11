using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wiviart.Utilities;

public class SceneLoader : Singleton<SceneLoader>
{
    private void Awake()
    {
        Instance = this;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the scene is loaded
        while (!asyncOperation.isDone)
        {
            Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");
            yield return null;
        }
    }
}