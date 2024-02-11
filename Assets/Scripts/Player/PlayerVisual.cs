using System;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerVisual : MonoBehaviour
    {
        [SerializeField] private MeshRenderer headMeshRenderer;
        [SerializeField] private MeshRenderer bodyMeshRender;

        private Material material;

        private void Awake()
        {
            material = new Material(headMeshRenderer.material);
            headMeshRenderer.material = material;
            bodyMeshRender.material = material;
        }

        public void SetPlayerColor(Color color)
        {
            material.color = color;
        }
    }
}