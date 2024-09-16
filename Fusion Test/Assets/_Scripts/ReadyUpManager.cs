using System;
using System.Collections.Generic;

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

    public void UpdateUI(GameManager.PlayState playState, List<Player> allPlayers, Action onAllPlayersReady)
    {
        int  readyCount = 0;
        foreach (var fusionPlayer in allPlayers)
        {
            Player player = (Player)fusionPlayer;
            if (player.Ready)
            {
                readyCount++;
            }
        }

        bool allPlayersReady = readyCount == ConstVariables.PLAYER_COUNT;

        if (allPlayersReady)
        {
            onAllPlayersReady();
        }
    }

    internal void ShowDisconnectPrompt()
    {
        if (_disconnectPrompt.activeSelf) return;
        _disconnectPrompt.SetActive(true);
    }
}