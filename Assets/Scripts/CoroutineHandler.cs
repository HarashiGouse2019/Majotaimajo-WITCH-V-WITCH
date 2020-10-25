using System.Collections;

public sealed class CoroutineHandler : Singleton<CoroutineHandler>
{
    public static void Execute(IEnumerator enumerator)
    {
        Instance.StartCoroutine(enumerator);
    }
}