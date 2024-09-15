using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using FusionHelpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Room : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _regionDropdown;
    [SerializeField] private TMP_InputField _roomName;
    [SerializeField] private Button _enterRoomButton;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private FusionSession _gameManagerPrefab;

    [SerializeField] private UI_Notification _notification;


    private FusionLauncher.ConnectionStatus _status = FusionLauncher.ConnectionStatus.Disconnected;

    private void Start()
    {
        _enterRoomButton.onClick.AddListener(EnterRoom);
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private void EnterRoom()
    {
        string region = string.Empty;
        if (_regionDropdown.value > 0)
        {
            region = _regionDropdown.options[_regionDropdown.value].text;
            region = region.Split(" (")[0];
        }

        FusionLauncher.Launch(GameMode.Shared, region, _roomName.text, _gameManagerPrefab, _levelManager, OnConnectionStatusUpdate);
    }

    private void OnConnectionStatusUpdate(
        NetworkRunner runner, FusionLauncher.ConnectionStatus status, string reason)
    {
        if (!this) return;

        if (status != _status)
        {
            switch (status)
            {
                case FusionLauncher.ConnectionStatus.Disconnected:
                    // ErrorBox.Show("Disconnected!", reason, () => { });
                    break;
                case FusionLauncher.ConnectionStatus.Failed:
                    // ErrorBox.Show("Error!", reason, () => { });
                    break;
            }
        }

        _status = status;
        UpdateUI();
    }

    private void UpdateUI()
    {
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
                gameObject.SetActive(false);
                break;
            case FusionLauncher.ConnectionStatus.Loading:
                _notification.Show("Loading!");
                break;
            case FusionLauncher.ConnectionStatus.Loaded:
                break;
        }
    }
}
