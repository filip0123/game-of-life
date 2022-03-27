using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private static SoundController _instance;

    public static SoundController Instance => _instance;

    public AudioSource _musicSource;
    public AudioSource _SFXSource;

    public List<AudioClip> _clips;

    bool _muted;

    public bool Muted { get => _muted; }

    public void Initialize()
    {
        _instance = this;
    }

    public void Mute()
    {
        _muted = true;
        _musicSource.mute = true;
        _SFXSource.mute = true;
    }

    public void UnMute()
    {
        _muted = false;
        _musicSource.mute = false;
        _SFXSource.mute = false;
    }

    public void PlayClip(int index)
    {
        _SFXSource.clip = _clips[index];
        _SFXSource.Play();
    }
}

public enum Sound
{
    Bell = 0, 
    Button = 1, 
    Pop = 2,
    Error = 3
}
