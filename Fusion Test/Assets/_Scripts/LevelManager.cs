using System;
using System.Collections;
using Fusion;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelManager : NetworkSceneManagerDefault
{
	public static LevelManager Instance { get; private set; }
	private SceneRef _loadedScene = SceneRef.None;
	[SerializeField] private GameManager _gameManagerPrefab;
	[SerializeField] private GameObject _notificationPrefab;
	private UI_Notification _notification;
	private FusionLauncher.ConnectionStatus _status = FusionLauncher.ConnectionStatus.Disconnected;

	void Awake()
	{
		Instance = this;
	}

	public void LoadLevel(int nextLevelIndex)
	{
		if (_loadedScene.IsValid)
		{
			Debug.Log($"LevelManager.UnloadLevel(); _loadedScene={_loadedScene}");
			Runner.UnloadScene(_loadedScene);
			_loadedScene = SceneRef.None;
		}
		Debug.Log($"LevelManager.LoadLevel({nextLevelIndex});");

		Runner.LoadScene(SceneRef.FromIndex(nextLevelIndex), new LoadSceneParameters(LoadSceneMode.Single), true);
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
	}

	public void StartLauncher(string region, string room, LevelManager _levelManager)
	{
		FusionLauncher.Launch(GameMode.Shared, region, room, _gameManagerPrefab, _levelManager, OnConnectionStatusUpdate);
	}

	private void OnConnectionStatusUpdate(
	  NetworkRunner runner,
	  FusionLauncher.ConnectionStatus status,
	  string reason)
	{
		if (!this) return;

		_status = status;
		UpdateNotification();
	}

	private void UpdateNotification()
	{
		if (!_notification)
		{
			var obj = Instantiate(_notificationPrefab, transform);
			_notification = obj.GetComponentInChildren<UI_Notification>();
		}

		switch (_status)
		{
			case FusionLauncher.ConnectionStatus.Disconnected:
				_notification.Show("Disconnected!", 3);
				break;
			case FusionLauncher.ConnectionStatus.Failed:
				_notification.Show("Failed!", 1);
				break;
			case FusionLauncher.ConnectionStatus.Connecting:
				_notification.Show("Connecting");
				break;
			case FusionLauncher.ConnectionStatus.Connected:
				_notification.Show("Connected", 1);
				break;
			case FusionLauncher.ConnectionStatus.Loading:
				_notification.Show("Loading!");
				break;
			case FusionLauncher.ConnectionStatus.Loaded:
				break;
		}
	}
}