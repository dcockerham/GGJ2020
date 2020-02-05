using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public AudioSource mainMusic;
    public AudioSource loseMusic;

    public enum PlayState
    {
        Title,
        Playing,
        Pause,
        GameOver,
        Victory,
        Other,
    }
    public PlayState playState;

    public enum PauseOptions
    {
        Continue,
        Retry,
        Quit,
        NUMBER_OF_MENU_OPTIONS,
    }
    public enum GameOverOptions
    {
        Retry,
        Quit,
        NUMBER_OF_MENU_OPTIONS,
    }
    public int menuSelect;
    public GameObject pausePanel;
    public GameObject pauseSelector;
    public GameObject gameOverPanel;
    public GameObject gameOverSelector;
    public GameObject introPanel;


    private void Awake()
    {
        instance = this;
        if (introPanel)
        {
            playState = PlayState.Title;
            introPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            playState = PlayState.Playing;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        if (playState == PlayState.Title)
        {
            if (Input.GetKeyDown(fireKeyCode) || Input.GetKeyDown(pauseKeyCode))
            {
                playState = PlayState.Playing;
                introPanel.SetActive(false);
                Time.timeScale = 1f;
            }
        }
        else if (playState == PlayState.Pause)
        {
            if (Input.GetKeyDown(upKeyCode))
            {
                SetPauseSelector(menuSelect - 1 + ((menuSelect == 0) ? (int)PauseOptions.NUMBER_OF_MENU_OPTIONS : 0));
            }
            if (Input.GetKeyDown(downKeyCode))
            {
                SetPauseSelector(menuSelect + 1 - ((menuSelect+1 == (int)PauseOptions.NUMBER_OF_MENU_OPTIONS) ? (int)PauseOptions.NUMBER_OF_MENU_OPTIONS : 0));
            }
            if (Input.GetKeyDown(fireKeyCode) || Input.GetKeyDown(pauseKeyCode))
            {
                switch (menuSelect)
                {
                    case (int)PauseOptions.Continue:
                        PauseGame();
                        break;
                    case (int)PauseOptions.Retry:
                        // restart the scene?
                        PauseGame();
                        playState = PlayState.Other;
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        break;
                    case (int)PauseOptions.Quit:
                        // return to the title screen?
                        playState = PlayState.Other;
                        SceneManager.LoadScene(0);
                        break;
                    default:
                        break;
                }
            }
        }
        else if (playState == PlayState.Playing)
        {
            if (Input.GetKeyDown(pauseKeyCode))
            {
                PauseGame();
            }
        }
        else if (playState == PlayState.GameOver)
        {
            if (Input.GetKeyDown(upKeyCode))
            {
                SetGameOverSelector(menuSelect - 1 + ((menuSelect == 0) ? (int)GameOverOptions.NUMBER_OF_MENU_OPTIONS : 0));
            }
            if (Input.GetKeyDown(downKeyCode))
            {
                SetGameOverSelector(menuSelect + 1 - ((menuSelect + 1 == (int)GameOverOptions.NUMBER_OF_MENU_OPTIONS) ? (int)GameOverOptions.NUMBER_OF_MENU_OPTIONS : 0));
            }
            if (Input.GetKeyDown(fireKeyCode) || Input.GetKeyDown(pauseKeyCode))
            {
                switch (menuSelect)
                {
                    case (int)GameOverOptions.Retry:
                        // restart the scene?
                        playState = PlayState.Other;
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        break;
                    case (int)GameOverOptions.Quit:
                        // return to the title screen?
                        playState = PlayState.Other;
                        SceneManager.LoadScene(0);
                        break;
                    default:
                        break;
                }
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
            playState = PlayState.Victory;
            Invoke("Victory", 2.0f);
        }
    }

    public void Victory()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GameOver()
    {
        if (playState == PlayState.Playing)
        {
            print("_//GAME OVER\\\\_");
            if (loseMusic)
            {
                loseMusic.Play();
                if (mainMusic)
                {
                    mainMusic.Stop();
                }
            }
            Invoke("GameOverPart2", 1.0f);
        }
    }

    public void GameOverPart2()
    {
        playState = PlayState.GameOver;
        gameOverPanel.SetActive(true);
        SetGameOverSelector(0);
    }

    public void PauseGame()
    {
        if (playState == PlayState.Playing)
        {
            playState = PlayState.Pause;
            pausePanel.SetActive(true);
            SetPauseSelector(0);
            Time.timeScale = 0f;
        }
        else if (playState == PlayState.Pause)
        {
            playState = PlayState.Playing;
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void SetPauseSelector(int selectorIndex)
    {
        menuSelect = selectorIndex;
        Vector3 newPos = pauseSelector.transform.localPosition;
        newPos.y = 30 - menuSelect * 30;
        pauseSelector.transform.localPosition = newPos;
    }

    public void SetGameOverSelector(int selectorIndex)
    {
        menuSelect = selectorIndex;
        Vector3 newPos = gameOverSelector.transform.localPosition;
        newPos.y = -30 - menuSelect * 30;
        gameOverSelector.transform.localPosition = newPos;
    }
}
