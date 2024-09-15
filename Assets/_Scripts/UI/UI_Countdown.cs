using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Countdown : MonoBehaviour
{
    TextMeshProUGUI text;

    public void ShowText(string value)
    {
        if (text == null)
            text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = value;
    }
}
