using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenDebugConsole : MonoBehaviour
{
    public GameObject uiDebugConsole; // Reference to the UI window to toggle

    // Check for input every frame
    void Update()
    {
        // Capture the input string
        string inputString = Input.inputString.ToLower();

        // Check if the input string contains the word "issue"
        if (inputString.Contains("t"))
        {
            // Toggle the UI window
            ToggleUI();
        }
    }

    // Method to toggle the UI window
    void ToggleUI()
    {
        // Check if the UI window is active
        if (uiDebugConsole.activeSelf)
        {
            // Deactivate the UI window if it's active
            uiDebugConsole.SetActive(false);
        }
        else
        {
            // Activate the UI window if it's inactive
            uiDebugConsole.SetActive(true);
        }
    }
}
