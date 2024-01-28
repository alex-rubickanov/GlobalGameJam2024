using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    public AudioSource bgMusic;
    public AudioSource sfx;
    [Header("SFX")]
    public AudioClip fartBombSfx;
    public AudioClip bottleSFx;
    public AudioClip SlideSfx;
    public AudioClip BalloonSFx;
    public AudioClip bombSfx;
    public AudioClip bonkSfx;
    public AudioClip playerTrappedSfx;
    public AudioClip scanSFX;
    public AudioClip throwSFX;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayOneShotSfx(AudioClip clip, float volume)
    {
        sfx.PlayOneShot(clip, volume);
    }
    public void PlayOneShotSfx(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }

}
