using System;
using Managers;
using UnityEngine;

namespace UI
{
    public class CharacterColorSelectUI : MonoBehaviour
    {
        [SerializeField] private Transform colorButtonsTransform;
        [SerializeField] private CharacterSelectColorButtonUI colorButtonTemplate;

        private void Start()
        {
            for (int i = 0; i < KitchenGameMultiplayerManager.Instance.PlayerColorList.Count; i++)
            {
                CharacterSelectColorButtonUI characterSelectColorButtonUI =
                    Instantiate(colorButtonTemplate, colorButtonsTransform);
                characterSelectColorButtonUI.ColorIndex = i;
                characterSelectColorButtonUI.gameObject.SetActive(true);
            }
        }
    }
}