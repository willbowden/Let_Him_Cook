using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingGrill : MonoBehaviour
{
    [SerializeField]
    private GameObject progressBarPrefab; // Prefab for the progress bar
    [SerializeField]
    private float cookingTime = 5f; // Time required to cook each patty

    private Dictionary<GameObject, ProgressBar> pattyToProgressBar = new Dictionary<GameObject, ProgressBar>(); // Map each patty to its progress bar
    private Dictionary<GameObject, float> pattyCookingProgress = new Dictionary<GameObject, float>(); // Map each patty to its cooking progress

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.name == "PattyRaw" || other.gameObject.name == "PattyCooked") && !pattyToProgressBar.ContainsKey(other.gameObject))
        {
            AddPattyToGrill(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (pattyToProgressBar.ContainsKey(other.gameObject))
        {
            RemovePattyFromGrill(other.gameObject);
        }
    }

    private void AddPattyToGrill(GameObject patty)
    {
        // Create and position a progress bar for this patty
        GameObject progressBarInstance = Instantiate(progressBarPrefab, patty.transform);
        // progressBarInstance.transform.SetParent(, false);
        progressBarInstance.transform.position = patty.transform.position + new Vector3(0, 0.2f, 0);
        progressBarInstance.transform.rotation = Quaternion.Euler(0, 180, 0);
        progressBarInstance.transform.localScale = new Vector3(1, 1, 1);

        // Initialize progress tracking
        pattyToProgressBar[patty] =  progressBarInstance.GetComponent<ProgressBar>();
        pattyCookingProgress[patty] = 0f;

        pattyToProgressBar[patty].Show();

        // Start the cooking process
        StartCoroutine(CookPatty(patty));
    }

    private void RemovePattyFromGrill(GameObject patty)
    {
        if (pattyToProgressBar.TryGetValue(patty, out ProgressBar progressBar))
        {
            Destroy(progressBar.gameObject);
        }

        pattyToProgressBar.Remove(patty);
        pattyCookingProgress.Remove(patty);
    }

    private IEnumerator CookPatty(GameObject patty)
    {
        ProgressBar progressBar = pattyToProgressBar[patty];

        while (pattyCookingProgress[patty] < 1f)
        {
            pattyCookingProgress[patty] += Time.deltaTime / cookingTime;
            progressBar.SetProgress(Mathf.Clamp01(pattyCookingProgress[patty]));
            yield return null;
        }

        FinishCooking(patty);
    }

    private void FinishCooking(GameObject patty)
    {
        // Replace raw patty with cooked version
        patty.transform.GetPositionAndRotation(out var instancePosition, out var instanceRotation);
        GameObject cookedPrefab = patty.GetComponent<GrillableIngredient>().cookedVersionPrefab;

        // Clean up
        RemovePattyFromGrill(patty);
        Destroy(patty);

        Instantiate(cookedPrefab, instancePosition, instanceRotation);
    }
}
