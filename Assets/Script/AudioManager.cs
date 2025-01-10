using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("----Audio Source----")]
    public AudioSource musicSource;
    public AudioSource SFXSource;
    public AudioSource walking;

    [Header("Audio Clip")]
    [Header("-SFX")]
    //main char
    public AudioClip walk;
    public AudioClip crouch;
    public AudioClip takingDamage;
    public AudioClip reload;
    public AudioClip singleShot;
    //monster
    //aranbatum
    public AudioClip aranBatumRoar;
    //suki
    public AudioClip sukiRoar;

    [Header("-BGM")]
    public AudioClip normal;
    public AudioClip miniboss;
    public AudioClip boss;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // Destroy this instance because the singleton already exists
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Make sure this instance persists across scenes
        }
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void turnOn(AudioSource audio)
    {
        walking.enabled = true;
    }
    public void turnOff(AudioSource audio)
    {
        walking.enabled = false;
    }
}
