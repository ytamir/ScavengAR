﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text timer;
    private float timeRemaining;
    private int mode;
	// Use this for initialization
	void Start () {
        timeRemaining = GameObject.FindGameObjectWithTag("GameDriver").GetComponent<GameDriver>().getTime();
        mode = GameObject.Find("GameDriver").GetComponent<GameDriver>().getGameMode();
    }

    private void LateUpdate()
    {
        if(mode == 0)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                var minutes = Math.Floor(timeRemaining / 60); //Divide the guiTime by sixty to get the minutes.
                var seconds = Math.Floor(timeRemaining % 60);//Use the euclidean division for the seconds.
                var fraction = (timeRemaining * 100) % 100;
                //update the label value
                if (minutes >= 0 && seconds >= 0)
                {
                    timer.text = string.Format("Time: {0:00} : {1:00}", minutes, seconds);
                }
                if (seconds <= 10 && minutes == 0)
                {
                    if (seconds % 2 == 0)
                    {
                        timer.color = Color.white;
                    }
                    else
                    {
                        timer.color = Color.red;
                    }
                }
            }
            else
            {
                timer.text = string.Format("Time: {0:00} : {1:00}", 0, 0);
            }
        }else
        {
            timeRemaining += Time.deltaTime;
            var minutes = Math.Floor(timeRemaining / 60); //Divide the guiTime by sixty to get the minutes.
            var seconds = Math.Floor(timeRemaining % 60);//Use the euclidean division for the seconds.
            var fraction = (timeRemaining * 100) % 100;
            //update the label value
            timer.text = string.Format("Time: {0:00} : {1:00}", minutes, seconds);
        }
    }
}
