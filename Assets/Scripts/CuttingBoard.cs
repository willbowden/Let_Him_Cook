using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.UI;

public class CuttingBoard : MonoBehaviour
{
    public Image progressBar;
    private GameObject content;
    private int chops = 0;

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Chop();
        }
    }

    void Awake()
    {
        XRSocketInteractor socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(IngredientAdded);
        socket.selectExited.AddListener(IngredientRemoved);
        progressBar.gameObject.SetActive(false);
    }

    public void IngredientAdded(SelectEnterEventArgs args)
    {
        GameObject obj = args.interactableObject.transform.gameObject;

        // Should be controlled by the Interactable Layer Mask, but worth checking just in case.
        if (obj.GetComponent<CuttableIngredient>() != null)
        {
            content = obj;
            progressBar.gameObject.SetActive(true);

        }
    }

    public void IngredientRemoved(SelectExitEventArgs args)
    {
        ResetProgressAndContent();
    }

    private void ResetProgressAndContent()
    {
        content = null;
        chops = 0;
        progressBar.rectTransform.localScale = new Vector3 (0, 1, 1);
        progressBar.gameObject.SetActive(false);
    }

    private void Chop()
    {
        int chopsRequired = content.GetComponent<CuttableIngredient>().chopsToCut;
        chops += 1;
        if (chops >= chopsRequired)
        {
            content.transform.GetPositionAndRotation(out var instancePosition, out var instanceRotation);
            GameObject cutPrefab = content.GetComponent<CuttableIngredient>().cutVersionPrefab;
            Destroy(content);
            Instantiate(cutPrefab, new Vector3(0, 0.02f, 0) + instancePosition, instanceRotation);

            ResetProgressAndContent();
        }
        else
        {
            float newSize = chops / chopsRequired;
            progressBar.rectTransform.localScale = new Vector3 (newSize, 1, 1);
            print(newSize);
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        Collider other = col.collider;
        if (other.gameObject.name == "Knife" && content != null)
        {
            Chop();
        }
    }
}
