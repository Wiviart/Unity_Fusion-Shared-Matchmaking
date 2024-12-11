using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSceneManager : NetworkSceneManagerDefault
{
    public static NetworkSceneManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadLevel(NetworkRunner runner, int nextLevelIndex)
    {
        if (!runner.IsSceneAuthority) return;
        Debug.Log($"LevelManager.LoadLevel({nextLevelIndex});");
        runner.LoadScene(SceneRef.FromIndex(nextLevelIndex), new LoadSceneParameters(LoadSceneMode.Single), true);
    }
}