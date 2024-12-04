using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Attachment;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using Vector3 = UnityEngine.Vector3;

public class BurgerPlate : MonoBehaviour
{
  [SerializeField]
  private GameObject socketPrefab;

  private Stack<GameObject> contents = new();
  private GameObject bottomSocket;
  // private InteractionLayerMask INGREDIENT_MASK;
  // private InteractionLayerMask STACK_INGREDIENT_MASK;
  private LayerMask INGREDIENT_PHYSICS_MASK;
  private LayerMask STACK_INGREDIENT_PHYSICS_MASK;
  private LayerMask TOP_INGREDIENT_PHYSICS_MASK;

  public Stack<GameObject> GetContents()
  {
    return contents;
  }

  void Start()
  {
    bottomSocket = transform.Find("StackSocket").gameObject;

    var socketInteractor = bottomSocket.GetComponent<XRSocketInteractor>();
    socketInteractor.selectEntered.AddListener(IngredientAdded);
    socketInteractor.selectExited.AddListener(IngredientRemoved);

    // INGREDIENT_MASK = InteractionLayerMask.NameToLayer("Ingredients");
    // STACK_INGREDIENT_MASK = InteractionLayerMask.NameToLayer("StackedIngredients");
    INGREDIENT_PHYSICS_MASK = LayerMask.NameToLayer("Ingredient");
    STACK_INGREDIENT_PHYSICS_MASK = LayerMask.NameToLayer("StackedIngredient");
    TOP_INGREDIENT_PHYSICS_MASK = LayerMask.NameToLayer("TopStackIngredient");

    XRGrabInteractable plateGrab = GetComponent<XRGrabInteractable>();
    plateGrab.selectEntered.AddListener(PlateGrabbed);
    plateGrab.selectExited.AddListener(PlateReleased);
  }

  private void IngredientAdded(SelectEnterEventArgs args)
  {
    GameObject ingredient = args.interactableObject.transform.gameObject;
    if (ingredient == null) return;

    ingredient.GetComponent<Collider>().excludeLayers = STACK_INGREDIENT_PHYSICS_MASK;
    ingredient.layer = TOP_INGREDIENT_PHYSICS_MASK;

    GameObject previousSocket;

    if (contents.Count > 0)
    {
      GameObject previousIngredient = contents.Peek();
      previousIngredient.layer = STACK_INGREDIENT_PHYSICS_MASK;

      previousSocket = previousIngredient.transform.Find("StackSocket").gameObject;
    }
    else
    {
      previousSocket = bottomSocket;
    }

    previousSocket.GetComponent<XRSocketInteractor>().allowHover = false;

    StartCoroutine(CreateSocket(ingredient));

    // string parentName = transform.parent.gameObject.name;

    // print(string.Format("Socket on {0} ENTERED by {1}", parentName, ingredient.name));

    contents.Push(ingredient);
  }

  private IEnumerator CreateSocket(GameObject ingredient)
  {
    yield return new WaitForSeconds(0.1f);

    Vector3 ingredientSize = ingredient.GetComponent<MeshRenderer>().bounds.size;
    Transform attachPoint = ingredient.transform.Find("AttachPoint");

    Vector3 offset = new(0, ingredientSize.y, 0);

    GameObject newSocket = Instantiate(socketPrefab, ingredient.transform);
    newSocket.transform.position += offset;
    newSocket.name = "StackSocket";
    Vector3 newColliderSize = new(1, 0.5f, 1);
    BoxCollider bc = newSocket.GetComponent<BoxCollider>();
    bc.size = newColliderSize;
    Vector3 worldSize = newSocket.transform.TransformVector(newColliderSize);
    Vector3 worldCenter = newSocket.transform.TransformPoint(bc.center);
    Vector3 newCenter = new(worldCenter.x, (worldSize.y / 2) + attachPoint.position.y + ingredientSize.y, worldCenter.z);
    bc.center = newSocket.transform.InverseTransformPoint(newCenter);

    newSocket.transform.Find("AttachPoint").position = new Vector3(attachPoint.position.x, attachPoint.position.y + ingredientSize.y, attachPoint.position.z);

    var newSocketInteractor = newSocket.GetComponent<XRSocketInteractor>();
    newSocketInteractor.attachTransform = newSocket.transform.Find("AttachPoint");
    newSocketInteractor.selectEntered.AddListener(IngredientAdded);
    newSocketInteractor.selectExited.AddListener(IngredientRemoved);
  }

  private void IngredientRemoved(SelectExitEventArgs args)
  {
    GameObject ingredient = args.interactableObject.transform.gameObject;
    if (ingredient == null) return;

    GameObject socket = ingredient.transform.Find("StackSocket").gameObject;
    Destroy(socket);

    ingredient.GetComponent<Collider>().excludeLayers = 0;
    ingredient.layer = INGREDIENT_PHYSICS_MASK;

    // string parentName = transform.parent.gameObject.name;

    // print(string.Format("Socket on {0} EXITED by {1}", parentName, ingredient.name));

    contents.Pop();

    GameObject previousSocket;

    if (contents.Count > 0)
    {
      GameObject topIngredient = contents.Peek();
      topIngredient.layer = TOP_INGREDIENT_PHYSICS_MASK;
      previousSocket = topIngredient.transform.Find("StackSocket").gameObject;
    }
    else
    {
      previousSocket = bottomSocket;
    }

    previousSocket.GetComponent<XRSocketInteractor>().allowHover = true;

    // Vector3 originalSize = socketPrefab.GetComponent<BoxCollider>().size;
    // previousSocket.GetComponent<BoxCollider>().size = originalSize;
  }

  private void PlateGrabbed(SelectEnterEventArgs args)
  {
    if (contents.Count > 0)
    {
      GameObject topIngredient = contents.Peek();
      XRSocketInteractor socket = topIngredient.transform.Find("StackSocket").GetComponent<XRSocketInteractor>();
      socket.allowSelect = false;
      socket.allowHover = false;
    }
  }

  private void PlateReleased(SelectExitEventArgs args)
  {
    if (contents.Count > 0)
    {
      GameObject topIngredient = contents.Peek();
      XRSocketInteractor socket = topIngredient.transform.Find("StackSocket").GetComponent<XRSocketInteractor>();
      socket.allowSelect = true;
      socket.allowHover = true;
    }
  }
}