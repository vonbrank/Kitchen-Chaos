using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private void OnEnable()
        {
            playButton.onClick.AddListener(HandlePlay);
            quitButton.onClick.AddListener(HandleQuit);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(HandlePlay);
            quitButton.onClick.RemoveListener(HandleQuit);
        }

        private void Start()
        {
            playButton.Select();
        }

        private void HandlePlay()
        {
            SceneLoader.Load(SceneLoader.Scene.GameScene);
        }

        private void HandleQuit()
        {
            Application.Quit();
        }
    }
}