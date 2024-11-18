using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.UI;

public class CuttingBoard : MonoBehaviour
{
    public Scrollbar progressBar;
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

        // Should be controlled by the Interactable Layer Mask, but worth checking just in case.
        if (obj.GetComponent<CuttableIngredient>() != null) {
            content = obj;
        }
    }

    public void IngredientRemoved(SelectExitEventArgs args)
    {
        content = null;
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.name == "Knife" && content != null) {
            Vector3 instancePosition = content.transform.position;
            Quaternion instanceRotation = content.transform.rotation;
            GameObject cutPrefab = content.GetComponent<CuttableIngredient>().cutVersionPrefab;
            Destroy(content);
            Instantiate(cutPrefab, new Vector3(0, 0.02f, 0) + instancePosition, instanceRotation);

        }
    }
}
