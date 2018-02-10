using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AugmentedScript : MonoBehaviour
{

    private float originalLatitude;
    private float originalLongitude;
    private float currentAlititude;
    private float currentLongitude;
    private float currentLatitude;
    private float targetLongitude;
    private float targetLatitude;

    private GameObject distanceTextObject;
    private GameObject oglatTextObject;
    private GameObject oglonTextObject;
    private GameObject curlatTextObject;
    private GameObject curlonTextObject;
    private GameObject numupdatesTextObject;
    private GameObject cube;

    public int numupdates = 0;

    private double distance;

    private bool setOriginalValues = true;

    private Vector3 targetPosition;
    private Vector3 originalPosition;

    private float speed = .1f;

    IEnumerator GetCoordinates()
    {
        //while true so this function keeps running once started.
        while (true)
        {
            if (!setOriginalValues)
            {
                // check if user has location service enabled
                if (!Input.location.isEnabledByUser)
                    yield break;

                // Start service before querying location
                Input.location.Start(1f, .1f);

                // Wait until service initializes
                int maxWait = 20;
                while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
                {
                    yield return new WaitForSeconds(1);
                    maxWait--;
                }

                // Service didn't initialize in 20 seconds
                if (maxWait < 1)
                {
                    print("Timed out");
                    yield break;
                }
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                print("Unable to determine device location");
                yield break;
            }
            else if(Input.location.status == LocationServiceStatus.Running)
            {
                if(numupdates < 50) numupdates++;
                numupdatesTextObject.GetComponent<Text>().text = "Number of Updates: " + numupdates;

                // Access granted and location value could be retrieved
                print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

                //if original value has not yet been set save coordinates of player on app start
                if (setOriginalValues)
                {
                    originalLatitude = Input.location.lastData.latitude;
                    originalLongitude = Input.location.lastData.longitude;
                    targetLatitude = originalLatitude;
                    targetLongitude = originalLongitude;
                    setOriginalValues = false;
                }

                //overwrite current lat and lon everytime
                currentLatitude = Input.location.lastData.latitude;
                currentLongitude = Input.location.lastData.longitude;
                currentAlititude = Input.location.lastData.altitude;

                //calculate the distance between where the player was when the app started and where they are now.
                Calc(originalLatitude, originalLongitude, currentLatitude, currentLongitude);

            }
            Input.location.Stop();
        }
    }

    //calculates distance between two sets of coordinates, taking into account the curvature of the earth.
    public void Calc(float lat1, float lon1, float lat2, float lon2)
    {

        var R = 6378.137; // Radius of earth in KM
        var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
        var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
          Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
          Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        distance = R * c;
        distance = distance * 1000f; // meters
                                     //set the distance text on the canvas
        distanceTextObject.GetComponent<Text>().text = "Distance: " + distance;
        oglonTextObject.GetComponent<Text>().text = "Original Lon: " + originalLongitude;
        oglatTextObject.GetComponent<Text>().text = "Original Lat: " + originalLatitude;
        curlonTextObject.GetComponent<Text>().text = "Current Lon: " + currentLongitude;
        curlatTextObject.GetComponent<Text>().text = "Current Lat: " + currentLatitude;
        //convert distance from double to float
        float distanceFloat = (float)distance;
        //set the target position of the ufo, this is where we lerp to in the update function
        targetPosition = originalPosition - new Vector3(0, 0, distanceFloat * 12);
        //distance was multiplied by 12 so I didn't have to walk that far to get the UFO to show up closer

    }

    void Start()
    {
        //get distance text reference
        distanceTextObject = GameObject.FindGameObjectWithTag("DistanceText");
        oglatTextObject = GameObject.FindGameObjectWithTag("OriginalLatText");
        oglonTextObject = GameObject.FindGameObjectWithTag("OriginalLonText");
        curlatTextObject = GameObject.FindGameObjectWithTag("CurrentLatText");
        curlonTextObject = GameObject.FindGameObjectWithTag("CurrentLonText");
        cube = GameObject.FindGameObjectWithTag("NumUpdatesText");
        //start GetCoordinate() function 
        StartCoroutine("GetCoordinates");
        //initialize target and original position
        targetPosition = transform.position;
        originalPosition = transform.position;

        /*initialize target and original position
        targetPosition = transform.position;
        originalPosition = transform.position;
        //start GetCoordinate() function 
        StartCoroutine("GetCoordinates");*/
    }

    void Update()
    {
        //linearly interpolate from current position to target position
        //transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
        //targetPosition = transform.position;
        cube.transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
        //rotate by 1 degree about the y axis every frame
        transform.eulerAngles += new Vector3(0, 1f, 0);

    }
}

/*public class AugmentedScript : MonoBehaviour
{
	private float originalLatitude;
	private float originalLongitude;
    private float currentAlititude;
	private float currentLongitude;
	private float currentLatitude;
	private float targetLongitude;
	private float targetLatitude;

	private GameObject distanceTextObject;
    private GameObject oglatTextObject;
    private GameObject oglonTextObject;
    private GameObject curlatTextObject;
    private GameObject curlonTextObject;

	private double distance;

	private bool setOriginalValues = true;

	private Vector3 targetPosition;
	private Vector3 originalPosition;

	private float speed = 0; //.1f;

	IEnumerator GetCoordinates()
	{
		//while true so this function keeps running once started.
		while (true) {
			// check if user has location service enabled //this should be a component of it's own so that all objects can access it 
			if (!Input.location.isEnabledByUser)
				yield break;

			// Start service before querying location
			Input.location.Start (1f,.1f);

			// Wait until service initializes
			int maxWait = 20;
			while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
				yield return new WaitForSeconds (1);
				maxWait--;
			}

			// Service didn't initialize in 20 seconds
			if (maxWait < 1) {
				print ("Timed out");
				yield break;
			}

			// Connection has failed
			if (Input.location.status == LocationServiceStatus.Failed) {
				print ("Unable to determine device location");
				yield break;
			} else {
				// Access granted and location value could be retrieved
				print ("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

				//if original value has not yet been set save coordinates of player on app start
				if (setOriginalValues) {
					originalLatitude = Input.location.lastData.latitude;
					originalLongitude = Input.location.lastData.longitude;
                    currentAlititude = Input.location.lastData.altitude;
					targetLatitude = originalLatitude;
					targetLongitude = originalLongitude;
					setOriginalValues = false;
				}

				//overwrite current lat and lon everytime
				currentLatitude = Input.location.lastData.latitude;
				currentLongitude = Input.location.lastData.longitude;

				//calculate the distance between where the player was when the app started and where they are now.
				//CalcDistance (originalLatitude, originalLongitude, currentLatitude, currentLongitude);

				//calculate the distance between where the object is and where the player is now
				CalcDistance(targetLatitude, targetLongitude, currentLatitude, currentLongitude);
			}
			Input.location.Stop();
		}
	}

	//calculates distance between two sets of coordinates, taking into account the curvature of the earth.
	public void CalcDistance(float lat1, float lon1, float lat2, float lon2)
	{

		var R = 6378.137; // Radius of earth in KM
		var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
		var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
		float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
			Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
			Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
		var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
		distance = R * c;
		distance = distance * 1000f; // meters
		//set the distance text on the canvas
		distanceTextObject.GetComponent<Text> ().text = "Distance: " + distance;
        oglonTextObject.GetComponent<Text>().text = "Original Lon: " + originalLongitude;
        oglatTextObject.GetComponent<Text>().text = "Original Lat: " + originalLatitude;
        curlonTextObject.GetComponent<Text>().text = "Current Lon: " + currentLongitude;
        curlatTextObject.GetComponent<Text>().text = "Current Lat: " + currentLatitude;
		//convert distance from double to float
		float distanceFloat = (float)distance;
		//set the target position of the ufo, this is where we lerp to in the update function
		targetPosition = originalPosition - new Vector3 (0, 0, distanceFloat);
		//distance was multiplied by 12 so I didn't have to walk that far to get the UFO to show up closer

	}

	void Start(){
		//get distance text reference
		distanceTextObject = GameObject.FindGameObjectWithTag ("DistanceText");
        oglatTextObject = GameObject.FindGameObjectWithTag("OriginalLatText");
        oglonTextObject = GameObject.FindGameObjectWithTag("OriginalLonText");
        curlatTextObject = GameObject.FindGameObjectWithTag("CurrentLatText");
        curlonTextObject = GameObject.FindGameObjectWithTag("CurrentLonText");
		//initialize target and original position
		targetPosition = transform.position;
		originalPosition = transform.position;
		//start GetCoordinate() function 
		StartCoroutine ("GetCoordinates");
	}

	void Update(){
		//linearly interpolate from current position to target position
		transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
		//doesn't work if transform.position = targetPosition

		//rotate by 1 degree about the y axis every frame
		transform.eulerAngles += new Vector3 (0, 1f, 0);

	}
}*/
