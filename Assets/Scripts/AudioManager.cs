﻿using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager Instance;



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
        Audio a = Find(_name);
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
        Audio a = Find(_name);
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

    public static Audio Find(string _name)
    {
        Audio a = Array.Find(Instance.getAudio, sound => sound.name == _name);
        if (a == null)
        {
            Debug.LogWarning("Sound name " + _name + " was not found.");
            return null;
        }
        else
        {
            return a;
        }
    }

}

namespace Extensions
{
    public static class AudioManagerExtension
    {
        public static void Play(this Audio audio, float _volume = 100, bool _oneShot = false)
        {
            switch (_oneShot)
            {
                case true:
                    audio.source.PlayOneShot(audio.clip, _volume / 100);
                    break;
                default:
                    audio.source.Play();
                    audio.source.volume = _volume / 100;
                    break;
            }
        }
    }
}