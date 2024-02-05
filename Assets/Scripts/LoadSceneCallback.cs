using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LoadSceneCallback : MonoBehaviour
    {
        private bool isFirstUpdate = true;

        private void Update()
        {
            if (isFirstUpdate)
            {
                isFirstUpdate = false;
                SceneLoader.LoadSceneCallBack();
            }
        }
    }
}