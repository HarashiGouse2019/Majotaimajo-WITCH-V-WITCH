using Extensions;
using System.Collections;
using UnityEngine;

public class MaplePawn : PlayerPawn
{
    [SerializeField]
    private FollowBehind[] _starBits = new FollowBehind[2];

    #region Constants
    private const float FOCUS_OFFSET_X = 0.5f;
    private const float FOCUS_OFFSET_Y = 0.5f;
    private const int LEFT_STARBIT = 0;
    private const int RIGHT_STARBIT = 1;
    #endregion

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
            _starBits[LEFT_STARBIT].UpdateOffset(IsOnFocus ? new Vector3(FOCUS_OFFSET_X, FOCUS_OFFSET_Y) : Vector3.zero);
            _starBits[RIGHT_STARBIT].UpdateOffset(IsOnFocus ? new Vector3(-FOCUS_OFFSET_X, FOCUS_OFFSET_Y) : Vector3.zero);
            yield return null;
        }
    }
}
