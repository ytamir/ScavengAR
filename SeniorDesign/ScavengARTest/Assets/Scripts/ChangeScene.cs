using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NewScene(string sceneName)
    {
        NetworkManager.singleton.ServerChangeScene(sceneName);
        //SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
