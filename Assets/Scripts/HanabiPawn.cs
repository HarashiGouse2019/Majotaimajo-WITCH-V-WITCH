using System.Collections;

public class HanabiPawn : PlayerPawn
{
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void Start()
    {
        base.Start();

        //Start Focus Cycle Coroutine
        //FocusCycle().Start();
    }

    protected override IEnumerator FocusCycle()
    {
        while (true)
        {

            yield return null;
        }
    }
}
