// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;
// using UnityEngine.XR.Interaction.Toolkit.Interactors;
// using Vector3 = UnityEngine.Vector3;

// public class BurgerPlate : MonoBehaviour
// {
//   [SerializeField]
//   // private Transform nextAttachPoint;
//   private Stack<GameObject> contents = new();
//   [SerializeField]
//   private GameObject socket;
//   [SerializeField]
//   private GameObject burgerStack;
//   [SerializeField]
//   private GameObject bottomSocketPoint;

//   private XRSocketInteractor socketInteractor;

//   void Start()
//   {
//     socketInteractor = socket.GetComponent<XRSocketInteractor>();
//     socketInteractor.selectEntered.AddListener(IngredientAdded);
//     // socketInteractor.selectExited.AddListener(IngredientRemoved);
//   }

//   private void IngredientAdded(SelectEnterEventArgs args)
//   {
//     GameObject ingredient = args.interactableObject.transform.gameObject;
//     ingredient.transform.SetParent(burgerStack.transform);
    
//     XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;
//     interactor.interactionManager.CancelInteractableSelection(args.interactableObject);

//     string parentName = transform.parent.gameObject.name;

//     print(string.Format("Adding {1} to {0}", parentName, ingredient.name));

//     contents.Push(ingredient);
//   }

//   private IEnumerator MoveSocketAndDropObject(GameObject ingredient)
//   {
//     Vector3 offset = new(0, ingredient.GetComponent<MeshRenderer>().bounds.size.y, 0);
//     yield return new WaitForSeconds(0.1f);
//     socket.transform.position += offset;
//   }

//   // private void IngredientRemoved(SelectExitEventArgs args)
//   // {

//   //   GameObject ingredient = args.interactableObject.transform.gameObject;

//   //   string parentName = transform.parent.gameObject.name;

//   //   print(string.Format("Socket on {0} EXITED by {1}", parentName, ingredient.name));

//   //   if (ingredient != contents.Peek()) { return; }

//   //   ingredient.transform.SetParent(null);

//   //   GameObject socket = ingredient.transform.Find("StackSocket").gameObject;
//   //   Destroy(socket);

//   //   contents.Pop();

//   //   if (contents.Count > 0)
//   //   {
//   //     GameObject topIngredient = contents.Peek();
//   //     GameObject topSocket = topIngredient.transform.Find("StackSocket").gameObject;

//   //     HandleRemoval(topIngredient, topSocket);
//   //   }
//   //   else
//   //   {
//   //     HandleRemoval(null, bottomSocket);
//   //   }
//   // }

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