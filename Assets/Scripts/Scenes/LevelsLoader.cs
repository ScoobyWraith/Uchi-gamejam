using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsLoader : MonoBehaviour
{
    public static LevelsLoader Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void loadLevel(int level)
    {
        if (level == 1)
        {
            ScenesManager.Instance.LoadSceneAdditiveWithLoading("Quiz-1-1");
            return;
        }

        Debug.LogWarning("Не понял, какой уровень грузить");
    }


}
