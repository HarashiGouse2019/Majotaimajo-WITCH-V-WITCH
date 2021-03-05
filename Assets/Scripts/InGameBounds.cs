using UnityEngine;

public class InGameBounds : Singleton<InGameBounds>
{
    [SerializeField]
    private Transform leftBound, rightBound, topBound, bottomBound;

    public static Transform LeftBound
    {
        get
        {
            return Instance.leftBound;
        }
    }

    public static Transform RightBound
    {
        get{
        return Instance.rightBound;
        }
    }
    
    public static Transform TopBound
    {
        get
        {
            return Instance.topBound;
        }
    }

    public static Transform BottomBound
    {
        get
        {
            return Instance.bottomBound;
        }
    }

}
