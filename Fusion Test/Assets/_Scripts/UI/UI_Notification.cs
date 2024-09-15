using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class UI_Notification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    void Start()
    {
        Hide(0);
    }

    public void Show(string message, float delay = 0)
    {
        _text.text = message;
        gameObject.SetActive(true);

        if (delay != 0) Hide(delay);
    }

    async void Hide(float delay)
    {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        if (!gameObject) return;
        gameObject.SetActive(false);
    }
}

