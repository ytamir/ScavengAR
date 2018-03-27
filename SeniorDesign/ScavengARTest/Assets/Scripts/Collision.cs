using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    public GameObject btn;

    // Use this for initialization
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        btn.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name);
        btn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
