using System;
using System.Collections;
using Fusion;
using FusionHelpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelManager : NetworkSceneManagerDefault
{
	private SceneRef _loadedScene = SceneRef.None;

	public void LoadLevel(int nextLevelIndex)
	{
		if (_loadedScene.IsValid)
		{
			Debug.Log($"LevelManager.UnloadLevel(); _loadedScene={_loadedScene}");
			Runner.UnloadScene(_loadedScene);
			_loadedScene = SceneRef.None;
		}
		Debug.Log($"LevelManager.LoadLevel({nextLevelIndex});");

		Runner.LoadScene(SceneRef.FromIndex(nextLevelIndex), new LoadSceneParameters(LoadSceneMode.Additive), true);
		_loadedScene = SceneRef.FromIndex(nextLevelIndex);
	}

	public override void Shutdown()
	{
		Debug.Log("LevelManager.Shutdown();");
		if (_loadedScene.IsValid)
		{
			Debug.Log($"LevelManager.UnloadLevel(); _loadedScene={_loadedScene}");
			SceneManager.UnloadSceneAsync(_loadedScene.AsIndex);
			_loadedScene = SceneRef.None;
		}
		base.Shutdown();

		FindObjectOfType<UI_Menu>().OnClickCancelButton();
	}
}