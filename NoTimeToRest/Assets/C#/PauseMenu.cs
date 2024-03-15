using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;
    public AudioSource audioSource;
    public AudioSource audioBackgroundSource;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
        // Unpause the audio source
        if (audioBackgroundSource != null)
        {
            audioSource.UnPause();
            audioBackgroundSource.UnPause();
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause game time
        isPaused = true;
        // Pause the audio source
        if (audioBackgroundSource != null)
        {
            audioSource.Pause();
            audioBackgroundSource.Pause();
        }
    }
}
