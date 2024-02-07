using System;
using UnityEngine;

namespace KitchenObjects
{
    public class FollowTransform : MonoBehaviour
    {
        public Transform TargetTransform { private get; set; }

        private void LateUpdate()
        {
            if (TargetTransform is not null)
            {
                transform.position = TargetTransform.position;
                transform.rotation = TargetTransform.rotation;
            }
        }
    }
}