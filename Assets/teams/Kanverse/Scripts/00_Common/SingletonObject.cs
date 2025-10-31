using UnityEngine;

public abstract class SingletonObject<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool isPossibleDuplicate = false;
    public bool isDontDestory = true;
    protected static T _instance = null;

    public static T Instance
    {
        get
        {
            TryCreateSingletonObject();
            return _instance;
        }
    }

    public static T InstanceWithoutCreate
    {
        get
        {
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = gameObject.GetComponent<T>();
            if (Util.IsRootGameObject(gameObject) && isDontDestory) DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this) // 중복 방지: 기존 인스턴스와 다르면 제거
        {
            if (isPossibleDuplicate)
            {
                _instance = this.GetComponent<T>();
            }
            else
            {
                Debug.LogWarning($"[SingletonObject] 중복된 {typeof(T).Name} 인스턴스가 발견되어 제거됩니다.");
                Destroy(gameObject);
            }
            return;
        }
    }

    public static void TryCreateSingletonObject()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<T>();

            if (_instance == null)
            {
                // DebugConsole.Log($"[SingletonObject] {typeof(T).Name}이(가) 존재하지 않아서 새로운 인스턴스를 생성합니다.");
                GameObject go = new GameObject(typeof(T).ToString());
                _instance = go.AddComponent<T>();

                if (Util.IsRootGameObject(go)) DontDestroyOnLoad(go);
            }
        }
    }
}