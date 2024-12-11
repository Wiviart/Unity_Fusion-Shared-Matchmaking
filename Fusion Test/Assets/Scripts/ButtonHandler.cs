using System;
using UnityEngine;
using UnityEngine.UI;

namespace Wiviart.UI
{
    public abstract class ButtonHandler : MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        protected abstract void OnClick();
    }
}