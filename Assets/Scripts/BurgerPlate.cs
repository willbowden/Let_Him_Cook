using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BurgerPlate : MonoBehaviour
{
  [SerializeField]
  // private Transform nextAttachPoint;
  private Stack<GameObject> contents = new();
  [SerializeField]
  private GameObject socketPrefab;
  [SerializeField]
  private GameObject burgerStack;

  private GameObject bottomSocket;

  void Start()
  {
    // if (nextAttachPoint == null)
    // {
    //   Debug.LogError("BurgerPlate does not have the required intial attach point defined. Disabling this plate.");
    //   this.enabled = false;
    // }

    bottomSocket = transform.Find("StackSocket").gameObject;

    var socketInteractor = bottomSocket.GetComponent<XRSocketInteractor>();
    socketInteractor.selectEntered.AddListener(IngredientAdded);
    socketInteractor.selectExited.AddListener(IngredientRemoved);
    // socketInteractor.attachTransform = nextAttachPoint;

  }

  private void IngredientAdded(SelectEnterEventArgs args)
  {
    GameObject ingredient = args.interactableObject.transform.gameObject;
    ingredient.transform.SetParent(burgerStack.transform);

    Vector3 offset = new(0, ingredient.GetComponent<MeshRenderer>().bounds.size.y, 0);

    GameObject newSocket = Instantiate(socketPrefab, ingredient.transform);
    newSocket.transform.position += offset;
    newSocket.name = "StackSocket";

    var newSocketInteractor = newSocket.GetComponent<XRSocketInteractor>();
    newSocketInteractor.attachTransform = newSocket.transform;
    newSocketInteractor.selectEntered.AddListener(IngredientAdded);
    newSocketInteractor.selectExited.AddListener(IngredientRemoved);

    print(string.Format("Added new socket. Selecting: {0}, Hovering: {1}", newSocketInteractor.allowSelect, newSocketInteractor.allowHover));

    if (contents.Count > 0)
    {
      GameObject previousIngredient = contents.Peek();
      GameObject socket = previousIngredient.transform.Find("StackSocket").gameObject;
     
      EnableSocket(socket);
    }
    else
    {
      DisableSocket(bottomSocket);
    }

    contents.Push(ingredient);
  }

  private void IngredientRemoved(SelectExitEventArgs args)
  {

    GameObject ingredient = args.interactableObject.transform.gameObject;
    print(string.Format("Removing ingredient {0}", ingredient.name));

    if (ingredient != contents.Peek()) { return; }

    ingredient.transform.SetParent(null);

    GameObject socket = ingredient.transform.Find("StackSocket").gameObject;
    Destroy(socket);

    contents.Pop();

    if (contents.Count > 0)
    {
      GameObject topIngredient = contents.Peek();
      GameObject topSocket = topIngredient.transform.Find("StackSocket").gameObject;

      EnableSocket(topSocket);
    }
    else
    {
      EnableSocket(bottomSocket);
    }
  }

  private void EnableSocket(GameObject stackSocket)
  {
    XRSocketInteractor socket = stackSocket.GetComponent<XRSocketInteractor>();
    if (socket == null) { return; }

    // socket.selectEntered.AddListener(IngredientAdded);
    // socket.selectExited.AddListener(IngredientRemoved);
    socket.allowHover = true;
    socket.allowSelect = true;
  }

  private void DisableSocket(GameObject stackSocket)
  {
    XRSocketInteractor socket = stackSocket.GetComponent<XRSocketInteractor>();
    if (socket == null) { return; }

    // socket.selectEntered.RemoveListener(IngredientAdded);
    // socket.selectExited.RemoveListener(IngredientRemoved);
    socket.allowHover = false;
    socket.allowSelect = false;
  }
}