using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnBoundary : MonoBehaviour
{
    [SerializeField]
    private float despawnDelay = 10f;
    private LayerMask layerMask;

    void Start()
    {
        layerMask = LayerMask.GetMask("Ingredient", "Tools", "StackedIngredient", "TopStackIngredient");
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            Destroy(other.gameObject, despawnDelay);
        }
    }
}
