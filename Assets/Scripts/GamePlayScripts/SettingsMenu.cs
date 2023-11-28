using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // public void BackMainMenu()
    // {
    //     SceneManager.LoadScene("MainMenu");
    // }
    //
}
