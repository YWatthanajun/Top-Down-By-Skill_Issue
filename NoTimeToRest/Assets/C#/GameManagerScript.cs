using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManagerScript : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioSource audioBackgroundSource;

    // Start is called before the first frame update
    public void Restart()
    {
        if (audioBackgroundSource != null)
        {
            audioSource.UnPause();
            audioBackgroundSource.UnPause();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
    public void Retry1()
    {
        SceneManager.LoadScene("Gameplay1");
        if (audioBackgroundSource != null)
        {
            audioSource.UnPause();
            audioBackgroundSource.UnPause();
        }
    }
    public void Retry2()
    {
        SceneManager.LoadScene("Gameplay2");
        if (audioBackgroundSource != null)
        {
            audioSource.UnPause();
            audioBackgroundSource.UnPause();
        }
    }
    public void Retry3()
    {
        SceneManager.LoadScene("Gameplay3");
        if (audioBackgroundSource != null)
        {
            audioSource.UnPause();
            audioBackgroundSource.UnPause();
        }
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
