using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button pauseResume; // Reference to the pause/resume button
    [SerializeField] private Button quit; // Reference to the quit button
    [SerializeField] private Button levels; // Reference to the levels button

    [Header("Level Buttons")]
    [SerializeField] private GameObject levelButtonsContainer; // Parent GameObject for level buttons
    [SerializeField] private Button backButton; // Reference to the back button

    private bool isPaused = false; // Track whether the game is paused
    private Text pauseResumeText; // Reference to the button's text component

    void Start()
    {
        // Get the Text component from the pauseResume button
        pauseResumeText = pauseResume.GetComponentInChildren<Text>();
        if (pauseResumeText != null)
        {
            pauseResumeText.text = "Resume"; // Set initial text
        }

        // Ensure level buttons and back button are hidden initially
        if (levelButtonsContainer != null)
        {
            levelButtonsContainer.SetActive(false);
        }
        if (backButton != null)
        {
            backButton.gameObject.SetActive(false);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1; // Resume the game by setting time scale back to normal
        isPaused = false;   // Ensure the paused state is updated
    }


    public void Quit()
    {
        // Quit the application
        Application.Quit();
    }

    public void Levels()
    {
        // Show level buttons and back button, hide main menu buttons
        pauseResume.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        levels.gameObject.SetActive(false);

        if (levelButtonsContainer != null)
        {
            levelButtonsContainer.SetActive(true); // Show level buttons
        }
        if (backButton != null)
        {
            backButton.gameObject.SetActive(true); // Show back button
        }
    }

    public void BackToMenu()
    {
        // Hide level buttons and back button, show main menu buttons
        pauseResume.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        levels.gameObject.SetActive(true);

        if (levelButtonsContainer != null)
        {
            levelButtonsContainer.SetActive(false); // Hide level buttons
        }
        if (backButton != null)
        {
            backButton.gameObject.SetActive(false); // Hide back button
        }
    }

    public void Level1() {

    }

    public void Level2() {
        
    }
}
