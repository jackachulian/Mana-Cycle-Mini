using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour
{   
    public static SoundManager Instance;
    public static SoundEffects sfx;
    [SerializeField] private AudioSource musicSource, effectSource;
    // public float masterVolume;
    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            sfx = GetComponent<SoundEffects>();
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }

    }

    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        // create a new gameobject with an audiosource, to avoid interfering with other sound effects
        GameObject tempEffectSource = new GameObject("tempEffectSource");
        tempEffectSource.AddComponent<AudioSource>();
        tempEffectSource.GetComponent<AudioSource>().pitch = pitch;
        tempEffectSource.GetComponent<AudioSource>().PlayOneShot(clip, Instance.effectSource.volume*volume);
        Destroy(tempEffectSource, clip.length);
    }

    /** Set the background music. If the passed song is already playing, do not replay */
    public void SetBGM(AudioClip clip) {
        if (Instance.musicSource.clip == clip) return;

        Instance.musicSource.Stop();
        Instance.musicSource.clip = clip;
        Instance.musicSource.Play();
    }

    /** Load the song but do not play it yet. */
    public void LoadBGM(AudioClip clip) {
        Instance.musicSource.Stop();
        Instance.musicSource.clip = clip;
    }

    public void PlayBGM() {
        Instance.musicSource.Play();
    }

    public void UnpauseBGM() {
        Instance.musicSource.UnPause();
    }

    public void PauseBGM() {
        Instance.musicSource.Pause();
    }

    public void StopBGM() {
        Instance.musicSource.Stop();
    }

    public void SetBGMVolume(float v)
    {
        Instance.musicSource.volume = v;
    }

    public void UnloadBGM() {
        StopBGM();
        Instance.musicSource.clip = null;
    }

    // sliders cannot call functions with more than 1 param (?)
    public void PlaySound(AudioClip clip)
    {
        PlaySound(clip, 1f);
    }

    float timeSinceLastTestSound = 0f;
    public void PlayTestSound(AudioClip clip)
    {
        if (Time.time - timeSinceLastTestSound >= 0.2f)
        {
            PlaySound(clip, 1f);
            timeSinceLastTestSound = Time.time;
        }
    }

    public void SetMusicVolume(float value)
    {
        Instance.musicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        Instance.effectSource.volume = value;
    }

}
