using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    //singleton instance
    public static AudioManager instance;

    //Suoni gestiti dal manager
    public Sound[] sounds;

    [Range(0,1f)] public float musicVolume;
    [Range(0,1f)] public float SFXVolume;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SetUpSounds();
    }

    void Start()
    {

    }

    void SetUpSounds()
    {
        foreach (Sound sound in sounds)
        {
            sound.audioSource = GetComponentInChildren<AudioPlayer>().gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }
    }

    public void PlayTargetSound(string clipName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == clipName);
        s.audioSource.Play();
    }

    public void PauseClip(string clipName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == clipName);
        s.audioSource.Pause();
    }

    public void ResumeClip(string clipName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == clipName);
        if(s.audioSource.isPlaying)
            return;
        s.audioSource.Play();
    }

    public void StopClip(string clipName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == clipName);
        s.audioSource.Stop();  
    }

    public void StopAllMusic()
    {
        foreach (Sound s in sounds)
        {
            s.audioSource.Stop();
        }
    }
}
