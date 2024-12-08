using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFaceUser : MonoBehaviour
{
    private Camera mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null) {
            enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCamera.transform);
    }
}
