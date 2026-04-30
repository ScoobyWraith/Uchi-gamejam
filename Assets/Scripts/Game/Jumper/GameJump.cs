using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameJump : MonoBehaviour
{
    public ModalView failModal;
    public ModalView winModal;
    public GameObject heart;
    public Text timerBlock;
    public GameObject platformsObject;
    public GameObject backsObject;
    public GameObject playersObject;
    public GameObject enemiesObject;
    public JumpGameConfig jumpGameConfig;
    public float cheatTime = 0;

    private float mainY;
    private JumpGameSettings gameSettings;
    private List<GameObject> lives = new List<GameObject>();
    private Transform background1;
    private Transform background2;
    private Vector3 originalBackgroundPosition;
    private List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> originalEnemies = new List<GameObject>();
    private List<GameObject> platforms = new List<GameObject>();
    private GameObject originalPlatform;
    private Vector3 platformBounds;
    private Camera mainCamera;
    private PlayerJump player;
    private float timer;
    private bool isRun = false;
    private float backgroundWidth;
    private float playerWidth;
    private System.Random rnd;
    private float mapLength;
    private float noEnemyDistance;

    public void Start()
    {
        mainCamera = Camera.main;
        
        LoadSettings();
        LoadGame();
        ScenesLoader.SceneLoaded();
    }

    public void FixedUpdate()
    {
        if (!isRun)
        {
            return;
        }

        timer += Time.fixedDeltaTime;
        MoveBackground();
        MoveBreaks();
        ShowTimerInHUD();

        if (timer > gameSettings.gameDurationSeconds)
        {
            StopGame();
            winModal.OpenModal();
        }
    }

    public void StartGame()
    {
        isRun = true;
        timer = 0;
        player.StartPlayer();
    }

    public void StopGame()
    {
        isRun = false;
        player.StopPlayer();
    }

    public void RestartGame()
    {
        LoadGame();
        StartGame();
    }

    public void CompleteGame()
    {
        GlobalGame globalGame = GlobalGame.GetInstance();
        LevelsLoader levelsLoader = LevelsLoader.GetInstance();

        globalGame.IncProgress();
        levelsLoader.LoadLevelByProgress();
    }

    private void LoadSettings()
    {
        GlobalGame globalGame = GlobalGame.GetInstance();

        gameSettings = jumpGameConfig.GetSettingsByProgress(globalGame.GetCurrentProgress());

        if (cheatTime >= 1)
        {
            gameSettings.gameDurationSeconds = cheatTime;
        }

        NormolizeCurves();
    }
 
    private void LoadGame()
    {
        noEnemyDistance = gameSettings.noEnemyDurationSeconds * gameSettings.speed;
        mapLength = 0;
        timer = 0;
        rnd = new System.Random();

        CalcMainY();
        LoadBackgrounds();
        LoadPlayer();
        LoadEnemies();
        LoadPlatforms();
        LoadNewBreaks();
        LoadHUD();
    }

    private void CalcMainY()
    {
        Vector2 screenSize = GetScreenSize();
        mainY = -screenSize.y / 2 + screenSize.y * gameSettings.platformDistanceFromBottom;
    }

    private void LoadHUD()
    {
        timerBlock.text = ((int) gameSettings.gameDurationSeconds).ToString();

        if (lives != null && lives.Count > 0)
        {
        
            foreach (GameObject item in lives)
            {
                Destroy(item);
            }
        }

        lives = new List<GameObject>();
        heart.SetActive(false);

        for (int i = 0; i < gameSettings.lives; i++)
        {
            GameObject l = Instantiate(heart, heart.transform.parent);
            l.SetActive(true);
            lives.Add(l);
        }
    }

    private void LoadBackgrounds()
    {
        if (background2 != null)
        {
            return;
        }
        
        Transform parent = backsObject.transform;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.name.Equals(gameSettings.backName))
            {
                background1 = child;
                background1.gameObject.SetActive(true);
            } else
            {
                child.gameObject.SetActive(false);
            }
        }

        Vector2 screenSize = GetScreenSize();
        float screenWeight = screenSize.x;
        float screenHeight = screenSize.y;
        SpriteRenderer spriteRenderer = background1.GetComponent<SpriteRenderer>();
        float scaleY = screenHeight / spriteRenderer.sprite.bounds.size.y;
        background1.localScale = new Vector3(scaleY, scaleY, 1f);
        backgroundWidth = spriteRenderer.bounds.size.x;

        float deltaX = backgroundWidth / 2 - screenWeight / 2;
        background1.localPosition = new Vector3(deltaX, 0, 0);
        background2 = Instantiate(background1, background1.parent);
        background2.localPosition = background1.localPosition + new Vector3(backgroundWidth * 0.9999f, 0);
        originalBackgroundPosition = background1.localPosition;
    }

    private void LoadEnemies()
    {
        foreach (GameObject item in enemies)
        {
            Destroy(item);
        }

        enemies = new List<GameObject>();

        Vector2 screenSize = GetScreenSize();
        float screenWeight = screenSize.x;
        
        Transform parent = enemiesObject.transform;
        GameObject pool = null;

        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject child = parent.GetChild(i).gameObject;

            if (child.name.Equals(gameSettings.enemiesName))
            {
                pool = child;
                pool.SetActive(true);
            } else
            {
                child.SetActive(false);
            }
        }

        parent = pool.transform;

        if (originalEnemies.Count == 0)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                GameObject child = parent.GetChild(i).gameObject;
                child.SetActive(false);
                originalEnemies.Add(child);
            }
        }
    }

    private void LoadPlayer()
    {
        if (player != null)
        {
            player.LoadPlayer(gameSettings);
            return;
        }
        
        Transform parent = playersObject.transform;
        GameObject p = null;

        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject child = parent.GetChild(i).gameObject;

            if (child.name.Equals(gameSettings.playerName))
            {
                p = child;
            } else
            {
                child.SetActive(false);
            }
        }

        player = p.GetComponent<PlayerJump>();
        player.SeteOnHit(HitPlayer);
        playerWidth = player.GetWidth();
        player.LoadPlayer(gameSettings);
    }

    private void LoadPlatforms()
    {
        foreach (GameObject p in platforms)
        {
            Destroy(p);
        }
        
        platforms = new List<GameObject>();
        Transform parent = platformsObject.transform;

        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject child = parent.GetChild(i).gameObject;

            if (child.name.Equals(gameSettings.platformName))
            {
                originalPlatform = child;
                originalPlatform.SetActive(false);
            } else
            {
                child.SetActive(false);
            }
        }

        Vector2 screnSize = GetScreenSize();
        SpriteRenderer spriteRenderer = originalPlatform.GetComponent<SpriteRenderer>();
        float height = screnSize.y * gameSettings.platformDistanceFromBottom;
        float scale = height / spriteRenderer.sprite.bounds.size.y;
        originalPlatform.transform.localScale = new Vector3(scale, scale, 1f);
        platformBounds = spriteRenderer.bounds.size;
    }
    
    private void ShowTimerInHUD()
    {
        int t = (int) (gameSettings.gameDurationSeconds - timer);
        timerBlock.text = t.ToString();
    }

    private void MoveBackground()
    {
        float deltaX = -gameSettings.speed * Time.fixedDeltaTime * GetCurrentCurveValue(gameSettings.speedByTime);

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

    private void MoveBreaks()
    {
        float deltaX = gameSettings.speed * Time.fixedDeltaTime * GetCurrentCurveValue(gameSettings.speedByTime);
        noEnemyDistance -= deltaX;
        mapLength -= deltaX;
        
        foreach (GameObject g in enemies)
        {
            g.transform.localPosition += new Vector3(-deltaX, 0, 0);
        }

        foreach (GameObject g in platforms)
        {
            g.transform.localPosition += new Vector3(-deltaX, 0, 0);
        }
        
        DestroyUnavailable();
        LoadNewBreaks();
    }

    private void DestroyUnavailable()
    {
        Vector2 screenSize = GetScreenSize();
        float bound = -screenSize.x;
        
        enemies = enemies.Where(g =>
        {
            if (g.transform.localPosition.x >= bound)
            {
                return true;
            }

            Destroy(g);
            return false;
        }).ToList();

        platforms = platforms.Where(g =>
        {
            if (g.transform.localPosition.x >= bound)
            {
                return true;
            }

            Destroy(g);
            return false;
        }).ToList();
    }

    private void LoadNewBreaks()
    {
        Vector2 screenSize = GetScreenSize();
        
        while(mapLength < screenSize.x * 1.5)
        {
            float unholeDistance = playerWidth * GetRndBetween(
                GetCurrentCurveValue(gameSettings.minUnholeDistance), 
                GetCurrentCurveValue(gameSettings.maxUnholeDistance)
            );

            float curX = 0;

            float startXForPlatforms = -screenSize.x / 2 + mapLength;
            float endXForPlatforms = 0;

            while(curX < unholeDistance)
            {
                GameObject newPlatform = Instantiate(originalPlatform, originalPlatform.transform.parent);
                newPlatform.gameObject.SetActive(true);

                float cX = -screenSize.x / 2 + mapLength + curX + platformBounds.x / 2;
                curX += platformBounds.x;

                float cY = mainY - platformBounds.y / 2;

                newPlatform.transform.localPosition = new Vector3(cX, cY, 0);
                platforms.Add(newPlatform);

                endXForPlatforms = cX + platformBounds.x / 2;
            }

            if (endXForPlatforms > noEnemyDistance)
            {
                float gap = playerWidth * GetRndBetween(
                    GetCurrentCurveValue(gameSettings.minBreakDistance), 
                    GetCurrentCurveValue(gameSettings.maxBreakDistance)
                );

                float startForBreak = Mathf.Max(startXForPlatforms + gap, noEnemyDistance);
                float endForBreak = endXForPlatforms - gap;
                float curXForBreaks = startForBreak;

                if (startForBreak < endForBreak)
                {
                    while(curXForBreaks < endForBreak)
                    {
                        GameObject originalBreak = originalEnemies[rnd.Next(originalEnemies.Count)];
                        GameObject newBreak = Instantiate(originalBreak, originalBreak.transform.parent);
                        newBreak.gameObject.SetActive(true);

                        SpriteRenderer spriteRenderer = newBreak.GetComponent<SpriteRenderer>();
                        Vector3 sizes = spriteRenderer.bounds.size;


                        float cX = curXForBreaks + sizes.x / 2;
                        float cY = mainY + sizes.y / 2;
                        newBreak.transform.localPosition = new Vector3(cX, cY, 0);
                        enemies.Add(newBreak);

                        float distance = playerWidth * GetRndBetween(
                            GetCurrentCurveValue(gameSettings.minBreakDistance), 
                            GetCurrentCurveValue(gameSettings.maxBreakDistance)
                        );

                        curXForBreaks += distance;
                    }
                }
            }

            float holeSize = playerWidth * GetRndBetween(
                GetCurrentCurveValue(gameSettings.minHoleSize), 
                GetCurrentCurveValue(gameSettings.maxHoleSize)
            );

            mapLength += curX + holeSize;
        }
    }
    
    private void HitPlayer()
    {
        GameObject live = lives[lives.Count - 1];
        lives.RemoveAt(lives.Count - 1);
        Destroy(live);

        if (lives.Count == 0)
        {
            StopGame();
            failModal.OpenModal();
        }
    }

    private Vector2 GetScreenSize()
    {
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWeight = screenHeight * aspectRatio;

        return new Vector2(screenWeight, screenHeight);
    }

    private float GetCurrentCurveValue(AnimationCurve curve)
    {
        float timePart = Mathf.Clamp(timer / gameSettings.gameDurationSeconds, 0, 1);
        return curve.Evaluate(timePart);
    }

    private void ShuffleList<T>(IList<T> list)
    {
        int n = list.Count;
        
        for (int i = n - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private void NormolizeCurves()
    {
        gameSettings.minUnholeDistance = NormalizeCurve(gameSettings.minUnholeDistance);
        gameSettings.maxUnholeDistance = NormalizeCurve(gameSettings.maxUnholeDistance);
        gameSettings.minHoleSize = NormalizeCurve(gameSettings.minHoleSize);
        gameSettings.maxHoleSize = NormalizeCurve(gameSettings.maxHoleSize);
        gameSettings.minBreakDistance = NormalizeCurve(gameSettings.minBreakDistance);
        gameSettings.maxBreakDistance = NormalizeCurve(gameSettings.maxBreakDistance);
        gameSettings.speedByTime = NormalizeCurve(gameSettings.speedByTime);
    }

    private static AnimationCurve NormalizeCurve(AnimationCurve curve)
    {
        if (curve == null || curve.length == 0)
            return new AnimationCurve();

        float minX = float.MaxValue;
        float maxX = float.MinValue;

        foreach (Keyframe key in curve.keys)
        {
            minX = Mathf.Min(minX, key.time);
            maxX = Mathf.Max(maxX, key.time);
        }

        if (Mathf.Approximately(minX, maxX))
        {
            Keyframe[] newKeys = { new Keyframe(0.5f, curve.Evaluate(minX)) };
            return new AnimationCurve(newKeys);
        }

        float range = maxX - minX;

        Keyframe[] normalizedKeys = new Keyframe[curve.length];

        for (int i = 0; i < curve.length; i++)
        {
            Keyframe oldKey = curve.keys[i];
            float normalizedX = (oldKey.time - minX) / range;

            normalizedKeys[i] = new Keyframe(
                normalizedX,
                oldKey.value,
                oldKey.inTangent / range,
                oldKey.outTangent / range,
                oldKey.inWeight,
                oldKey.outWeight
            );
        }

        return new AnimationCurve(normalizedKeys);
    }

    private float GetRndBetween(float min, float max)
    {   
        return (float)(min + rnd.NextDouble() * (Mathf.Max(max, min) - Mathf.Min(max, min)));
    }
}
