using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTesting : MonoBehaviour
{
    public void ChangeMiniBossBGM()
    {
        //AudioManager.Instance.musicSource.Stop();
        AudioManager.Instance.musicSource.clip = AudioManager.Instance.miniboss;
        AudioManager.Instance.musicSource.Play();
    }

    public void ChangeBossBGM()
    {
        //AudioManager.Instance.musicSource.Stop();
        AudioManager.Instance.musicSource.clip = AudioManager.Instance.boss;
        AudioManager.Instance.musicSource.Play();
    }

    public void ChangeNormalBGM()
    {
        //AudioManager.Instance.musicSource.Stop();
        AudioManager.Instance.musicSource.clip = AudioManager.Instance.normal;
        AudioManager.Instance.musicSource.Play();
    }


}
