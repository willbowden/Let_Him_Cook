using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    public Sprite recipeImage; // Store sprite for the recipe image
    [TextArea(3, 10)]           // Allows multiline text input in the Inspector
    public string recipeText;  // Multiline string for the recipe text
}
