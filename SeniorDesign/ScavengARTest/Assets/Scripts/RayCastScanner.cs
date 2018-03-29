using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastScanner : MonoBehaviour
{

    // Use this for initialization

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {



    }

    public void doRayCast()
    {
        Transform cam = Camera.main.transform;
        //Debug.Log("here");
        RaycastHit hit;
        Debug.Log("here");
        if (Physics.Raycast(cam.position, cam.forward, out hit, 500))
        {
                Debug.Log("Scanned!");
                DestroyObject(hit.collider.gameObject);
        }
    }
}
