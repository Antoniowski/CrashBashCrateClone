using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    [Range(0,1f)]
    public float volume;
    
    [Range(0,3f)]
    public float pitch;

    public bool loop;

    public AudioClip audioClip;
    [HideInInspector]
    public AudioSource audioSource;
}
