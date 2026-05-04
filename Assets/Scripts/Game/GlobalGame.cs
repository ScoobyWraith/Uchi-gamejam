using UnityEngine;

public class GlobalGame : MonoBehaviour
{
    private static GlobalGame instance;
    
    public int initProgress = 0;
    public bool skipQuiz = false;
    public bool useSavingGame = true;
    
    private int currentProgress = 0;

    public static GlobalGame GetInstance()
    {
        return instance;
    }
    
    public void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        if (useSavingGame)
        {
            currentProgress = PlayerPrefs.GetInt("playerProgress", 0);
        }
        else
        {
            currentProgress = initProgress;
        }

        instance = this;
    }

    public int GetCurrentProgress()
    {
        return currentProgress;
    }

    public void IncProgress()
    {
        currentProgress++;

        if (useSavingGame)
        {
            PlayerPrefs.SetInt("playerProgress", currentProgress);
            PlayerPrefs.Save();
        }
    }

    public void ResetGame()
    {
        currentProgress = 0;

        if (useSavingGame)
        {
            PlayerPrefs.SetInt("playerProgress", currentProgress);
            PlayerPrefs.Save();
        }
    }
}
