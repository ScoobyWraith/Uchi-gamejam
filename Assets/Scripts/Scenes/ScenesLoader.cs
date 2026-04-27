using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour
{
    private static ScenesLoader instance;
    
    public GameObject loadingScreen;
    public float minLoadingDelaySeconds = 1f;

    private Scene criticalScene;

    public static ScenesLoader GetInstance()
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

        instance = this;
        criticalScene = SceneManager.GetSceneByName(ForceBackgroundSceneLoader.CRITICAL_SCENE_NAME);
    }

    public void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }

    public void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }

    public void LoadSceneAdditiveWithLoading(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string goalSceneName)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        ShowLoadingScreen();
        SceneManager.SetActiveScene(criticalScene);

        Debug.Log($"{currentScene.name} {goalSceneName}");

        if (!currentScene.name.Equals(criticalScene.name))
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }

        if (goalSceneName.Equals(criticalScene.name))
        {
            yield return new WaitForSeconds(minLoadingDelaySeconds);
            HideLoadingScreen();
            yield break;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(goalSceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;
        float startTime = Time.time;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        float elapsedTime = Time.time - startTime;

        if (elapsedTime < minLoadingDelaySeconds)
        {
            float remainingTime = minLoadingDelaySeconds - elapsedTime;
            yield return new WaitForSeconds(remainingTime);
        }

        asyncLoad.allowSceneActivation = true;
        yield return asyncLoad;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(goalSceneName));
        HideLoadingScreen();
    }
}
