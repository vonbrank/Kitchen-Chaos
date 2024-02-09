using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TestCharacterSelectUI : MonoBehaviour
    {
        [SerializeField] private Button readyButton;

        private void OnEnable()
        {
            readyButton.onClick.AddListener(HandleReadyButtonClick);
        }

        private void OnDisable()
        {
            readyButton.onClick.RemoveListener(HandleReadyButtonClick);
        }

        private void HandleReadyButtonClick()
        {
            PlayerSelectReadyManager.Instance.PlayerSelectReady();
        }
    }
}