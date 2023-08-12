using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    Slider masterSlider;
    [SerializeField]
    Slider musicSlider;
    [SerializeField]
    Slider soundsSlider;
    void Awake()
    {
        masterSlider.onValueChanged.AddListener(AudioSystem.instance.SetMasterAudio);
        musicSlider.onValueChanged.AddListener(AudioSystem.instance.SetMusicVolume);
        soundsSlider.onValueChanged.AddListener(AudioSystem.instance.SetSoundsVolume);
        this.gameObject.SetActive(false);
    }
    private void Start()
    {
        LoadSettings();
    }
    public void LoadSettings()
    {
        if(PlayerPrefs.HasKey("MasterVolume"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        if (PlayerPrefs.HasKey("SoundsVolume"))
        {
            soundsSlider.value = PlayerPrefs.GetFloat("SoundsVolume");
        }
    }
    public void SaveSettings()
    {
        AudioSystem.instance.PlaySound("Click");
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SoundsVolume", soundsSlider.value);
    }
}
