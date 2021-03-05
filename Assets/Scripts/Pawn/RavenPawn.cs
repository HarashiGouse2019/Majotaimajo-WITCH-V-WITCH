using System.Collections;
using UnityEngine;
using Extensions;

public class RavenPawn : PlayerPawn
{
    [SerializeField]
    private AutoOrbit[] autoOrbitObjs;

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
        FocusCycle().Start();
    }

    protected override IEnumerator FocusCycle()
    {
        while (true)
        {
            for (int index = ZERO; index < autoOrbitObjs.Length; index++)
            {
                AutoOrbit orbitObj = autoOrbitObjs[index];
                orbitObj.ChangeRadius(IsOnFocus);
            }

            yield return null;
        }
    }
}