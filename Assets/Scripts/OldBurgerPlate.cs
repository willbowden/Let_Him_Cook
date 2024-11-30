// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;
// using UnityEngine.XR.Interaction.Toolkit.Interactors;

// public class BurgerPlate : MonoBehaviour
// {
//   [SerializeField]
//   // private Transform nextAttachPoint;
//   private Stack<GameObject> contents = new();
//   [SerializeField]
//   private GameObject socketPrefab;
//   [SerializeField]
//   private GameObject burgerStack;

//   private GameObject bottomSocket;

//   void Start()
//   {
//     // if (nextAttachPoint == null)
//     // {
//     //   Debug.LogError("BurgerPlate does not have the required intial attach point defined. Disabling this plate.");
//     //   this.enabled = false;
//     // }

//     bottomSocket = transform.Find("StackSocket").gameObject;

//     var socketInteractor = bottomSocket.GetComponent<XRSocketInteractor>();
//     socketInteractor.selectEntered.AddListener(IngredientAdded);
//     socketInteractor.selectExited.AddListener(IngredientRemoved);
//     // socketInteractor.attachTransform = nextAttachPoint;

//   }

//   private void IngredientAdded(SelectEnterEventArgs args)
//   {
//     GameObject ingredient = args.interactableObject.transform.gameObject;
//     ingredient.transform.SetParent(burgerStack.transform);

//     Vector3 offset = new(0, ingredient.GetComponent<MeshRenderer>().bounds.size.y, 0);

//     GameObject newSocket = Instantiate(socketPrefab, ingredient.transform);
//     newSocket.transform.position += offset;
//     newSocket.name = "StackSocket";

//     var newSocketInteractor = newSocket.GetComponent<XRSocketInteractor>();
//     newSocketInteractor.attachTransform = newSocket.transform;
//     newSocketInteractor.selectEntered.AddListener(IngredientAdded);
//     newSocketInteractor.selectExited.AddListener(IngredientRemoved);

//     string parentName = transform.parent.gameObject.name;

//     print(string.Format("Socket on {0} ENTERED by {1}", parentName, ingredient.name));

//     if (contents.Count > 0)
//     {
//       GameObject previousIngredient = contents.Peek();
//       GameObject socket = previousIngredient.transform.Find("StackSocket").gameObject;

//       HandleAddition(previousIngredient, socket);
//     }
//     else
//     {
//       HandleAddition(null, bottomSocket);
//     }

//     contents.Push(ingredient);
//   }

//   private void IngredientRemoved(SelectExitEventArgs args)
//   {

//     GameObject ingredient = args.interactableObject.transform.gameObject;

//     string parentName = transform.parent.gameObject.name;

//     print(string.Format("Socket on {0} EXITED by {1}", parentName, ingredient.name));

//     if (ingredient != contents.Peek()) { return; }

//     ingredient.transform.SetParent(null);

//     GameObject socket = ingredient.transform.Find("StackSocket").gameObject;
//     Destroy(socket);

//     contents.Pop();

//     if (contents.Count > 0)
//     {
//       GameObject topIngredient = contents.Peek();
//       GameObject topSocket = topIngredient.transform.Find("StackSocket").gameObject;

//       HandleRemoval(topIngredient, topSocket);
//     }
//     else
//     {
//       HandleRemoval(null, bottomSocket);
//     }
//   }

//   private void HandleRemoval(GameObject ingredient, GameObject stackSocket)
//   {
//     XRSocketInteractor socket = stackSocket.GetComponent<XRSocketInteractor>();
//     if (socket == null) { return; }

//     if (ingredient != null)
//     {
//       ingredient.GetComponent<XRGrabInteractable>().enabled = true;
//       // ingredient.GetComponent<Collider>().enabled = true;
//     }

//     print(string.Format("Enabling Socket Collider {0}", stackSocket.transform.parent.gameObject.name));

//     socket.GetComponent<BoxCollider>().enabled = true;
//   }

//   private void HandleAddition(GameObject ingredient, GameObject stackSocket)
//   {
//     XRSocketInteractor socket = stackSocket.GetComponent<XRSocketInteractor>();
//     if (socket == null) { return; }

//     if (ingredient != null)
//     {
//       ingredient.GetComponent<XRGrabInteractable>().enabled = false;
//       // ingredient.GetComponent<Collider>().enabled = false;
//     }

//     print(string.Format("Disabling Socket Collider {0}", stackSocket.transform.parent.gameObject.name));

//     socket.GetComponent<BoxCollider>().enabled = false;
//   }
// }