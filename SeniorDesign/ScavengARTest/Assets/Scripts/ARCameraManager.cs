using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCameraManager : MonoBehaviour {

    public GameObject arcanv;

	// Use this for initialization
	void Start () {
        arcanv.SetActive(false);
        this.gameObject.SetActive(false);
	}

    public void GetDeviceOrientation()
    {
        Debug.Log("Device Orientation (Magnetic): " + Input.compass.magneticHeading);
        Debug.Log("Device Orientation (True): " + Input.compass.trueHeading);
    }

    // Update is called once per frame
    void Update () {
        /*Debug.Log("Rotation: " + this.transform.rotation);
        Debug.Log("Position: " + this.transform.position);*/
	}
}
