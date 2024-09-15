using UnityEngine;
using Fusion;
using FusionHelpers;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

public class GameManager : FusionSession
{
	public static GameManager Instance { get; private set; }
	public enum PlayState { LOBBY, LEVEL, TRANSITION }
	[Networked] public PlayState currentPlayState { get; set; }
	public UI_Countdown countdownPrefab;
	private bool _restart;
	public bool DisconnectByPrompt { get; set; }
	public const ShutdownReason ShutdownReason_GameAlreadyRunning = (ShutdownReason)100;
	UI_Countdown countdown;

	public override void Spawned()
	{
		base.Spawned();

		Instance = this;
		Runner.RegisterSingleton(this);

		if (Object.HasStateAuthority)
		{
			LoadLevel(ConstVariables.LOBBY);
		}
		else if (currentPlayState != PlayState.LOBBY)
		{
			Debug.Log("Rejecting Player, game is already running!");
			_restart = true;
		}

		countdown = Instantiate(countdownPrefab);
		countdown.ShowText("");
	}

	void Update()
	{
		ReadyUpManager readyUpManager = FindObjectOfType<ReadyUpManager>();
		if (readyUpManager != null)
			readyUpManager.UpdateUI(currentPlayState, AllPlayers, OnAllPlayersReady);

		if (_players.Count == ConstVariables.PLAYER_COUNT && !isCountingDown)
		{
			StartCoroutine(CountdownToReady());
		}

		// if (_restart || DisconnectByPrompt)
		// {
		// 	Restart(_restart ? ShutdownReason_GameAlreadyRunning : ShutdownReason.Ok);
		// 	_restart = false;

		// 	DisconnectByPrompt = true;
		// }
	}

	protected override void OnPlayerAvatarAdded(FusionPlayer fusionPlayer)
	{
		StartCoroutine(SetPlayerSpawnPosition((Player)fusionPlayer));
	}

	protected override void OnPlayerAvatarRemoved(FusionPlayer fusionPlayer)
	{

	}

	public void OnReadyButtonClicked()
	{
		Player player = GetPlayer<Player>(Runner.LocalPlayer);
		player.ToggleReady();
	}

	bool allReady = false;
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
		LoadLevel(ConstVariables.GAMEPLAY);

		yield return null;

		foreach (Player p in AllPlayers.Cast<Player>())
		{
			StartCoroutine(SetPlayerSpawnPosition(p));
		}
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

	IEnumerator SetPlayerSpawnPosition(Player player)
	{
		var spawns = FindObjectOfType<SpawnPositions>();
		while (spawns == null)
		{
			spawns = FindObjectOfType<SpawnPositions>();
			yield return null;
		}

		foreach (Player p in AllPlayers.Cast<Player>())
		{
			if (p.PlayerIndex == player.PlayerIndex)
			{
				p.GetComponent<Transform>().position = spawns.GetSpawnPosition(player.PlayerIndex);
				break;
			}
		}
	}

	internal void OnQuitButtonClicked()
	{
		DisconnectByPrompt = true;
	}

	public void Restart(ShutdownReason shutdownReason)
	{
		if (!Runner.IsShutdown)
		{
			// Calling with destroyGameObject false because we do this in the OnShutdown callback on FusionLauncher
			Runner.Shutdown(false, shutdownReason);
			_restart = false;
		}
	}

	protected override void MaybeSpawnNextAvatar()
	{
		foreach (KeyValuePair<int, PlayerRef> refByIndex in playerRefByIndex)
		{
			if (Runner.IsServer || (Runner.Topology == Topologies.Shared && refByIndex.Value == Runner.LocalPlayer))
			{
				if (!_players.TryGetValue(refByIndex.Value, out _))
				{
					Debug.Log($"I am State Auth for Player Index {refByIndex.Key} - Spawning Avatar");
					Runner.SpawnAsync(_playerPrefab, Vector3.zero, Quaternion.identity, refByIndex.Value, (runner, o) =>
					{
						Runner.SetPlayerObject(refByIndex.Value, o);
						if (o.TryGetComponent<FusionPlayer>(out var player))
						{
							player.NetworkedPlayerIndex = refByIndex.Key;
							player.InitNetworkState();
						}
					});
				}
			}
		}
	}
}