using System;
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

    private void LateUpdate()
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            var minutes = Math.Floor(timeRemaining / 60); //Divide the guiTime by sixty to get the minutes.
            var seconds = Math.Floor(timeRemaining % 60);//Use the euclidean division for the seconds.
            var fraction = (timeRemaining * 100) % 100;
            //update the label value
            timer.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
        }else
        {
            timer.text = string.Format("{0:00} : {1:00} : {2:000}", 0, 0, 0);
        }
    }
}
