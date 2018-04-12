using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDriver : NetworkBehaviour {

    [SyncVar]
    public float gameTime = 300.0f;
    [SyncVar]
    public float gameSizeX = 0.0024f;
    [SyncVar]
    public float gameSizeY = 0.0018f;
    [SyncVar]
    public int gameMode;
    [SyncVar]
    public int objectCount = 10;
    [SyncVar]
    public Vector2d hostLocation = new Vector2d(0,0);
    [SyncVar]
    public int playersConnected = 0;

    public SyncListString playerNames = new SyncListString();

    public SyncListFloat objLocations = new SyncListFloat();
    public SyncListInt playerScores = new SyncListInt();

    private int score = 0;
    private string playerName;
    
    public GameObject decBtn;
    public GameObject incBtn;
    public Text curTime;

    public static GameDriver gd;

    // Use this for initialization

    [Command]
    public void CmdSetPlayerName(string pname)
    {
        playerNames.Add(pname);
        playerScores.Add(0);
        Debug.Log(playerNames);
    }

    public void setPlayerName(string pname)
    {
        playerName = pname;
    }
    
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
        objectCount += 1;
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
        objectCount -= 1;
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
                objectCount = 10;
                break;
            case 1:
                gameSizeX = 0.0034f;
                gameSizeY = 0.0028f;
                objectCount = 15;
                break;
            case 2:
                gameSizeX = 0.0044f;
                gameSizeY = 0.0038f;
                objectCount = 20;
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
        for (int i = 0; i < objectCount; i++)
        {
            Vector2 testLoc = Random.insideUnitCircle;
            testLoc.x *= gameSizeX;
            testLoc.y *= gameSizeY;
            objLocations.Add(testLoc.x);
            objLocations.Add(testLoc.y);
        }
        this.GetComponent<ChangeScene>().NewScene("AlecTest");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        if(isServer)
        {
            playersConnected = NetworkServer.connections.Count;
            Debug.Log(playersConnected);
        }
        if(SceneManager.GetActiveScene().name == "AlecTest")
        {
            if(gameMode == 0)
            {
                gameTime -= Time.deltaTime;
            } else
            {
                gameTime += Time.deltaTime;
            }
        }
    }
}
