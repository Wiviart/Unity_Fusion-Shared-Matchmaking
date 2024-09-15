using System;
using System.Collections.Generic;
using FusionHelpers;
using UnityEngine;
using UnityEngine.UI;

public class ReadyUpManager : MonoBehaviour
{
    private float _delay;
    [SerializeField] private GameObject _disconnectPrompt;
    [SerializeField] Button yesButton;

    private void Start()
    {
        _disconnectPrompt.SetActive(false);
        yesButton.onClick.AddListener(AttemptDisconnect);
    }

    void AttemptDisconnect()
    {
        GameManager.Instance.DisconnectByPrompt = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowDisconnectPrompt();
        }
    }

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

        int  readyCount = 0;
        foreach (var fusionPlayer in allPlayers)
        {
            Player player = (Player)fusionPlayer;
            if (player.Ready)
            {
                readyCount++;
            }
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

        bool allPlayersReady = readyCount == ConstVariables.PLAYER_COUNT;

        // _disconnectInfoText.SetActive(!allPlayersReady);
        // _readyupInfoText.SetActive(!allPlayersReady && playerCount > 1);

        if (allPlayersReady)
        {
            _delay = 2.0f;
            onAllPlayersReady();
        }
    }

    internal void ShowDisconnectPrompt()
    {
        if (_disconnectPrompt.activeSelf) return;
        _disconnectPrompt.SetActive(true);
    }
}