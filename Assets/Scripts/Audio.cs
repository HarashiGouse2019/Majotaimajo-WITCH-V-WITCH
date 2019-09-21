using UnityEngine.Audio;
using UnityEngine;

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
