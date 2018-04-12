using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transitions : MonoBehaviour {

    public GameObject userNameText;
	// Use this for initialization
	void Start () {
		
	}

    public void goToMenu(GameObject targetMenu)
    {
        if(!userNameText.GetComponent<Text>().text.Equals(""))
        {
            this.gameObject.SetActive(false);
            targetMenu.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
