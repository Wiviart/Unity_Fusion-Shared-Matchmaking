using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Countdown : MonoBehaviour
{
    TextMeshProUGUI text;

    public void Init(float value)
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = value.ToString();
        StartCoroutine(Countdown(value));
    }

    private IEnumerator Countdown(float value)
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            value--;
            if (value <= 0)
            {
                value = 0;
                break;
            }
            text.text = value.ToString();
        }
    }
}
