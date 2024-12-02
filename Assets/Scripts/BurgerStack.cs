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

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void StackGrabbed(SelectEnterEventArgs args)
    {
        print("GRABBED STACK!");
        // if (burgerPlate.TryGetComponent(out BurgerPlate plate))
        // {
        //     if (plate.ContentCount <= 0) return;
        // }

        // GameObject topIngredient = plate.TopIngredient;
        // XRGrabInteractable grab = topIngredient.GetComponent<XRGrabInteractable>();

        // XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;
        // grab.interactionManager.SelectEnter((IXRSelectInteractor)interactor, grab);
    }
}
