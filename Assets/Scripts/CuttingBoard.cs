using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CuttingBoard : MonoBehaviour
{
    private GameObject content;

    void Awake()
    {
        XRSocketInteractor socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(IngredientAdded);
        socket.selectExited.AddListener(IngredientRemoved);
    }

    public void IngredientAdded(SelectEnterEventArgs args)
    {
        GameObject obj = args.interactableObject.transform.gameObject;
        content = obj;
    }

    public void IngredientRemoved(SelectExitEventArgs args)
    {
        content = null;
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.name == "Knife" && content != null) {
            Destroy(content);
        }
    }
}
