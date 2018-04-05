using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MiniMapCanvas : MonoBehaviour {

    public GameObject MiniMapCv;
    public GameObject ARView;
    public GameObject ARCamera;
    public GameObject MainCamera;
    public GameObject scanbtn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoToCamera()
    {
        ARView.SetActive(true);
        ARCamera.SetActive(true);
        MiniMapCv.SetActive(false);
    }

    public void GoToMiniMap()
    {
        MiniMapCv.SetActive(true);
        scanbtn.SetActive(true);
        ARView.SetActive(false);
        ARCamera.SetActive(false);
    }
}
