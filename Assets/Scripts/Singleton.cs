using UnityEngine;

public class Singleton<T>: MonoBehaviour
{
    protected static T Instance;
    void OnEnable()
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

    public static bool IsNull() => Instance == null;
}
