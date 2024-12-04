using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject spawn;
    [SerializeField]
    private Transform spawnPoint;
    private int objectsInZone = 0;
    private bool spawnCountdownRunning = false;
    
    
    void Start()
    {
        if (spawn.GetNamedChild("AttachPoint") == null)
        {
            Debug.LogError($"The spawnable object {spawn.name} does not have the required \"AttachPoint\" transform as a child. Disabling its spawner.");
            this.enabled = false;
        }   
        else
        {
            Spawn();            
        }
    }

    void Spawn()
    {
        GameObject spawned = Instantiate(spawn, spawnPoint.position, spawnPoint.rotation);

        Vector3 initialPosition = spawned.transform.position;
        Quaternion initialRotation = spawned.transform.rotation;

        Vector3 positionDelta = initialPosition - spawnPoint.position;
        Quaternion rotationDelta = initialRotation * Quaternion.Inverse(spawnPoint.rotation);
        spawned.transform.SetPositionAndRotation(initialPosition + positionDelta, initialRotation * rotationDelta);
    }

    IEnumerator SpawnCountdown()
    {
        spawnCountdownRunning = true;

        yield return new WaitForSeconds(4);

        Spawn();

        spawnCountdownRunning = false;
    }

    void OnTriggerEnter(Collider col)
    {
        print(string.Format("Collider {0} entering", col.gameObject.name));
        if (col.gameObject.name == spawn.name)
        {
            objectsInZone += 1;
        }
    }

    void OnTriggerExit(Collider col)
    {
        print(string.Format("Collider {0} exiting", col.gameObject.name));
        if (col.gameObject.name == spawn.name)
        {
            objectsInZone -= 1;
            if (objectsInZone == 0 && !spawnCountdownRunning)
            {
                StartCoroutine(SpawnCountdown());
            }
        }
    }
}
