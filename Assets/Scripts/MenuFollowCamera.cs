using UnityEngine;

public class MenuFollowCamera : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera
    public float distanceFromCamera = 3f; // Distance from the camera to place the canvas

    void Update()
    {
        if (mainCamera != null)
        {
            // Position the canvas in front of the camera
            transform.position = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;

            // Make sure the canvas always faces the camera
            transform.LookAt(mainCamera.transform);
        }
    }
}
