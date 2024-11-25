using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BurgerPlate : MonoBehaviour
{
  [SerializeField]
  private Transform nextAttachPoint;
  private List<GameObject> contents = new();
  private XRSocketInteractor socket;

  void Start()
  {
    if (nextAttachPoint == null)
    {
      Debug.LogError("BurgerPlate does not have the required intial attach point defined. Disabling this plate.");
      this.enabled = false;
    }

    socket = GetComponent<XRSocketInteractor>();
    socket.selectEntered.AddListener(IngredientAdded);
    socket.selectExited.AddListener(IngredientRemoved);
    socket.attachTransform = nextAttachPoint;
  }

  private void IngredientAdded(SelectEnterEventArgs args)
  {
    GameObject obj = args.interactableObject.transform.gameObject;
    
  }

  private void IngredientRemoved(SelectExitEventArgs args)
  {
    GameObject obj = args.interactableObject.transform.gameObject;

  }
}