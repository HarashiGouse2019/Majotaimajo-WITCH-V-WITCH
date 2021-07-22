using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>
{
    public static void LoadScene(string name, bool additive = false)
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
