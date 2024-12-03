using UnityEngine;

public class ServiceBell : MonoBehaviour
{
    private AudioSource bellSound;

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        bellSound = GetComponent<AudioSource>();
    }

    // For mouse interaction
    void OnMouseDown()
    {
        PlaySound();
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
