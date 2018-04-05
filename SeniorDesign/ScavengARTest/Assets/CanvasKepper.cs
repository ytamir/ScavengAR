using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasKepper : MonoBehaviour {

    private static CanvasKepper cv;

	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        DontDestroyOnLoad(this);
        if(cv == null)
        {
            cv = this;
        } else
        {
            DestroyObject(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
