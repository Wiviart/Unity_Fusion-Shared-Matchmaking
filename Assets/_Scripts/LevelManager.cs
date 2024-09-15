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
	public ReadyUpManager readyUpManager
	{
		get
		{
			return FindObjectOfType<ReadyUpManager>();
		}
	}

	public void LoadLevel(int nextLevelIndex)
	{
		if (_loadedScene.IsValid)
		{
			Debug.Log($"LevelManager.UnloadLevel(); _loadedScene={_loadedScene}");
			Runner.UnloadScene(_loadedScene);
			//UnloadScene();
			_loadedScene = SceneRef.None;
		}
		Debug.Log($"LevelManager.LoadLevel({nextLevelIndex});");

		Runner.LoadScene(SceneRef.FromIndex(nextLevelIndex), new LoadSceneParameters(LoadSceneMode.Additive), true);
		_loadedScene = SceneRef.FromIndex(nextLevelIndex);
	}

}