using System.Collections.Generic;
using UnityEngine;

public class CheckPlate : MonoBehaviour
{
    public Collider checkArea;
    public string checkBtn = "Submit";
    public List<string> activeOrders = new List<string>();
    public int fullPts = 100;
    public int halfPts = 50;
    public int emptyPts = 0;

    private void Update()
    {
        if (Input.GetButtonDown(checkBtn) && activeOrders.Count > 0)
        {
            CheckOrder();
        }
    }

    private void CheckOrder()
    {
        Collider[] objsInArea = Physics.OverlapBox(checkArea.bounds.center, checkArea.bounds.extents, checkArea.transform.rotation, LayerMask.GetMask("Order"));
        List<string> ingredientsOnPlate = new List<string>();

        foreach (var obj in objsInArea)
        {
            if (obj.CompareTag("Ingredient"))
            {
                ingredientsOnPlate.Add(obj.name);
            }
        }

        if (ingredientsOnPlate.Count == 0)
        {
            Debug.Log("Empty Plate");
            // GameManager.Instance.AddScore(emptyPts);
        }

        string[] requiredIngredients = activeOrders[0].Split(',');
        int matchedIngredients = 0;

        foreach (var ingredient in requiredIngredients)
        {
            if (ingredientsOnPlate.Contains(ingredient))
            {
                matchedIngredients++;
            }
        }

        if (matchedIngredients == requiredIngredients.Length)
        {
            Debug.Log("Full Plate");
            // GameManager.Instance.AddScore(fullPts);
            activeOrders.RemoveAt(0);
        }
        else if (matchedIngredients > 0)
        {
            Debug.Log("Half Plate");
            // GameManager.Instance.AddScore(halfPts);
            activeOrders.RemoveAt(0);
        }
        else
        {
            Debug.Log("No Match Plate");
            // GameManager.Instance.AddScore(emptyPts);
        }
    }


}