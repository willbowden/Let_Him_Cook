using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BurgerStack : MonoBehaviour
{
    [SerializeField]
    private GameObject burgerPlate;

    private XRInteractionManager interactionManager;

    void Start()
    {
        interactionManager = FindFirstObjectByType<XRInteractionManager>();
        if (interactionManager == null)
        {
            Debug.LogError("Couldn't find Interaction Manager!");
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Controller_Base")
        {
           NearFarInteractor nfi = other.gameObject.GetComponent<ControllerCollision>().nearFarInteractor.GetComponent<NearFarInteractor>();
           nfi.selectEntered.AddListener(StackGrabbed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Controller_Base")
        {
           NearFarInteractor nfi = other.gameObject.GetComponent<ControllerCollision>().nearFarInteractor.GetComponent<NearFarInteractor>();
           nfi.selectEntered.RemoveListener(StackGrabbed);
        }
    }

    private void StackGrabbed(SelectEnterEventArgs args)
    {
        if (burgerPlate.TryGetComponent(out BurgerPlate plate))
        {
            if (plate.ContentCount <= 0) return;
        }

        GameObject topIngredient = plate.TopIngredient;
        XRGrabInteractable grab = topIngredient.GetComponent<XRGrabInteractable>();

        // XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;
        
        print(string.Format("Grabbing {0}!", topIngredient.name));

        args.interactableObject = grab;
        plate.IngredientRemoved(args);

        // grab.interactionManager.SelectEnter((IXRSelectInteractor)interactor, grab);
    }
}
