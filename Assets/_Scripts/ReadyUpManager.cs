using System;
using System.Collections.Generic;
using FusionHelpers;
using UnityEngine;

public class ReadyUpManager : MonoBehaviour
{
    private float _delay;

    public void UpdateUI(GameManager.PlayState playState, IEnumerable<FusionPlayer> allPlayers, Action onAllPlayersReady)
    {
        if (_delay > 0)
        {
            _delay -= Time.deltaTime;
            return;
        }

        // if (playState != GameManager.PlayState.LOBBY)
        // {
        //     // foreach (ReadyupIndicator ui in _readyUIs.Values)
        //     //     LocalObjectPool.Release(ui);
        //     // _readyUIs.Clear();
        //     gameObject.SetActive(false);
        //     return;
        // }

        // gameObject.SetActive(true);

        int playerCount = 0, readyCount = 0;
        foreach (FusionPlayer fusionPlayer in allPlayers)
        {
            Player player = (Player)fusionPlayer;
            if (player.ready)
                readyCount++;
            playerCount++;
        }

        // foreach (ReadyupIndicator ui in _readyUIs.Values)
        // {
        //     ui.Dirty();
        // }

        // foreach (FusionPlayer fusionPlayer in allPlayers)
        // {
        //     Player player = (Player)fusionPlayer;

        //     ReadyupIndicator indicator;
        //     if (!_readyUIs.TryGetValue(player.PlayerId, out indicator))
        //     {
        //         indicator = LocalObjectPool.Acquire(_readyPrefab, Vector3.zero, Quaternion.identity, _readyUIParent);
        //         _readyUIs.Add(player.PlayerId, indicator);
        //     }
        //     if (indicator.Refresh(player))
        //         _audioEmitter.PlayOneShot();
        // }

        bool allPlayersReady = readyCount == playerCount;

        // _disconnectInfoText.SetActive(!allPlayersReady);
        // _readyupInfoText.SetActive(!allPlayersReady && playerCount > 1);

        if (allPlayersReady)
        {
            _delay = 2.0f;
            onAllPlayersReady();
        }
    }
}