using System.Collections;
using System.Threading.Tasks;
using Fusion;

using UnityEngine;

public class Player : NetworkBehaviour
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

	public void Init(SpawnPositions spawner)
	{
		gameObject.name = $"Player {Id}";
		transform.position = spawner.GetSpawnPosition().position;
	}

	public void ToggleReady()
	{
		Ready = !Ready;

		_readyUI.SetActive(Ready);

		Debug.Log("Player " + Id + " is " + (Ready ? "ready" : "not ready"));
	}
}