using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Utils
{
    public static class SceneLoader
    {
        public enum Scene
        {
            MainMenuScene,
            LoadingScene,
            GameScene,
            LobbyScene,
            CharacterSelectScene,
            Max,
        }

        private static Scene currentTargetScene;

        public static void Load(Scene scene)
        {
            currentTargetScene = scene;

            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

        public static void LoadNetwork(Scene scene)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Single);
        }

        public static void LoadSceneCallBack()
        {
            SceneManager.LoadScene(currentTargetScene.ToString());
        }
    }
}