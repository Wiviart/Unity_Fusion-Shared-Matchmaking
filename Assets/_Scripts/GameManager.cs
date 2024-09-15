using UnityEngine;
using Fusion;
using FusionHelpers;
using System.Collections;

public class GameManager : FusionSession
{
	public static GameManager Instance { get; private set; }
	public enum PlayState { LOBBY, LEVEL, TRANSITION }
	[Networked] public PlayState currentPlayState { get; set; }
	public UI_Countdown countdownPrefab;

	public override void Spawned()
	{
		base.Spawned();
		Runner.RegisterSingleton(this);
		DontDestroyOnLoad(gameObject);

		if (Object.HasStateAuthority)
		{
			LoadLevel(ConstVariables.LOBBY);
		}
		// else if (currentPlayState != PlayState.LOBBY)
		// {
		// 	Debug.Log("Rejecting Player, game is already running!");
		// 	_restart = true;
		// }
	}

	void Update()
	{
		ReadyUpManager readyUpManager = FindObjectOfType<ReadyUpManager>();
		if (readyUpManager != null)
			readyUpManager.UpdateUI(currentPlayState, AllPlayers, OnAllPlayersReady);
	}

	protected override void OnPlayerAvatarAdded(FusionPlayer fusionPlayer)
	{

	}

	protected override void OnPlayerAvatarRemoved(FusionPlayer fusionPlayer)
	{

	}

	public void OnReadyButtonClicked()
	{
		if (Object.HasStateAuthority)
		{
			Player player = Runner.GetPlayerObject(Runner.LocalPlayer).GetComponent<Player>();
			player.ToggleReady();
		}
	}

	bool allReady = false;
	public void OnAllPlayersReady()
	{
		if (allReady) return;
		allReady = true;
		Debug.Log("All players are ready");

		// close and hide the session from matchmaking / lists. this demo does not allow late join.
		Runner.SessionInfo.IsOpen = false;
		Runner.SessionInfo.IsVisible = false;

		// Reset stats and transition to level.
		StartCoroutine(CountdownToStart());
	}

	IEnumerator CountdownToStart()
	{
		UI_Countdown countdown = Instantiate(countdownPrefab);
		countdown.Init(ConstVariables.WAIT_TIME);
		yield return new WaitForSeconds(ConstVariables.WAIT_TIME);
		LoadLevel(ConstVariables.GAMEPLAY);
	}

	private void LoadLevel(int nextLevelIndex)
	{
		if (Object.HasStateAuthority)
			Runner.GetLevelManager().LoadLevel(nextLevelIndex);
	}
}