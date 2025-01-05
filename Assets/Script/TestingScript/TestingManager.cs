using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestingManager : MonoBehaviour
{
    [Header("Pause Manager")]
    public bool isPause;
    public GameObject PauseDisplay;

    public TestingManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    { 
       if(isPause)
       {
            Time.timeScale = 1f;
            PauseDisplay.SetActive(false);
       }
       else
       {
            Time.timeScale = 0f;
            PauseDisplay.SetActive(true);
        }
    }

}
