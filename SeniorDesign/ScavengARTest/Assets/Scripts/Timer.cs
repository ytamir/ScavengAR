using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text timer;
    private float timeRemaining;
	// Use this for initialization
	void Start () {
        timeRemaining = GameObject.FindGameObjectWithTag("GameDriver").GetComponent<GameDriver>().getTime();

    }
	
	// Update is called once per frame
	void Update () {
        timeRemaining -= Time.deltaTime;
        var minutes = timeRemaining / 60; //Divide the guiTime by sixty to get the minutes.
        var seconds = timeRemaining % 60;//Use the euclidean division for the seconds.
        var fraction = (timeRemaining * 100) % 100;

        //update the label value
        timer.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
    }
}
