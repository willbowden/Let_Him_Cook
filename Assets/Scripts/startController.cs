using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class startController : MonoBehaviour
{ 
    [Header("Camera")]
    [SerializeField] public Camera mainCamera; // Reference to the main camera
    public float distanceFromCamera = 0.6f;

    [Header("UI Elements")]
    [SerializeField] private Button start; // Reference to the start button
    [SerializeField] private Button quit; // Reference to the quit button
    [SerializeField] private TextMeshProUGUI lhc; // Reference to the quit button

    [Header("How to Play")]
    public GameObject canvas; // Assign your Canvas in the Inspector
    [SerializeField] private GameObject htpContainer; // Parent GameObject for level buttons
    [SerializeField] private Button backButton; // Reference to the back button

    

    void Start()
    {
        if (mainCamera == null) return;

        if (htpContainer != null)
        {
            htpContainer.SetActive(false); // Show htp container
        }
        if (backButton != null)
        {
            backButton.gameObject.SetActive(false); // Show back button
        }
    }

    public void Quit()
    {
        // Quit the application
        Application.Quit();
    }

    public void Continue()
    {
        // Show htp container and back button, hide main menu buttons
        start.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        lhc.gameObject.SetActive(false);

        if (htpContainer != null)
        {
            htpContainer.SetActive(true); // Show htp container
        }
        if (backButton != null)
        {
            backButton.gameObject.SetActive(true); // Show back button
        }
    }

    public void BackToMenu()
    {
        // Hide info and startgame button, show main menu buttons
        start.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        lhc.gameObject.SetActive(true);

        if (htpContainer != null)
        {
            htpContainer.SetActive(false); // Hide htp container
        }
        if (backButton != null)
        {
            backButton.gameObject.SetActive(false); // Hide back button
        }
    }

    public void StartGame()
    {
        if (canvas != null)
        {
            canvas.SetActive(false); // Hide the canvas
        }
        Time.timeScale = 1;
        SceneManager.LoadScene("kitchen"); // Loads the "kitchen" scene
    }
}

