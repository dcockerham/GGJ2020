using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // key code parameters
    public KeyCode leftKeyCode = KeyCode.LeftArrow;
    public KeyCode rightKeyCode = KeyCode.RightArrow;
    public KeyCode upKeyCode = KeyCode.UpArrow;
    public KeyCode downKeyCode = KeyCode.DownArrow;
    public KeyCode fireKeyCode = KeyCode.Space;
    public KeyCode tractorKeyCode = KeyCode.RightShift;
    public KeyCode pauseKeyCode = KeyCode.Return;

    public int numLivingEnemies;

    public enum PlayState
    {
        Title,
        Playing,
        Pause,
        Other,
    }
    public PlayState playState;

    public enum MenuOptions
    {
        Continue,
        Retry,
        Quit,
        NUMBER_OF_MENU_OPTIONS,
    }
    public int menuSelect;
    public GameObject pausePanel;
    public GameObject pauseSelector;


    private void Awake()
    {
        instance = this;
        playState = PlayState.Playing;
    }

    private void Update()
    {
        if (playState == PlayState.Pause)
        {
            if (Input.GetKeyDown(upKeyCode))
            {
                SetSelectorPos(menuSelect - 1 + ((menuSelect == 0) ? (int)MenuOptions.NUMBER_OF_MENU_OPTIONS : 0));
                /*menuSelect--;
                if (menuSelect < 0)
                {
                    menuSelect += (int)MenuOptions.NUMBER_OF_MENU_OPTIONS;
                }*/
            }
            if (Input.GetKeyDown(downKeyCode))
            {
                SetSelectorPos(menuSelect + 1 - ((menuSelect+1 == (int)MenuOptions.NUMBER_OF_MENU_OPTIONS) ? (int)MenuOptions.NUMBER_OF_MENU_OPTIONS : 0));
                /*menuSelect++;
                if (menuSelect >= (int)MenuOptions.NUMBER_OF_MENU_OPTIONS)
                {
                    menuSelect -= (int)MenuOptions.NUMBER_OF_MENU_OPTIONS;
                }*/
            }
            if (Input.GetKeyDown(fireKeyCode))
            {
                switch (menuSelect)
                {
                    case (int)MenuOptions.Continue:
                        PauseGame();
                        break;
                    case (int)MenuOptions.Retry:
                        // restart the scene?
                        break;
                    case (int)MenuOptions.Quit:
                        // return to the title screen?
                        break;
                    default:
                        break;
                }
            }
            if (Input.GetKeyDown(pauseKeyCode))
            {
                PauseGame();
            }
        }
        else if (playState == PlayState.Playing)
        {
            if (Input.GetKeyDown(pauseKeyCode))
            {
                PauseGame();
            }
        }
    }

    public void EnemySpawned()
    {
        numLivingEnemies++;
    }

    public void EnemyDestroyed()
    {
        numLivingEnemies--;
        if (playState == PlayState.Playing && numLivingEnemies <= 0)
        {
            print("~=STAGE CLEAR=~");
            playState = PlayState.Other;
        }
    }

    public void GameOver()
    {
        if (playState == PlayState.Playing)
        {
            print("_//GAME OVER\\\\_");
        }
    }

    public void PauseGame()
    {
        if (playState == PlayState.Playing)
        {
            playState = PlayState.Pause;
            pausePanel.SetActive(true);
            SetSelectorPos(0);
            Time.timeScale = 0f;
        }
        else if (playState == PlayState.Pause)
        {
            playState = PlayState.Playing;
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void SetSelectorPos(int selectorIndex)
    {
        menuSelect = selectorIndex;
        Vector3 newPos = pauseSelector.transform.localPosition;
        newPos.y = 30 - menuSelect * 30;
        pauseSelector.transform.localPosition = newPos;
    }
}
