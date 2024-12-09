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
    [SerializeField] private Button playagain; // Reference to the play again button
    [SerializeField] private Button quit; // Reference to the quit button
    [SerializeField] private TextMeshProUGUI score; // Reference to the score text

    void Start()
    {
        canvas.SetActive(false);
    }

    public void Appear(int scoreValue)
    {
        canvas.SetActive(true);

        score.text = "Score:\n" + scoreValue;

        // Position the canvas
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;
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
        SceneManager.LoadScene("kitchen"); // Loads the "kitchen" scene
    }
}
