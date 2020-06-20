using System;
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
    public class Audio
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

    public Audio[] getMusic;

    public static string NowPlaying;

    // Start is called before the first frame update
    public float timeSamples;

    public float[] positionSeconds;

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

        foreach (Audio a in getMusic)
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

    public static void Play(string _name, float _volume = 100, bool _oneShot = false)
    {
        Audio a = Array.Find(Instance.getMusic, sound => sound.name == _name);
        if (a == null)
        {
            Debug.LogWarning("Sound name " + _name + " was not found.");
            return;
        }
        else
        {
            switch (_oneShot)
            {
                case true:
                    a.source.PlayOneShot(a.clip, _volume / 100);
                    break;
                default:
                    a.source.Play();
                    a.source.volume = _volume / 100;
                    break;
            }

        }
    }
    public static void Stop(string _name)
    {
        Audio a = Array.Find(Instance.getMusic, sound => sound.name == _name);
        if (a == null)
        {
            Debug.LogWarning("Music name " + _name + " was not found.");
            return;
        }
        else
        {
            a.source.Stop();
        }
    }

    public AudioClip GetAudio(string _name, float _volume = 100)
    {
        Audio a = Array.Find(getMusic, sound => sound.name == _name);
        if (a == null)
        {
            Debug.LogWarning("Music name " + _name + " was not found.");
            return null;
        }
        else
        {
            a.source.Play();
            a.source.volume = _volume / 100;
            return a.source.clip;
        }
    }
}
