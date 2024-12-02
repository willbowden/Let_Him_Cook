using UnityEngine;
using UnityEngine.InputSystem;

public class CanvasInteraction : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera
    public float distanceFromCamera = 0.6f; // Distance from the camera to place the canvas

    public GameObject canvas; // Assign your Canvas in the Inspector
    public InputActionReference homeButtonAction; // Reference the Input Action

    private void OnEnable()
    {

        if (canvas != null || mainCamera != null || homeButtonAction != null) {
            // Enable the action and subscribe to the performed event
            homeButtonAction.action.performed += OnHomeButtonPressed;
            homeButtonAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        homeButtonAction.action.performed -= OnHomeButtonPressed;
        homeButtonAction.action.Disable();
    }

    private void OnHomeButtonPressed(InputAction.CallbackContext context)
    {
        // Check if the game is currently running or paused
        if (Time.timeScale == 1) // Game is running
        {
            Time.timeScale = 0; // Pause the game

            transform.position = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;

            // Make sure the canvas always faces the camera
            transform.LookAt(mainCamera.transform);
            transform.Rotate(0, 180, 0);
            
            canvas.SetActive(true); // Show the pause menu
        }
        else // Game is already paused
        {
            Time.timeScale = 1; // Resume the game
            canvas.SetActive(false); // Hide the pause menu
        }
    }

}
