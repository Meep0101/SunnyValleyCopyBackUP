using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Add this line to include the necessary namespace

public class Wait : MonoBehaviour
{
    public float wait_time = 5f;

    void Start()
    {
        StartCoroutine(Wait_for_intro());
    }

    IEnumerator Wait_for_intro()
    {
        yield return new WaitForSeconds(wait_time);
        SceneManager.LoadScene(1);
    }
}
