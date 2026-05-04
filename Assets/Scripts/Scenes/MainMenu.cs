using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject canvas;
    public GameObject eventSystem;
    public LevelsLoader levelsLoader;

    public void PlayGame()
    {       
        AudioManager.GetInstance().PlayClickSound();
        
        levelsLoader.LoadGameCoroutine();
        DestroyUI();
    }

    public void DestroyUI()
    {
        Destroy(canvas);
        Destroy(eventSystem);
    }
}
