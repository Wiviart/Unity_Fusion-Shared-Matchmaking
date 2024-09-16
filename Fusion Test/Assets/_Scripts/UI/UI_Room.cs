using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Room : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _regionDropdown;
    [SerializeField] private TMP_InputField _roomName;
    [SerializeField] private Button _enterRoomButton;

    private LevelManager _levelManager;

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

        LevelManager.Instance.StartLauncher(region, _roomName.text, _levelManager);
    }
}
