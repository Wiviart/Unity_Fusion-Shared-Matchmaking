using System;
using System.Collections;
using Fusion;
using TMPro;
using UnityEngine;

namespace Wiviart.UI
{
    public class FindGameButton : ButtonHandler
    {
        bool isSearching = false;

        protected override void OnClick()
        {
            if (isSearching)
            {
                Disconnect();
                isSearching = false;
            }
            else
            {
                Connect();
                isSearching = true;
            }
        }

        void Connect()
        {
            var gameMode = GameMode.Shared;
            var roomName = "Default";
            var region = "";
            FusionLauncher.Instance.LaunchGame(gameMode, region, roomName);

            StartCoroutine(CountWaitingTime());
        }

        void Disconnect()
        {
            StopAllCoroutines();
            var text = GetComponentInChildren<TextMeshProUGUI>();
            text.text = "FIND";
            FusionLauncher.Instance.Disconnect();
        }

        private IEnumerator CountWaitingTime()
        {
            var text = GetComponentInChildren<TextMeshProUGUI>();
            var timer = 0;
            while (true)
            {
                timer++;
                var minutes = timer / 60;
                var seconds = timer % 60;
                text.text = minutes.ToString("00") + ":" + seconds.ToString("00");
                yield return new WaitForSeconds(1);
            }
        }
    }
}