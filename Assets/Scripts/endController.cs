using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndController : MonoBehaviour
{ 
    [Header("Camera")]
    [SerializeField] public Camera mainCamera; // Reference to the main camera
    public float distanceFromCamera = 0.6f;

    [Header("UI Elements")]
    [SerializeField] private GameObject canvas; // Reference to the canvas

    [SerializeField] private GameManager gameManager; // Reference to GameManager
    [SerializeField] private Button playagain; // Reference to the play again button
    [SerializeField] private Button quit; // Reference to the quit button
    [SerializeField] private TextMeshProUGUI score; // Reference to the score text
    // [SerializeField] private TMP_Text scoreUIText;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        canvas.SetActive(false); // Hide the canvas initially
    }

    public void Appear()
    {
        // Extract and reformat the score text
        string updatedUI = "Score:\n" + gameManager.totalScore;

        // Update the canvas score text
        canvas.SetActive(true);
        score.text = updatedUI;

        // Position the canvas relative to the camera
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;

        Time.timeScale = 0;
    }

    public void Quit()
    {
        // Quit the application
        Application.Quit();
    }

    public void PlayAgain()
    {
        if (canvas != null)
        {
            canvas.SetActive(false); // Hide the canvas
        }
        Time.timeScale = 1;
        SceneManager.LoadScene("kitchen"); // Load the "kitchen" scene
    }
}
