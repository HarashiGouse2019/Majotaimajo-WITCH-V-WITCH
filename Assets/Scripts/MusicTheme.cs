using System;
using UnityEngine;

public class MusicTheme : MonoBehaviour
{

      
    [SerializeField] string themeSong;

    [SerializeField] float mainSongStart, mainSongEnd;
    public void Play()
    {
       MusicManager.Play(themeSong, 100f, false, mainSongStart, mainSongEnd);
    }
}
