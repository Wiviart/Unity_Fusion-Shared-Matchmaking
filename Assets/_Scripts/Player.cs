using System.Collections;
using System.Threading.Tasks;
using Fusion;
using FusionHelpers;
using UnityEngine;

public class Player : FusionPlayer
{
	[Networked] public bool Ready { get; set; }
	[SerializeField] private GameObject _readyPrefab;
	GameObject _readyUI;

	public override void Spawned()
	{
		base.Spawned();

		Ready = false;

		_readyUI = Instantiate(_readyPrefab, transform.position, Quaternion.identity);
		_readyUI.SetActive(Ready);
	}

	public override void InitNetworkState()
	{
		gameObject.name = $"Player {PlayerIndex}";
	}

	public void ToggleReady()
	{
		Ready = !Ready;

		_readyUI.SetActive(Ready);

		Debug.Log("Player " + PlayerId + " is " + (Ready ? "ready" : "not ready"));
	}
}