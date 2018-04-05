using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Location;
using Mapbox.Unity.Utilities;
using Mapbox.Unity.Map;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    float _positionFollowFactor;
    Vector3 _targetPosition;

	// Use this for initialization
	void Start () {
		
	}

    public void setTargetPosition(Vector3 _newPos)
    {
        _targetPosition = _newPos;
    }
	
	// Update is called once per frame
	void Update () {

        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _positionFollowFactor);

    }
}
