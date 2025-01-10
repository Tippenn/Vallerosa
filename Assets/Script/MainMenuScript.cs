using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainmenuPanel;
    public GameObject optionsPanel;
    public GameObject exitPanel;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region mainmenu button

    public void StartClicked()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void SettingClicked()
    {
        optionsPanel.SetActive(true);
    }

    public void ExitClicked()
    {
        exitPanel.SetActive(true);
    }

    #endregion

    #region options button
    public void OptionExitClicked()
    {
        optionsPanel.SetActive(false);
    }
    #endregion

    #region exit button
    public void ExitYesClicked()
    {
        Application.Quit();
    }

    public void ExitNoClicked()
    {
        exitPanel.SetActive(false);
    }

    #endregion

}
