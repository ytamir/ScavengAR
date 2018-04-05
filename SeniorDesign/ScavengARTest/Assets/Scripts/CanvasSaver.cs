using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSaver : MonoBehaviour {

    private static CanvasSaver cs;
	// Use this for initialization
	void Start () {

        if(cs == null)
        {
            cs = this;
            DontDestroyOnLoad(this);
        }else
        {
            DestroyObject(gameObject);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
