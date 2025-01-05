using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("----Audio Source----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    [Header("-SFX")]
    public AudioClip backGround;
    public AudioClip walk;
    public AudioClip damage;
    public AudioClip jump;
    public AudioClip playerDeath;
    public AudioClip shoot;
    public AudioClip reload;
    public AudioClip enemyDeath;
    public AudioClip onClick;

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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlaySFX(onClick);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
