using System;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    public static T Instance   // Laze Load
    {
        get
        {
            if (_instance == null)
            {
                // 防止场景中重复
                Type type = typeof(T);
                _instance = (T)FindObjectOfType(type);
                
                if (_instance == null)
                {
                    GameObject obj = new GameObject(type.Name);
                    _instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }
            }

            return _instance;
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance != null && _instance.gameObject == gameObject)
        {
            _instance = null;
        }
    }

    /// <summary>
    /// 删除单例对象，并场景中的go
    /// </summary>
    public static void DestroyInstance()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
            _instance = null;
        }
    }
}
