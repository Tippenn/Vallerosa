using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider masterSlider;

    private void Awake()
    {

    }

    void Start()
    {
        LoadVolume();
    }

    #region setAudio
    public void SetMasterVolume()
    {
        float musicVolume = masterSlider.value;
        if (musicVolume == 0)
        {
            audioMixer.SetFloat("MasterVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(musicVolume) * 20);
        }
        PlayerPrefs.SetFloat("masterVolume", musicVolume);
    }

    public void SetMusicVolume()
    {
        float musicVolume = musicSlider.value;
        if (musicVolume == 0)
        {
            audioMixer.SetFloat("MusicVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        }
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }

    public void SetSFXVolume()
    {
        float SFXVolume = SFXSlider.value;
        if (SFXVolume == 0)
        {
            audioMixer.SetFloat("SFXVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(SFXVolume) * 20);
        }
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
    }

    #endregion
    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume");

        SetMusicVolume();
        SetSFXVolume();
        SetMasterVolume();
    }
}
