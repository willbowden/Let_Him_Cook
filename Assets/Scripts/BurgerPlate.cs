using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using Vector3 = UnityEngine.Vector3;


public class BurgerPlate : MonoBehaviour
{
  [SerializeField]
  private Stack<GameObject> contents = new();
  [SerializeField]
  private GameObject burgerStack;
  [SerializeField]
  private GameObject bottomAttachPoint;

  private GameObject nextAttachPoint;

  private LayerMask STACKED_MASK;
  private LayerMask NOTHING_MASK;
  private LayerMask INGREDIENT_MASK;

  void Start()
  {
    nextAttachPoint = bottomAttachPoint;
    STACKED_MASK = LayerMask.NameToLayer("StackedIngredient");
    NOTHING_MASK = LayerMask.NameToLayer("Nothing");
    INGREDIENT_MASK = LayerMask.NameToLayer("Ingredient");
  }

  private void OnTriggerEnter(Collider other)
  {
    XRGrabInteractable grab = other.gameObject.GetComponent<XRGrabInteractable>();
    if (grab != null)
    {
      grab.selectExited.AddListener(IngredientAdded);
    }
  }

  private void OnTriggerExit(Collider other)
  {
    XRGrabInteractable grab = other.gameObject.GetComponent<XRGrabInteractable>();
    if (grab != null)
    {
      grab.selectExited.RemoveListener(IngredientAdded);
    }
  }

  // TODO: Hologram or some indication of potential action.
  // TODO: Allow removal of ingredients.

  private void IngredientAdded(SelectExitEventArgs args)
  {
    GameObject ingredient = args.interactableObject.transform.gameObject;
    ingredient.transform.SetParent(burgerStack.transform);

    Rigidbody rb = ingredient.GetComponent<Rigidbody>();
    rb.isKinematic = true;
    rb.useGravity = false;

    ingredient.layer = STACKED_MASK;
    ingredient.GetComponent<Collider>().excludeLayers = STACKED_MASK;

    Transform ingredientAttachPoint = ingredient.transform.Find("AttachPoint");
    Vector3 offset = ingredient.transform.position - ingredientAttachPoint.position;
    Vector3 newPosition = nextAttachPoint.transform.position + offset;

    Quaternion newRotation = Quaternion.LookRotation(nextAttachPoint.transform.forward, nextAttachPoint.transform.up);

    ingredient.transform.SetPositionAndRotation(newPosition, newRotation);

    args.interactableObject.selectEntered.AddListener(IngredientRemoved);
    // Push contents to stack. Disable grabbing of objects below.
  }

  private void IngredientRemoved(SelectEnterEventArgs args)
  {
    GameObject ingredient = args.interactableObject.transform.gameObject;
    ingredient.transform.SetParent(null);

    Rigidbody rb = ingredient.GetComponent<Rigidbody>();
    rb.isKinematic = false;
    rb.useGravity = true;

    ingredient.layer = INGREDIENT_MASK;
    ingredient.GetComponent<Collider>().excludeLayers = NOTHING_MASK;

    args.interactableObject.selectEntered.RemoveListener(IngredientRemoved);

    // Pop from stack. Enable grabbing of object below.
  }
}