using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : MonoBehaviour
{
    public static AudioSystem instance;
    [SerializeField]
    AudioMixer mixer;
    [SerializeField]
    AudioSource musicSource, soundSource;
    [SerializeField]
    AudioClip backgroundMusic;
    [SerializeField]
    AudioClip[] battleMusic, sounds;
    void Awake()
    {
        if (instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void PlayMenuBackMusic()
    {
        musicSource.Stop();
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }
    public void PlaySound(string name)
    {
        AudioClip clip = Array.Find(sounds, s => s.name == name);
        if(clip == null)
        {
            clip = sounds[0];
        }
        soundSource.PlayOneShot(clip);
    }
    public void PlayBattleTheme(int ind)
    {
        AudioClip clip = battleMusic[ind];
        if (clip != null)
        {
            clip = battleMusic[0];
        }
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
    }
    void Start()
    {
        LoadSettings();
        if(!musicSource.isPlaying)
        {
            PlayMenuBackMusic();
        }
    }
    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            SetMasterAudio(PlayerPrefs.GetFloat("MasterVolume"));
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
        }
        if (PlayerPrefs.HasKey("SoundsVolume"))
        {
            SetSoundsVolume(PlayerPrefs.GetFloat("SoundsVolume"));
        }
    }
    public void SetMasterAudio(float volume)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }
    public void SetSoundsVolume(float volume)
    {
        mixer.SetFloat("SoundsVolume", Mathf.Log10(volume) * 20);
    }
}
