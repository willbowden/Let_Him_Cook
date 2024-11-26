using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReciepeController : MonoBehaviour
{
    [Header("List of Orders")]
    [SerializeField] private Reciepe[] reciepes; // Array of recipes
    private int reciepeIndex;

    [Header("UI Elements")]
    [SerializeField] private Image imageDisplay;  // Reference to the UI Image component
    [SerializeField] private TMP_Text textDisplay; // Reference to the TMP_Text component

    void Start()
    {
        reciepeIndex = 0;
        UpdateReciepeUI();
    }

    // Updates the UI with the current recipe
    private void UpdateReciepeUI()
    {
        if (reciepeIndex >= 0 && reciepeIndex < reciepes.Length)
        {
            imageDisplay.sprite = reciepes[reciepeIndex].reciepeImage;
            textDisplay.text = reciepes[reciepeIndex].reciepeText;
        }
        else
        {
            Debug.LogError("Reciepe index out of bounds!");
        }
    }

    // Navigates to the next recipe
    public void NextReciepe()
    {
        reciepeIndex = (reciepeIndex + 1) % reciepes.Length;
        UpdateReciepeUI();
    }

    // Navigates to the previous recipe
    public void PreviousReciepe()
    {
        reciepeIndex = (reciepeIndex - 1 + reciepes.Length) % reciepes.Length;
        UpdateReciepeUI();
    }
}
