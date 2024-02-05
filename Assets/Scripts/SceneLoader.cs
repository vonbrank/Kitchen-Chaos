using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public static class SceneLoader
    {
        public enum Scene
        {
            MainMenuScene,
            LoadingScene,
            GameScene
        }

        private static Scene currentTargetScene;

        public static void Load(Scene scene)
        {
            currentTargetScene = scene;

            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

        public static void LoadSceneCallBack()
        {
            SceneManager.LoadScene(currentTargetScene.ToString());
        }
    }
}