using UnityEngine;

public class GlobalGame : MonoBehaviour
{
    private static GlobalGame instance;
    
    public int initProgress = 0;
    public bool skipQuiz = false;
    
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

        currentProgress = initProgress;
        instance = this;
    }

    public int GetCurrentProgress()
    {
        return currentProgress;
    }

    public void IncProgress()
    {
        currentProgress++;
    }
}
