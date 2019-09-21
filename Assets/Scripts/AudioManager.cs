using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public new static AudioManager audio;

    public Slider musicVolumeAdjust, soundVolumeAdjust; //Reference to our volume sliders

    public Audio[] getAudio;

    private void Awake()
    {
        if (audio == null)
        {
            audio = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

        foreach (Audio a in getAudio)
        {
            a.source = gameObject.AddComponent<AudioSource>();

            a.source.clip = a.clip;

            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.enableLoop;
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

    public void Play(string _name, float _volume = 100)
    {
        Audio a = Array.Find(getAudio, sound => sound.name == _name);
        if (a == null)
        {
            Debug.LogWarning("Sound name " + _name + " was not found.");
            return;
        } else
        {
            a.source.Play();
            a.source.volume = _volume / 100;
        }
    }
    public void Stop(string _name)
    {
        Audio a = Array.Find(getAudio, sound => sound.name == _name);
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
}
