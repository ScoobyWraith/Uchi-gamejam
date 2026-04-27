using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static LevelsByProgress;

public class LevelsLoader : MonoBehaviour
{
    private static LevelsLoader instance;

    public LevelsByProgress levelsByProgress;

    public static LevelsLoader GetInstance()
    {
        return instance;
    }

    public void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        yield return new WaitUntil(() => ScenesLoader.GetInstance() != null && GlobalGame.GetInstance() != null);

        ScenesLoader scenesLoader = ScenesLoader.GetInstance();
        
        if (ForceBackgroundSceneLoader.LOADED_FROM_CRITICAL_SCENE_NAME)
        {
            LoadLevelByProgress();
            
            yield break;
        } 
        else
        {
            yield return new WaitForSeconds(scenesLoader.minLoadingDelaySeconds);
            scenesLoader.HideLoadingScreen();

            yield break;
        }
    }

    public void LoadIsland(int islandNumber)
    {
        GlobalGame globalGame = GlobalGame.GetInstance();
        
        globalGame.IncProgress();
        LoadLevelByProgress();
    }

    public void LoadLevelByProgress()
    {
        GlobalGame globalGame = GlobalGame.GetInstance();
        ScenesLoader scenesLoader = ScenesLoader.GetInstance();
        LevelByProgress levelByProgress = levelsByProgress.GetItem(globalGame.GetCurrentProgress());
        scenesLoader.LoadSceneAdditiveWithLoading(levelByProgress.sceneName);
    }
}
