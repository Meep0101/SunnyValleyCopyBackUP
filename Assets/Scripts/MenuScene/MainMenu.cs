using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MapSelection");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AboutUs()
    {
        SceneManager.LoadScene("AboutUS");
    }

    public void LearnHub()
    {
        SceneManager.LoadScene("LearnHub");
    }
}
