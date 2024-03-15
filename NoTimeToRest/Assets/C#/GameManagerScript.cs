using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManagerScript : MonoBehaviour
{

    public AudioSource audioSource;

    // Start is called before the first frame update
    public void Restart()
    {
        if (audioSource != null)
        {
            audioSource.UnPause();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
    public void Retry()
    {
        SceneManager.LoadScene("Gameplay");
        if (audioSource != null)
        {
            audioSource.UnPause();
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
