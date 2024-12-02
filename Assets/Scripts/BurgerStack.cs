using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BurgerStack : MonoBehaviour
{
    [SerializeField]
    private GameObject burgerPlate;

    private XRInteractionManager interactionManager;

    private List<XRBaseInteractor> presentInteractors = new();
    private bool isInTrigger = false;

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
            isInTrigger = true;
            presentInteractors.Add(nfi);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Controller_Base")
        {
            NearFarInteractor nfi = other.gameObject.GetComponent<ControllerCollision>().nearFarInteractor.GetComponent<NearFarInteractor>();
            presentInteractors.Remove(nfi);
            if (presentInteractors.Count == 0)
            {
                isInTrigger = false;
            }
        }
    }

    void Update()
    {
        if (isInTrigger && presentInteractors.Count > 0)
        {
            foreach (XRBaseInteractor interactor in presentInteractors)
            {
                if (interactor.isSelectActive)
                {
                    StackGrabbed(interactor);
                }
            }
        }
    }

    private void StackGrabbed(XRBaseInteractor interactor)
    {
        if (burgerPlate.TryGetComponent(out BurgerPlate plate))
        {
            if (plate.ContentCount <= 0) return;
        }

        plate.IngredientRemoved(interactor);
    }
}
