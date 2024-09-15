using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    [SerializeField] Button readyButton;
    [SerializeField] Button quitButton;

    private void Start()
    {
        readyButton.onClick.AddListener(OnReadyButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnQuitButtonClicked()
    {
        GameManager.Instance.OnQuitButtonClicked();
    }

    private void OnReadyButtonClicked()
    {
        GameManager.Instance.OnReadyButtonClicked();
    }
}
