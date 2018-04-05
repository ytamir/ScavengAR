using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARObjectGenerator : MonoBehaviour {

    public GameObject obman;
    public GameObject player;
    public GameObject locman;
    public GameObject ARCam;
    public GameObject prefab;
    public GameObject ARView;

	// Use this for initialization
	void Start () {
		
	}

    private double degreesToRadians(double degrees)
    {
        return degrees * Mathd.PI / 180;
    }

    public void GenerateObjects()
    {
        List<string> curCollisions = player.GetComponent<Collision>().getCurrentCollisions();
        Vector2d playerLoc = locman.GetComponent<PositionWithLocationProvider>().getCurrentLoc();
        foreach (var name in curCollisions)
        {
            Vector2d objLoc = obman.GetComponent<ObjectManager>().getObjectCoords(name);
            Debug.Log("objloc: " + objLoc);
            Debug.Log("playerloc: " + playerLoc);
            double LatDistance = Mathd.Abs(Mathd.Abs(objLoc.x) - Mathd.Abs(playerLoc.x)) * 111000;
            double LongDistance = Mathd.Abs(Mathd.Abs(objLoc.y) - Mathd.Abs(playerLoc.y)) * (Mathd.Cos(objLoc.x) * 111000);
            Debug.Log("LR: " + LongDistance + " UD: " + LatDistance);
            double newPosX, newPosY;
            if(objLoc.x < playerLoc.x)
            {
                Debug.Log("Spawning South");
                Debug.Log("Obj Loc x: " + objLoc.x + " Player Loc x: " + playerLoc.x);
                newPosY = ARCam.transform.position.z - (LatDistance);
            } else
            {
                Debug.Log("Spawning North");
                Debug.Log("Obj Loc x: " + objLoc.x + " Player Loc x: " + playerLoc.x);
                newPosY = ARCam.transform.position.z + (LatDistance);
            }
            if(objLoc.y < playerLoc.y)
            {
                Debug.Log("Spawning West");
                Debug.Log("Obj Loc x: " + objLoc.y + " Player Loc x: " + playerLoc.y);
                newPosX = ARCam.transform.position.x - (LongDistance);
            } else
            {
                Debug.Log("Spawning East");
                Debug.Log("Obj Loc x: " + objLoc.y + " Player Loc x: " + playerLoc.y);
                newPosX = ARCam.transform.position.x + (LongDistance);
            }
            GameObject plsWork = Instantiate(prefab, new Vector3((float)newPosX, ARCam.transform.position.y, (float)newPosY), Quaternion.identity);
            plsWork.name = name;
            plsWork.transform.parent = this.transform;

        }
        var newRotation = Quaternion.Euler(0, -Input.compass.trueHeading, 0);
        this.transform.rotation = newRotation;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
