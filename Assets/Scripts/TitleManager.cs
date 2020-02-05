using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public KeyCode upKeyCode = KeyCode.UpArrow;
    public KeyCode downKeyCode = KeyCode.DownArrow;

    public enum TitleOptions
    {
        SuperEasy,
        Easy,
        Medium,
        Hard,
        Deadly,
        Quit,
        NUMBER_OF_MENU_OPTIONS,
    }
    public int menuSelect;
    public GameObject titleSelector;


    // Start is called before the first frame update
    void Start()
    {
        SetMenuSelector((int)TitleOptions.Medium);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(upKeyCode))
        {
            SetMenuSelector(menuSelect - 1 + ((menuSelect == 0) ? (int)TitleOptions.NUMBER_OF_MENU_OPTIONS : 0));
        }
        if (Input.GetKeyDown(downKeyCode))
        {
            SetMenuSelector(menuSelect + 1 - ((menuSelect + 1 == (int)TitleOptions.NUMBER_OF_MENU_OPTIONS) ? (int)TitleOptions.NUMBER_OF_MENU_OPTIONS : 0));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (menuSelect == (int)TitleOptions.Quit)
            {
                Application.Quit();
            }
            else
            {
                PlayerPrefs.SetInt("difficulty", (int)menuSelect);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void SetMenuSelector(int selectorIndex)
    {
        menuSelect = selectorIndex;
        Vector3 newPos = titleSelector.transform.localPosition;
        newPos.y = -(menuSelect + (menuSelect == (int)TitleOptions.Quit ? 1 : 0)) * 20;
        titleSelector.transform.localPosition = newPos;
    }
}
