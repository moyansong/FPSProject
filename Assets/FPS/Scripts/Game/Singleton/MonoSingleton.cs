using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoSingleton<T> : MonoBehaviour where T: MonoBehaviour
{

    static T instance;

    public static T Instance
    {
        get
        {
            if (MonoSingletonObject.go == null)
            {
                MonoSingletonObject.go = new GameObject("MonoSingletonObject");
                DontDestroyOnLoad(MonoSingletonObject.go);
            }

            if (MonoSingletonObject.go != null && instance == null)
            {
                instance = MonoSingletonObject.go.AddComponent<T>();
            }

            return instance;
        }
    }

    public static bool destroyOnLoad = false;


    /// <summary>
    /// 
    /// </summary>
    public void AddSceneChangedEvent()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnSceneChanged(Scene sc1, Scene sc2)
    {
        if (destroyOnLoad)
        {
            if (instance != null)
            {
                DestroyImmediate(instance);
                Debug.Log(instance == null);
            }
        }
    }
}

public class MonoSingletonObject
{
    public static GameObject go;
}
