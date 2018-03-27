using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fader: MonoBehaviour

{
    public GameObject mainMenu;
    public GameObject gameSize;
    public GameObject gameMode;
    float fadeTime = 3f;
    Color colorToFadeTo;


    void Start()
    {
        
    }

    public void FadeOut(int nextPanel)
    {
        gameMode.SetActive(false);
        mainMenu.SetActive(false);
        gameSize.SetActive(false);

        switch(nextPanel)
        {
            case 0:
                mainMenu.SetActive(true);
                break;
            case 1:
                gameSize.SetActive(true);
                break;
            case 2:
                gameMode.SetActive(true);
                break;
        }
        
    }
}