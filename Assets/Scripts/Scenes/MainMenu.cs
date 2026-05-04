using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject canvas;
    public GameObject eventSystem;
    public LevelsLoader levelsLoader;

    public GameObject originButtons;
    public GameObject continueButtons;

    public void Awake()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        yield return new WaitUntil(() => GlobalGame.GetInstance() != null);

        GlobalGame globalGame = GlobalGame.GetInstance();

        if (globalGame.GetCurrentProgress() == 0)
        {
            originButtons.SetActive(true);
        }
        else
        {
            continueButtons.SetActive(true);
        }

        canvas.SetActive(true);
    }

    public void PlayGame()
    {       
        AudioManager.GetInstance().PlayClickSound();
        
        levelsLoader.LoadGameCoroutine();
        DestroyUI();
    }

    public void PlayOriginGame()
    {       
        AudioManager.GetInstance().PlayClickSound();
        
        GlobalGame globalGame = GlobalGame.GetInstance();
        globalGame.ResetGame();
        
        levelsLoader.LoadGameCoroutine();
        DestroyUI();
    }

    public void DestroyUI()
    {
        Destroy(canvas);
        Destroy(eventSystem);
    }
}
