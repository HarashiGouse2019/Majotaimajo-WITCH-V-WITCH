using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRACLoader : MonoBehaviour
{
    [SerializeField]
    private Animator characterBodyController;

    private void Start()
    {
        LoadCharacterRAC();
    }

    void LoadCharacterRAC()
    {
        characterBodyController.runtimeAnimatorController = GameManager.CharacterAnimatorController;
    }
}
