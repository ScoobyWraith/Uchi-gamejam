using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;
    
    public GameObject LoadingScreen;
    
    public float MinLoadingDelaySeconds = 1f;

    private Scene roadMapScene;

    public void Awake()
    {
        roadMapScene = SceneManager.GetActiveScene();
        
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneAdditiveWithLoading(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        LoadingScreen.SetActive(true);

        if (!currentScene.Equals(roadMapScene))
        {
            SceneManager.SetActiveScene(roadMapScene);
            SceneManager.UnloadSceneAsync(currentScene);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;
        float startTime = Time.time;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        float elapsedTime = Time.time - startTime;

        if (elapsedTime < MinLoadingDelaySeconds)
        {
            float remainingTime = MinLoadingDelaySeconds - elapsedTime;
            yield return new WaitForSeconds(remainingTime);
        }

        asyncLoad.allowSceneActivation = true;
        yield return asyncLoad;
        LoadingScreen.SetActive(false);
    }
}
