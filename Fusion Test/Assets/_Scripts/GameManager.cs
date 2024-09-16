using UnityEngine;
using Fusion;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
	public static GameManager Instance { get; private set; }
	public Player _playerPrefab;
	public enum PlayState { LOBBY, LEVEL, TRANSITION }
	[Networked] public PlayState currentPlayState { get; set; }
	public UI_Countdown countdownPrefab;
	public bool DisconnectByPrompt { get; set; }
	List<Player> AllPlayers = new List<Player>();
	bool allReady = false;
	PlayerRef localPlayer;

	public const ShutdownReason ShutdownReason_GameAlreadyRunning = (ShutdownReason)100;
	UI_Countdown countdown;

	public override void Spawned()
	{
		base.Spawned();

		Instance = this;
		DontDestroyOnLoad(gameObject);

		if (Object.HasStateAuthority)
		{
			ChangeState(PlayState.LOBBY);
		}
		else if (currentPlayState != PlayState.LOBBY)
		{
			Debug.Log("Rejecting Player, game is already running!");
		}

		countdown = Instantiate(countdownPrefab);
		countdown.ShowText("");
	}

	private void ChangeState(PlayState state)
	{
		currentPlayState = state;

		switch (state)
		{
			case PlayState.LOBBY:
				LoadLevel(ConstVariables.LOBBY);
				StartCoroutine(SpawnPlayer());
				break;
			case PlayState.LEVEL:
				LoadLevel(ConstVariables.GAMEPLAY);
				break;
		}
	}

	private IEnumerator SpawnPlayer()
	{
		SpawnPositions spawns = FindObjectOfType<SpawnPositions>();
		while (!spawns)
		{
			spawns = FindObjectOfType<SpawnPositions>();
			yield return null;
		}

		Debug.Log($"I am {Runner.LocalPlayer} and I am {(Runner.IsServer ? "Server" : "Master")}. The Session StateAuth is: {Object.StateAuthority} - Assigning to PlayerRef {localPlayer}");

		Player player = Runner.Spawn(_playerPrefab);
		Runner.SetPlayerObject(Runner.LocalPlayer, player.GetComponent<NetworkObject>());
		player.Init(spawns);
		AllPlayers.Add(player);
	}

	public void SetLocalPlayer(PlayerRef player)
	{
		localPlayer = player;
	}

	void Update()
	{
		ReadyUpManager readyUpManager = FindObjectOfType<ReadyUpManager>();
		if (readyUpManager != null && AllPlayers.Count > 0)
			readyUpManager.UpdateUI(currentPlayState, AllPlayers, OnAllPlayersReady);

		// if (_players.Count == ConstVariables.PLAYER_COUNT && !isCountingDown)
		// {
		// 	StartCoroutine(CountdownToReady());
		// }
	}

	public void OnReadyButtonClicked()
	{
		Player player = Runner.GetPlayerObject(Runner.LocalPlayer).GetComponent<Player>();
		player.ToggleReady();
	}

	public void OnAllPlayersReady()
	{
		if (allReady) return;
		allReady = true;
		Debug.Log("All players are ready");

		Runner.SessionInfo.IsOpen = false;
		Runner.SessionInfo.IsVisible = false;

		StopAllCoroutines();

		StartCoroutine(CountdownToStart());
	}

	IEnumerator CountdownToStart()
	{
		int countdownValue = ConstVariables.WAIT_TIME;

		countdown.ShowText(countdownValue.ToString());

		while (countdownValue > 0)
		{
			yield return new WaitForSeconds(1);
			countdownValue--;
			countdown.ShowText(countdownValue.ToString());
		}

		Destroy(countdown.gameObject);
		ChangeState(PlayState.LEVEL);
	}

	bool isCountingDown = false;

	IEnumerator CountdownToReady()
	{
		isCountingDown = true;
		int countdownValue = ConstVariables.WAIT_TIME_TO_READY;

		countdown.ShowText(countdownValue.ToString());

		while (countdownValue > 0)
		{
			yield return new WaitForSeconds(1);
			countdownValue--;
			countdown.ShowText(countdownValue.ToString());
		}

		foreach (Player p in AllPlayers.Cast<Player>())
		{
			p.Ready = true;
		}
	}

	private void LoadLevel(int nextLevelIndex)
	{
		if (!Object.HasStateAuthority) return;

		Runner.GetLevelManager().LoadLevel(nextLevelIndex);
	}
}