using System;
using Managers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Player
{
    public class CharacterSelectPlayer : MonoBehaviour
    {
        [SerializeField] private int playerIndex;
        [SerializeField] private Transform[] visualGameObjects;
        [SerializeField] private GameObject readyTextGameObject;
        [SerializeField] private PlayerVisual playerVisual;
        [SerializeField] private Button kickOffButton;

        private void OnEnable()
        {
            KitchenGameMultiplayerManager.Instance.OnPlayerDataNetworkListChanged += HandlePlayerDataNetworkListChanged;
            PlayerSelectReadyManager.Instance.OnPlayerReadyChanged += HandlePlayerReadyChanged;
            kickOffButton.onClick.AddListener(HandleKickOffButtonClick);
        }


        private void OnDisable()
        {
            KitchenGameMultiplayerManager.Instance.OnPlayerDataNetworkListChanged -= HandlePlayerDataNetworkListChanged;
            PlayerSelectReadyManager.Instance.OnPlayerReadyChanged -= HandlePlayerReadyChanged;
            kickOffButton.onClick.RemoveListener(HandleKickOffButtonClick);
        }

        private void UpdatePlayer()
        {
            if (KitchenGameMultiplayerManager.Instance.IsPlayerConnectedByIndex(playerIndex))
            {
                PlayerData playerData = KitchenGameMultiplayerManager.Instance.GetPlayerDataByIndex(playerIndex);
                readyTextGameObject.SetActive(PlayerSelectReadyManager.Instance.IsPlayerReady(playerData.ClientId));
                playerVisual.SetPlayerColor(
                    KitchenGameMultiplayerManager.Instance.GetPlayerColor(playerData.ColorIndex));
                kickOffButton.gameObject.SetActive(NetworkManager.Singleton.IsServer &&
                                                   playerData.ClientId != NetworkManager.ServerClientId);
                Show();
            }
            else
            {
                Hide();
                readyTextGameObject.SetActive(false);
                kickOffButton.gameObject.SetActive(false);
            }
        }

        private void HandlePlayerDataNetworkListChanged(object sender, EventArgs e)
        {
            UpdatePlayer();
        }


        private void HandlePlayerReadyChanged(object sender, EventArgs e)
        {
            UpdatePlayer();
        }

        private void HandleKickOffButtonClick()
        {
            PlayerData playerData = KitchenGameMultiplayerManager.Instance.GetPlayerDataByIndex(playerIndex);
            KitchenGameMultiplayerManager.Instance.KickOffPlayer(playerData.ClientId);
        }

        private void Start()
        {
            Hide();

            UpdatePlayer();
        }

        private void Hide()
        {
            visualGameObjects.HideVisual();
        }

        private void Show()
        {
            visualGameObjects.ShowVisual();
        }
    }
}