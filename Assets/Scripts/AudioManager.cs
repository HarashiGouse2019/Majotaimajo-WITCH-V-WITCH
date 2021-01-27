using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager Instance;

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

    public AudioMixerGroup audioMixer;

    public Slider soundVolumeAdjust; //Reference to our volume sliders

    public Audio[] getAudio;

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

        foreach (Audio a in getAudio)
        {
            a.source = gameObject.AddComponent<AudioSource>();

            a.source.clip = a.clip;

            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.enableLoop;
            a.source.outputAudioMixerGroup = audioMixer;
            a.source.playOnAwake = false;
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
        Audio a = Array.Find(Instance.getAudio, sound => sound.name == _name);
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
        Audio a = Array.Find(Instance.getAudio, sound => sound.name == _name);
        if (a == null)
        {
            Debug.LogWarning("Sound name " + _name + " was not found.");
            return;
        }
        else
        {
            a.source.Stop();
        }
    }

    public AudioClip GetAudio(string _name, float _volume = 100)
    {
        Audio a = Array.Find(getAudio, sound => sound.name == _name);
        if (a == null)
        {
            Debug.LogWarning("Sound name " + _name + " was not found.");
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

