using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCastScanner : MonoBehaviour
{

    public GameObject ARCamera;
    public GameObject ARObjectStorage;
    public Button LeaveAR, ScanAR;
    public GameObject playerColl;
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

        //Debug.Log("here");
        RaycastHit hit;
        string name = null;
        bool success = false;
        if (Physics.Raycast(ARCamera.transform.position, ARCamera.transform.forward, out hit, 1000))
        {
            Debug.Log("Scanned!");
            name = hit.collider.transform.parent.gameObject.name;
            DestroyObject(hit.collider.transform.parent.gameObject);
            GameObject.Find("GameDriver").GetComponent<GameDriver>().ScorePoint();
            GameObject.Find("Score").GetComponent<Text>().text = "Score: " + GameObject.Find("GameDriver").GetComponent<GameDriver>().GetScore() + "/10";
            GameObject x = GameObject.Find(name);
            DestroyObject(x);
            playerColl.GetComponent<Collision>().RemoveCollision(name);
            success = true;
        }
        if (ARObjectStorage.transform.childCount == 1 && success)
        {
            LeaveAR.gameObject.SetActive(true);
            ScanAR.gameObject.SetActive(false);
        }
        
    }
}
