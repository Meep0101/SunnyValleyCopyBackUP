using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapChoice : MonoBehaviour
{
    public void PlayMap()
    {
        SceneManager.LoadScene("TownScene");
    }

    public void BackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    //
}
