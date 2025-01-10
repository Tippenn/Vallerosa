using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject bossUI;
    public GameObject pauseUI;
    public GameObject winUI;
    public GameObject loseUI;

    public bool isPaused;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        if(PlayerStats.Instance.isDead)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                RestartLevel();
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                ExitMainMenu();
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Pause();
            }
        }
        
    }

    public void Pause()
    {
        if(isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            pauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0f;
            pauseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ReturnToHardCheckPoint()
    {

    }

    public void ReturnToSoftCheckPoint()
    {

    }

    public void WeWin()
    {
        winUI.SetActive(true);
        Invoke("ChangeScene", 3f);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("WinScene");
    }
    #region button
    public void ResumeLevel()
    {
        Debug.Log("played");
        Pause();
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void ExitMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion
}
