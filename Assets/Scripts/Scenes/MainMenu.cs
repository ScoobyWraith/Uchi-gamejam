using UnityEngine;

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
