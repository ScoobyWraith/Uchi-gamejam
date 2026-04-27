using UnityEngine;
using UnityEngine.SceneManagement;

public static class ForceBackgroundSceneLoader
{
    public const string CRITICAL_SCENE_NAME = "BackgroundScene";

    public static bool LOADED_FROM_CRITICAL_SCENE_NAME = true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {

        Scene activeScene = SceneManager.GetActiveScene();

        if (!activeScene.name.Equals(CRITICAL_SCENE_NAME))
        {
            LOADED_FROM_CRITICAL_SCENE_NAME = false;
            SceneManager.LoadScene(CRITICAL_SCENE_NAME, LoadSceneMode.Single);
            SceneManager.LoadScene(activeScene.name, LoadSceneMode.Additive);
        } 
    }
}
