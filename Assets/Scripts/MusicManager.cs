using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    /*Music Manager will essentially collect all music files
     * from all SongEntities detected by RoftScouter,
     * and it will preview it for the player.
     * 
     * Music Manager will later be changed to
     * SongEntityManager, because instead of just Music clips,
     * it'll be a class that holds not only the song (in which it'll go through and play),
     * but give us all kinds of different information.
     */

    private static MusicManager Instance;

    [System.Serializable]
    public class Music
    {
        public string name; // Name of the audio

        public AudioClip clip; //The Audio Clip Reference

        [Range(0f, 1f)]
        public float volume; //Adjust Volume

        [Range(.1f, 3f)]
        public float pitch; //Adject pitch

        public bool enableLoop; //If the audio can repeat

        [HideInInspector] public AudioSource source;
    }

    public AudioMixerGroup musicMixer;

    public Slider musicVolumeAdjust; //Reference to our volume sliders

    public Music[] getMusic;

    public static Music NowPlaying;

    // Start is called before the first frame update
    public float timeSamples;

    public float[] positionSeconds;

    static float StartLoopBoundary, EndLoopBoundary;
    static bool InIntro = true;
    static IEnumerator LoopCycle;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        foreach (Music a in getMusic)
        {
            a.source = gameObject.AddComponent<AudioSource>();

            a.source.clip = a.clip;

            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.enableLoop;
            a.source.outputAudioMixerGroup = musicMixer;
        }
    }

    static IEnumerator MusicLoopCycle()
    {
        while (true)
        {
            if (NowPlaying.source.time >= StartLoopBoundary)
                InIntro = false;

            if(NowPlaying.source.time >= EndLoopBoundary && InIntro == false)
            {
                Debug.Log("Looping...");
                NowPlaying.source.time = (int)StartLoopBoundary;
            }

            yield return null;
        }
    }

    /// <summary>
    /// Play audio and adjust its volume.
    /// </summary>
    /// 
    /// <param name="_name"></param>
    /// The audio clip by name.
    /// 
    /// <param name="_volume"></param>
    /// Support values between 0 and 100.
    ///

    public static void Play(string _name, float _volume = 100, bool _oneShot = false, float mainLoopStart = 0, float mainLoopEnd = 0)
    {
        LoopCycle = MusicLoopCycle();

        if (NowPlaying != null && _name == NowPlaying.name)
        {
            Debug.Log($"The track {_name} is already playing.");
            return;
        }

        Music a = Array.Find(Instance.getMusic, sound => sound.name == _name);

        if (a == null)
        {
            Debug.LogWarning("Sound name " + _name + " was not found.");
            return;
        }
        else
        {
            //Turn off previously playing music
            if (NowPlaying != null)
                StopNowPlaying();

            NowPlaying = a;

            StartLoopBoundary = mainLoopStart;
            EndLoopBoundary = mainLoopEnd == 0 ? NowPlaying.clip.length : mainLoopEnd;

            switch (_oneShot)
            {
                case true:
                    NowPlaying.source.PlayOneShot(a.clip, _volume / 100);
                    break;
                default:
                    NowPlaying.source.Play();
                    NowPlaying.source.volume = _volume / 100;
                    break;
            }

            Instance.StartCoroutine(LoopCycle);
        }
    }
    public static void Stop(string _name)
    {
        Music a = Array.Find(Instance.getMusic, sound => sound.name == _name);
        if (a == null)
        {
            Debug.LogWarning("Music name " + _name + " was not found.");
            return;
        }
        else
        {
            a.source.Stop();
            NowPlaying = null;
        }
    }

    public static void StopNowPlaying()
    {
        Instance.StopCoroutine(LoopCycle);
        Stop(NowPlaying.name);
    }
}
