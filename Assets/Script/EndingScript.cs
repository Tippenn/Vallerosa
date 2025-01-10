using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScript : MonoBehaviour
{
    void Start()
    {
        Invoke("PindahKeScene", 1f);
    }

    public void PindahKeScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
