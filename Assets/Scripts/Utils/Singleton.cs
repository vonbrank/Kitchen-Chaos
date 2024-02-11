using Unity.Netcode;
using UnityEngine;

namespace Utils
{
    public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            Instance = this as T;
        }
    }

    public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                base.Awake();
            }
        }
    }

    public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }

    public abstract class NetworkStaticInstance<T> : NetworkBehaviour where T : NetworkBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            Instance = this as T;
        }
    }

    public abstract class NetworkSingleton<T> : NetworkStaticInstance<T> where T : NetworkBehaviour
    {
        protected override void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                base.Awake();
            }
        }
    }

    public abstract class NetworkPersistentSingleton<T> : NetworkSingleton<T> where T : NetworkBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}