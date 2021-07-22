using System.Collections;
using UnityEngine;

using static MusicManager;


public class MusicTheme : MonoBehaviour
{

      
    [SerializeField] string themeSong;

    bool _atIntro = false;
    bool _inMainLoop = false;

    IEnumerator _themeCycle;

    private readonly string[] MARKERS =
    {
       " (Intro)",
       " (MainLoop)"
    };


    private void Awake()
    {
        _themeCycle = ThemeCycle();   
    }


    public void PlayTheme()
    {
        if(Exists(themeSong + MARKERS[0]))
        {
            PlayIntro();
        }
        else
        {
            PlayMainLoop();
        }
    }

    void PlayIntro()
    {

        if (Exists(themeSong + MARKERS[0]))
        {
            Play(themeSong + MARKERS[0], 100f);
            NowPlaying.enableLoop = _atIntro;
            _atIntro = true;
            StartCoroutine(_themeCycle);
        }
    }

    void PlayMainLoop()
    {
        if(Exists(themeSong + MARKERS[1]))
        {
            Play(themeSong + MARKERS[1], 100f);
            NowPlaying.enableLoop = true;
        } 
    }

    IEnumerator ThemeCycle()
    {
        yield return new WaitUntil(() => _atIntro && NowPlaying.source.timeSamples >= NowPlaying.clip.samples);
        PlayMainLoop();
    }
}
