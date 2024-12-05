using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ServiceBell : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private AudioSource bellSound;

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        bellSound = GetComponent<AudioSource>();
        if (bellSound == null)
        {
            Debug.LogError("No AudioSource found on this GameObject.");
        }
        gameManager = FindObjectOfType<GameManager>();

    }

    // For mouse interaction
    void OnMouseDown()
    {
        PlaySound();
        gameManager.OrderSubmitted();

    }

    // Method to handle poke interaction
    public void OnPoke()
    {
        print("POKED!");
        PlaySound();
        gameManager.OrderSubmitted();

    }

    // Shared method to play the bell sound
    public void PlaySound()
    {
        if (bellSound != null)
        {
            bellSound.PlayOneShot(bellSound.clip);
        }
    }
}
