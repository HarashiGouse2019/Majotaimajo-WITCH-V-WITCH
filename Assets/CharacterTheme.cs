using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTheme : MonoBehaviour
{
    [SerializeField] string characterThemeSong;

    private void Start()
    {
       MusicManager.Play(characterThemeSong);
    }
}
