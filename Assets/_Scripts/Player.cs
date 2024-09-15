using Fusion;
using FusionHelpers;
using UnityEngine;

public class Player : FusionPlayer
{
	[Networked] public bool ready { get; set; }

	public override void Spawned()
	{
		base.Spawned();

		ready = false;

	}

	public override void InitNetworkState()
	{

	}

	public void ToggleReady()
	{
		ready = !ready;

		Debug.Log("Player " + PlayerId + " is " + (ready ? "ready" : "not ready"));
	}

	public void ResetReady()
	{
		ready = false;
	}
}