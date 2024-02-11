using System;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterSelectColorButtonUI : MonoBehaviour
    {
        [SerializeField] private GameObject selectedGameObject;
        private Image image;
        private Button button;
        public int ColorIndex { get; set; }

        private void Awake()
        {
            image = GetComponent<Image>();
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            KitchenGameMultiplayerManager.Instance.OnPlayerDataNetworkListChanged += HandlePlayerDataChanged;
            button.onClick.AddListener(HandleButtonClick);
        }


        private void OnDisable()
        {
            KitchenGameMultiplayerManager.Instance.OnPlayerDataNetworkListChanged -= HandlePlayerDataChanged;
            button.onClick.RemoveListener(HandleButtonClick);
        }

        private void HandlePlayerDataChanged(object sender, EventArgs e)
        {
            UpdateVisual();
        }

        private void Start()
        {
            UpdateVisual();
        }

        private void HandleButtonClick()
        {
            KitchenGameMultiplayerManager.Instance.ChangeLocalPlayerColor(ColorIndex);
        }

        private void UpdateVisual()
        {
            image.color = KitchenGameMultiplayerManager.Instance.GetPlayerColor(ColorIndex);
            PlayerData playerData = KitchenGameMultiplayerManager.Instance.GetLocalPlayerData();
            if (ColorIndex == playerData.ColorIndex)
            {
                selectedGameObject.SetActive(true);
            }
            else
            {
                selectedGameObject.SetActive(false);
            }
        }
    }
}