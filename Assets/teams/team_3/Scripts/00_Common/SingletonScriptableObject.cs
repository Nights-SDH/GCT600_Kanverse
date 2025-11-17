using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<T>(PrefabRouter.ScriptableObjectRoute + typeof(T).Name);
                if (_instance == null)
                {
                    Debug.LogError($"ScriptableObject of type {typeof(T).Name} not found in Resources folder.");
                }
            }
            return _instance;
        }
    }
}
