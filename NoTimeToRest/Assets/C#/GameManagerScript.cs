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
    public void Retry()
    {
        SceneManager.LoadScene("Gameplay");
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
