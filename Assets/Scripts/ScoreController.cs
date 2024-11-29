using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text score; // Reference to the TMP_Text component

    private int scoreValue = 0; // Initialize score value to 0

    // Start is called before the first frame update
    private void Start()
    {
        // Parse the initial score value from the TMP_Text component, if needed
        if (score != null && !string.IsNullOrEmpty(score.text))
        {
            string[] splitText = score.text.Split(' ');
            if (splitText.Length > 1 && int.TryParse(splitText[1], out int parsedScore))
            {
                scoreValue = parsedScore;
            }
        }
    }

    // Adds the new score to the current score and updates the UI
    public void UpdateScore(int addedScore)
    {
        scoreValue += addedScore; // Add to the current score
        if (score != null)
        {
            score.text = "Score: " + scoreValue; // Update the UI text
        }
    }
}
