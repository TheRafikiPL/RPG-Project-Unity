using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NVorbis;
using UnityEngine.Audio;
using UnityEditor;

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
    [SerializeField]
    int selectedTheme = 0;

    //Ogg looping system variables
    [SerializeField]
    bool isCorrectOgg = false;
    [SerializeField]
    int loopStart;
    [SerializeField]
    int loopLength;
    [SerializeField]
    int loopEnd;
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
    private void Update()
    {
        if(isCorrectOgg)
        {
            if (musicSource.timeSamples > loopEnd)
            {
                musicSource.timeSamples -= loopLength;
            }
        }
    }
    void PlayMusic(AudioClip audioClip)
    {
        string path = AssetDatabase.GetAssetPath(audioClip.GetInstanceID());
        if (path.Split('.')[1] == "ogg")
        {
            using(var f = new VorbisReader(path))
            {
                if(f.Tags.All.ContainsKey("LOOPSTART") && f.Tags.All.ContainsKey("LOOPLENGTH"))
                {
                    loopStart = Convert.ToInt32(f.Tags.All["LOOPSTART"][0]);
                    loopLength = Convert.ToInt32(f.Tags.All["LOOPLENGTH"][0]);
                    loopEnd = loopStart + loopLength;
                    isCorrectOgg = true;
                }
            }
        }
        else
        {
            isCorrectOgg = false;
        }
        musicSource.clip = audioClip;
        musicSource.Play();
    }
    public void PlayMenuBackMusic()
    {
        musicSource.Stop();
        PlayMusic(backgroundMusic);
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
    public void PlayBattleTheme()
    {
        AudioClip clip = battleMusic[selectedTheme];
        if (clip != null)
        {
            clip = battleMusic[0];
        }
        musicSource.Stop();
        PlayMusic(clip);
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
