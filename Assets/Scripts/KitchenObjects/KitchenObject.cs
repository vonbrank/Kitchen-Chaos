using System;
using Managers;
using ScriptableObjects;
using Unity.Netcode;
using UnityEngine;

namespace KitchenObjects
{
    public class KitchenObject : NetworkBehaviour
    {
        [SerializeField] private KitchenObjectItem kitchenObjectItem;
        public KitchenObjectItem KitchenObjectItem => kitchenObjectItem;

        private IKitchenObjectParent kitchenObjectParent;

        public IKitchenObjectParent KitchenObjectParent => kitchenObjectParent;

        private FollowTransform followTransform;

        protected virtual void Awake()
        {
            followTransform = GetComponent<FollowTransform>();
        }

        public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
        {
            SetKitchenObjectParentServerRpc(kitchenObjectParent.NetworkObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
        {
            SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObjectReference);
        }

        [ClientRpc]
        private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
        {
            kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
            IKitchenObjectParent kitchenObjectParent =
                kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

            if (this.kitchenObjectParent is not null)
            {
                this.kitchenObjectParent.ClearKitchenObject();
            }

            this.kitchenObjectParent = kitchenObjectParent;

            if (kitchenObjectParent.HasKitchenObject())
            {
                Debug.LogError("kitchen object parent already has a kitchen object.");
            }

            kitchenObjectParent.SetKitchenObject(this);

            followTransform.TargetTransform = kitchenObjectParent.KitchenObjectFollowTransform;
            // transform.parent = kitchenObjectParent.KitchenObjectFollowTransform;
            // transform.localPosition = Vector3.zero;
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public void ClearKitchenObjectOnParent()
        {
            kitchenObjectParent.ClearKitchenObject();
        }

        public static void SpawnKitchenObject(KitchenObjectItem kitchenObjectItem,
            IKitchenObjectParent kitchenObjectParent)
        {
            KitchenGameMultiplayerManager.Instance.SpawnKitchenObject(kitchenObjectItem, kitchenObjectParent);
        }

        public static void DestroyKitchenObject(KitchenObject kitchenObject)
        {
            KitchenGameMultiplayerManager.Instance.DestroyKitchenObject(kitchenObject);
        }
    }
}