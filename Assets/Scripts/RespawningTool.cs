using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningTool : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    [SerializeField]
    private GameObject prefabOfSelf;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void OnDestroy()
    {
        if (!this.gameObject.scene.isLoaded) return;
        
        GameObject ob = Instantiate(prefabOfSelf, initialPosition, initialRotation);
        ob.name = prefabOfSelf.name;
        ob.GetComponent<RespawningTool>().prefabOfSelf = prefabOfSelf;
    }
}
