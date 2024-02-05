using System;
using UnityEngine;

namespace Utils
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera mainCamera;

        private enum Mode
        {
            LookAt,
            LookAtInverted,
            CameraForward,
            CameraForwardInverted
        }

        [SerializeField] private Mode mode;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            switch (mode)
            {
                case Mode.LookAt:
                    transform.LookAt(mainCamera.transform);
                    break;
                case Mode.LookAtInverted:
                    transform.LookAt(transform.position + (transform.position - mainCamera.transform.position));
                    break;
                case Mode.CameraForward:
                    transform.forward = mainCamera.transform.forward;
                    break;
                case Mode.CameraForwardInverted:
                    transform.forward = -mainCamera.transform.forward;
                    break;
            }
        }
    }
}