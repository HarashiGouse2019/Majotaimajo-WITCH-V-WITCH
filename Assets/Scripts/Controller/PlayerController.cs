using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Public Members
    //Have our keys mapped;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode up = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
    public KeyCode shoot = KeyCode.Z;
    public KeyCode special1 = KeyCode.A;
    public KeyCode special2 = KeyCode.S;
    public KeyCode special3 = KeyCode.D;
    #endregion

    #region Private Members
    //Reference Pawn
    PlayerPawn pawn;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        pawn = GetComponent<PlayerPawn>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This will be based on rotating, so we need a rotating function
    public void Rotate(object _direction)
    {
        //A switch case of if it's "Left" or "Right"
        switch (_direction)
        {
            //It takes Left and Right as 
            case "Left":

                break;
            case "Right":

                break;

            case -1:

                break;
            case 1:

                break;
            default:
                //Do Nothing
                break;
        }
    }

    //Remember, we are controlling pawn!!!
}
