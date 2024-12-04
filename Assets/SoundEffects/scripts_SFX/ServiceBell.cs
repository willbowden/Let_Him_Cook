using UnityEngine;

public class ServiceBell : MonoBehaviour
{
    [SerializeField] private CheckPlate CheckPlateController; // Reference to OrderController
    [SerializeField] private GameManager gameManager;

    private AudioSource bellSound;

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        bellSound = GetComponent<AudioSource>();
        CheckPlateController = FindObjectOfType<CheckPlate>();
        gameManager = FindObjectOfType<GameManager>();

    }

    // For mouse interaction
    void OnMouseDown()
    {
        PlaySound();
        CheckPlateController.CheckOrders();
        gameManager.OrderSubmitted();
    }

    // For VR interaction, you can call this method from a VR controller script
    public void PlaySound()
    {
        if (bellSound != null && !bellSound.isPlaying)
        {
            bellSound.Play();
        }
    }
}
