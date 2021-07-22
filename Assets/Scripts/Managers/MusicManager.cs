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
    public static AudioSource NowPlayingSource;

    // Start is called before the first frame update
    public float timeSamples;

    public float[] positionSeconds;

    public static int GetSamples() => NowPlaying.source.timeSamples;

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

    public static Music Play(string _name, float _volume = 100, bool _oneShot = false)
    {
        if (NowPlaying != null && _name == NowPlaying.name)
        {
            Debug.Log($"The track {_name} is already playing.");
            return NowPlaying;
        }

        Music a = Array.Find(Instance.getMusic, sound => sound.name == _name);

        if (a == null)
        {
            Debug.LogWarning("Sound name " + _name + " was not found.");
            return null;
        }
        else
        {
            //Turn off previously playing music
            if (NowPlaying != null)
                StopNowPlaying();

            NowPlaying = a;
            NowPlayingSource = a.source;

            switch (_oneShot)
            {
                case true:
                    NowPlayingSource.PlayOneShot(a.clip, _volume / 100);
                    break;
                default:
                    NowPlayingSource.Play();
                    NowPlayingSource.volume = _volume / 100;
                    break;
            }
        }

        return NowPlaying;
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

    public static bool Exists(string _name)
    {
        Music a = Array.Find(Instance.getMusic, sound => sound.name == _name);
        return a == null ? false : true;
    }

    public static void SetVolume(string _name, float _value)
    {
        Music a = Array.Find(Instance.getMusic, sound => sound.name == _name);
        if(a == null)
        {
            Debug.LogWarning("Music name " + _name + " was not found.");
            return;
        } else
        {
            a.source.volume = _value;
        }
    }

    public static void StopNowPlaying()
    {
        Stop(NowPlaying.name);
    }
}
