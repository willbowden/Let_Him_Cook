using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeController : MonoBehaviour
{
    [Header("List of Orders")]
    [SerializeField] private Recipe[] recipes; // Array of recipes
    private int recipeIndex;

    [Header("UI Elements")]
    [SerializeField] private Image imageDisplay;  // Reference to the UI Image component
    [SerializeField] private TMP_Text textDisplay; // Reference to the TMP_Text component

    void Start()
    {
        recipeIndex = 0;
        UpdateRecipeUI();
    }

    // Updates the UI with the current recipe
    private void UpdateRecipeUI()
    {
        if (recipeIndex >= 0 && recipeIndex < recipes.Length)
        {
            imageDisplay.sprite = recipes[recipeIndex].recipeImage;
            textDisplay.text = recipes[recipeIndex].recipeText;
        }
        else
        {
            Debug.LogError("Recipe index out of bounds!");
        }
    }

    // Navigates to the next recipe
    public void NextRecipe()
    {
        recipeIndex = (recipeIndex + 1) % recipes.Length;
        UpdateRecipeUI();
    }

    // Navigates to the previous recipe
    public void PreviousRecipe()
    {
        recipeIndex = (recipeIndex - 1 + recipes.Length) % recipes.Length;
        UpdateRecipeUI();
    }
}
