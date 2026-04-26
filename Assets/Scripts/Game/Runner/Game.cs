using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    public Canvas canvas;
    public GameSettings gameSettings;
    
    private Transform background1;
    private Transform background2;
    private Vector3 originalBackgroundPosition;
    private List<GameObject> enemies = new List<GameObject>();
    private Camera mainCamera;
    private float timer;
    private bool isRun = false;
    private float backgroundWidth;

    void Start()
    {
        mainCamera = Camera.main;
        
        LoadGame();
        StartGame();
    }

    void FixedUpdate()
    {
        if (!isRun)
        {
            return;
        }

        timer += Time.fixedDeltaTime;
        MoveBackground();
        //MoveEnemies();
    }

    public void StartGame()
    {
        isRun = true;
        timer = 0;
    }

    private void LoadGame()
    {
        timer = 0;

        LoadBackgrounds();
        //LoadEnemies();
    }

    private void LoadBackgrounds()
    {
        Transform parent = gameSettings.backs.transform;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.name.Equals(gameSettings.backName))
            {
                background1 = child;
            } else
            {
                child.gameObject.SetActive(false);
            }
        }

        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWeight = screenHeight * aspectRatio;
        SpriteRenderer spriteRenderer = background1.GetComponent<SpriteRenderer>();
        float scaleY = screenHeight / spriteRenderer.sprite.bounds.size.y;
        background1.localScale = new Vector3(scaleY, scaleY, 1f);
        backgroundWidth = spriteRenderer.bounds.size.x;

        float deltaX = backgroundWidth / 2 - screenWeight / 2;
        background1.localPosition = new Vector3(deltaX, 0, 0);
        background2 = Instantiate(background1, background1.parent);
        background2.localPosition = background1.localPosition + new Vector3(backgroundWidth, 0);
        originalBackgroundPosition = background1.localPosition;
    }

    private void LoadEnemies()
    {
        Transform parent = gameSettings.enemies.transform;
        GameObject pool = null;

        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject child = parent.GetChild(i).gameObject;

            if (child.name.Equals(gameSettings.enemiesName))
            {
                pool = child;
            } else
            {
                child.SetActive(false);
            }
        }

        parent = pool.transform;

        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject child = parent.GetChild(i).gameObject;
            enemies.Add(child);
        }
    }
    
    private void MoveBackground()
    {
        float deltaX = -getCurrentDeltaX();

        background1.localPosition += new Vector3(deltaX, 0, 0);
        background2.localPosition += new Vector3(deltaX, 0, 0);

        if (background2.localPosition.x < originalBackgroundPosition.x)
        {
            background1.localPosition = background2.localPosition + new Vector3(backgroundWidth, 0, 0);
            Transform tmp = background1;
            background1 = background2;
            background2 = tmp;
        }
    }

    private void MoveEnemies()
    {
        float deltaX = -getCurrentDeltaX();

        foreach(GameObject e in enemies)
        {
            e.transform.localPosition += new Vector3(deltaX, 0, 0);
        }
    }

    private float getCurrentDeltaX()
    {
        float timePart = Mathf.Clamp(timer / gameSettings.durationSeconds, 0, 1);
        float k = gameSettings.speedByTime.Evaluate(timePart);
        return Time.fixedDeltaTime * gameSettings.speed * k;
    }
}
