using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Button _findButton;
    [SerializeField] private GameObject _roomPanel;
    [SerializeField] private Button _cancelButton;

    void Start()
    {
        _findButton.onClick.AddListener(OnClickFindGameButton);
        _cancelButton.onClick.AddListener(OnClickCancelButton);

        OnClickCancelButton();
    }

    internal void OnClickCancelButton()
    {
        _menuPanel.SetActive(true);
        _roomPanel.SetActive(false);
    }

    private void OnClickFindGameButton()
    {
        _menuPanel.SetActive(false);
        _roomPanel.SetActive(true);
    }
}
