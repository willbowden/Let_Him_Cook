using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reciepe", menuName = "Reciepe", order = 1)]
public class Reciepe : ScriptableObject
{
    public Sprite reciepeImage; // Store sprite for the recipe image
    [TextArea(3, 10)]           // Allows multiline text input in the Inspector
    public string reciepeText;  // Multiline string for the recipe text
}
