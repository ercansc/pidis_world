using UnityEngine;

namespace UnityHelpers
{
    public class MBSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region FIELDS & MEMBERS

        [SerializeField]
        private static bool m_bDontDestroyOnLoad;
        private static T s_instance;
        private static bool s_bApplicationIsQuitting;

        private static object s_oLock = new object();
        #endregion

        #region PROPERTIES
        public static T Instance
        {
            get
            {
                if (s_bApplicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                        "' already destroyed on application quit." +
                        " Won't create again - returning null.");
                    return null;
                }

                lock (s_oLock)
                {
                    if (s_instance == null)
                    {
                        s_instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went really wrong " +
                                " - there should never be more than 1 singleton!" +
                                " Reopening the scene might fix it.");
                            return s_instance;
                        }

                        if (s_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            s_instance = singleton.AddComponent<T>();
                            singleton.name = "(singleton) " + typeof(T).ToString();

                            if (m_bDontDestroyOnLoad)
                                DontDestroyOnLoad(singleton);

                            Debug.Log("[Singleton] An instance of " + typeof(T) +
                                " is needed in the scene, so '" + singleton +
                                "' was created.");
                        }
                        else
                        {
                            Debug.Log("[Singleton] Using instance already created: " +
                                s_instance.gameObject.name);
                        }
                    }

                    return s_instance;
                }
            }
        }
        #endregion

        #region UNITY METHODS
        public virtual void OnDestroy()
        {
            s_bApplicationIsQuitting = true;
        }
        #endregion
    }
}

