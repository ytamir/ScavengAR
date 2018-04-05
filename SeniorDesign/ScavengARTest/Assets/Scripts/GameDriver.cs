using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDriver : MonoBehaviour {

    private float gameSizeX;
    private float gameSizeY;
    private int gameMode;
    private int score = 0;
    private float gameTime = 300.0f;
    public GameObject decBtn;
    public GameObject incBtn;
    public Text curTime;

    public static GameDriver gd;

    // Use this for initialization

    public void ScorePoint()
    {
        score++;
    }

    public int GetScore()
    {
        return score;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if(gd == null)
        {
            gd = this;
        } else if (gd != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void increaseGameTime()
    {
        gameTime += 60.0f;
        decBtn.GetComponent<Button>().interactable = true;
        if (gameTime == 600.0f)
        {
            incBtn.GetComponent<Button>().interactable = false;
        }
        curTime.text = (gameTime/60).ToString() + " minutes";

    }

    public void decreaseGameTime()
    {
        incBtn.GetComponent<Button>().interactable = true;
        if (gameTime > 60.0f)
        {
            gameTime -= 60.0f;
        }
        if (gameTime == 60.0f)
        {
            decBtn.GetComponent<Button>().interactable = false;
        }
        curTime.text = (gameTime / 60).ToString() + " minutes";
    }

    void Start () {
        curTime.text = (gameTime / 60).ToString() + " minutes";
    }

    public void setGameSize(int size)
    {
        switch(size)
        {
            case 0:
                gameSizeX = 0.0024f;
                gameSizeY = 0.0018f;
                break;
            case 1:
                gameSizeX = 0.0034f;
                gameSizeY = 0.0028f;
                break;
            case 2:
                gameSizeX = 0.0044f;
                gameSizeY = 0.0038f;
                break;
        }
    }

    public float getTime()
    {
        return gameTime;
    }

    public void setGameMode(int mode)
    {
        gameMode = mode;
        if(mode == 1)
        {
            gameTime = 0;
        }
    }

    public int getGameMode()
    {
        return gameMode;
    }

    public float getX()
    {
        return gameSizeX;
    }

    public float getY()
    {
        return gameSizeY;
    }

    public void loadGame()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
