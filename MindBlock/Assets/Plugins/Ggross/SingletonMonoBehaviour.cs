using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ggross.Template {

    /// <summary>
    /// MonoBehaviour单例模式模板
    /// </summary>
    /// <typeparam name="T">实例类名</typeparam>
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
  
        private static object _lock = new object();
  
        public static T Instance 
        {
            get
            {
                if (applicationIsQuitting) {
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T) FindObjectOfType(typeof(T));
 
                        if (FindObjectsOfType (typeof(T)).Length > 1)

                        {
                            return _instance;
                        }
 
                        if (_instance == null) {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = "(singleton) " + typeof(T).ToString();
 
                            //DontDestroyOnLoad(singleton);
                        }
                    }
 
                    return _instance;
                }
            }
        }
 
        private static bool applicationIsQuitting = false;
 
        public void OnDestroy()
        {
            //applicationIsQuitting = true;
        }
        
    }

}



