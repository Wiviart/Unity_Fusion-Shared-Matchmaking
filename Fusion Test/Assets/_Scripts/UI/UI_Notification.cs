using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Notification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    public void Show(string message, float delay = 0)
    {
        _text.text = message;
        gameObject.SetActive(true);

        if (delay == 0) return;
        StartCoroutine(Hide(delay));
    }

    IEnumerator Hide(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        gameObject.SetActive(false);
    }
}

