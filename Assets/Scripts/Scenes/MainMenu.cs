using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject canvas;
    public GameObject eventSystem;
    public LevelsLoader levelsLoader;

    public void PlayGame()
    {
        levelsLoader.LoadGameCoroutine();
        DestroyUI();
    }

    public void DestroyUI()
    {
        Destroy(canvas);
        Destroy(eventSystem);
    }
}
