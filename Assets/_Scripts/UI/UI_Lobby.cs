using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : NetworkBehaviour
{
    [SerializeField] Button readyButton;

    private void Start()
    {
        readyButton.onClick.AddListener(OnReadyButtonClicked);
    }

    private void OnReadyButtonClicked()
    {
        FindObjectOfType<GameManager>().OnReadyButtonClicked();
    }
}
