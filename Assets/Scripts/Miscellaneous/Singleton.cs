using UnityEngine;

public class Singleton<T>: MonoBehaviour
{
    protected static T Instance;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = (T)System.Convert.ChangeType(this, typeof(T));
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(gameObject);
        }
    }

    public static bool IsNull => Instance == null;
}
