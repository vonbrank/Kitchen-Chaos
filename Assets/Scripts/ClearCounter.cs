using UnityEngine;

namespace DefaultNamespace
{
    public class ClearCounter : MonoBehaviour
    {
        public void Interact()
        {
            Debug.Log($"Interact {transform}!");
        }
    }
}