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
        Hide();
    }

    public void Show(string message)
    {
        _text.text = message;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
