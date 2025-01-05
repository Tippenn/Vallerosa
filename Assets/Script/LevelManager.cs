using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject bossUI;
    public GameObject winUI;
    public GameObject loseUI;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void RestartLevel()
    {
        Debug.Log("hnjdfaqwjkbdw");
        SceneManager.LoadScene("BetaScene");
    }
}
