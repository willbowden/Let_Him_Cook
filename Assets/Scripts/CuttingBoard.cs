using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.UI;
using System.Linq;

public class CuttingBoard : MonoBehaviour
{
    [SerializeField]
    private ProgressBar progressBar;
    [SerializeField]
    private List<AudioClip> audioClips;
    
    private AudioSource audioSource;
    private GameObject content;
    private int chops = 0;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        XRSocketInteractor socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(IngredientAdded);
        socket.selectExited.AddListener(IngredientRemoved);
        progressBar.Hide();
    }

    public void IngredientAdded(SelectEnterEventArgs args)
    {
        GameObject obj = args.interactableObject.transform.gameObject;

        // Should be controlled by the Interactable Layer Mask, but worth checking just in case.
        if (obj.GetComponent<CuttableIngredient>() != null)
        {
            content = obj;
            progressBar.Show();
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
        progressBar.Reset();
        progressBar.Hide();
    }

    private void Chop()
    {

        int chopsRequired = content.GetComponent<CuttableIngredient>().chopsToCut;
        chops += 1;
        float newSize = (float)chops / chopsRequired;
        progressBar.SetProgress(newSize);
        if (chops >= chopsRequired)
        {
            content.transform.GetPositionAndRotation(out var instancePosition, out var instanceRotation);
            GameObject cutPrefab = content.GetComponent<CuttableIngredient>().cutVersionPrefab;
            Destroy(content);
            Instantiate(cutPrefab, new Vector3(0, 0.02f, 0) + instancePosition, transform.rotation * Quaternion.Euler(new Vector3(0, 90, 0)));

            // Don't need to call ResetProgressAndContent() here as it is triggered by IngredientRemoved when we Destroy(content).
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Contains("Knife") && content != null)
        {
            if (audioClips.Count > 0 && audioSource != null)
            {
                int index = (int)UnityEngine.Random.Range(0, audioClips.Count);
                audioSource.PlayOneShot(audioClips[index]);
            }
            Chop();
        }
    }
}
