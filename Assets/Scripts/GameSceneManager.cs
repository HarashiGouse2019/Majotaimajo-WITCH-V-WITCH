using Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        LoadScene("TITLE", true);
    }

    public static void UnloadScene(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }


    public static void LoadScene(string name, bool additive)
    {
        Instance.StartCoroutine(LoadSceneAsync(name, additive));
    }

    static IEnumerator LoadSceneAsync(string name, bool additive)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(name, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        loadingOperation.allowSceneActivation = false;
        yield return (loadingOperation.progress > 0.99f);
        loadingOperation.allowSceneActivation = true;
    }
}
