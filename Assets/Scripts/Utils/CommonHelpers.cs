using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class CommonHelpers
    {
        public static void HideVisual(this List<Transform> transforms)
        {
            foreach (Transform transform in transforms)
            {
                transform.gameObject.SetActive(false);
            }
        }

        public static void HideVisual(this Transform[] transforms)
        {
            foreach (Transform transform in transforms)
            {
                transform.gameObject.SetActive(false);
            }
        }

        public static void ShowVisual(this List<Transform> transforms)
        {
            foreach (Transform transform in transforms)
            {
                transform.gameObject.SetActive(true);
            }
        }

        public static void ShowVisual(this Transform[] transforms)
        {
            foreach (Transform transform in transforms)
            {
                transform.gameObject.SetActive(true);
            }
        }
    }
}