using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Order", menuName = "Order", order = 1)]
public class Order : ScriptableObject
{
    public Recipe recipe; // Store recipe
    public float timeInSeconds;  // line for time in seconds
}
