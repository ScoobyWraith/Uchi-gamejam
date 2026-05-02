using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public ModalView failModal;
    public ModalView winModal;
    public GameObject heart;
    public Text timerBlock;
    public GameObject backsObject;
    public GameObject playersObject;
    public GameObject enemiesObject;
    public RunGameConfig runGameConfig;
    public float cheatTime = 0;
    
    private RunGameSettings gameSettings;
    private List<GameObject> lives = new List<GameObject>();
    private Transform background1;
    private Transform background2;
    private Vector3 originalBackgroundPosition;
    private List<Transform> enemies = new List<Transform>();
    private List<Transform> originalEnemies = new List<Transform>();
    private Transform lastEnemy;
    private List<float> rowPositions = new List<float>();
    private Camera mainCamera;
    private Player player;
    private float timer;
    private bool isRun = false;
    private float backgroundWidth;
    private float playerWidth;
    private System.Random rnd;

    public void Start()
    {
        mainCamera = Camera.main;
        
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        yield return null;

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
        MoveEnemies();
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

        gameSettings = runGameConfig.GetSettingsByProgress(globalGame.GetCurrentProgress());

        if (cheatTime >= 1)
        {
            gameSettings.gameDurationSeconds = cheatTime;
        }

        NormolizeCurves();
    }
 
    private void LoadGame()
    {
        timer = 0;
        rnd = new System.Random();

        LoadBackgrounds();
        LoadPlayer();
        LoadEnemies();
        LoadHUD();
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
        if (enemies != null && enemies.Count > 0)
        {
            foreach (Transform item in enemies)
            {
                Destroy(item.gameObject);
            }
        }

        enemies = new List<Transform>();

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
                Transform child = parent.GetChild(i);
                child.localPosition = new Vector3(-2 * screenWeight, 0, 0);
                child.gameObject.SetActive(false);
                originalEnemies.Add(child);
            }
        }
        
        float scaleY = screenWeight / playerWidth;
        int enemiesQuantity = (int)(scaleY * gameSettings.rows);
        int enemyIndex = 0;

        while (enemiesQuantity > 0)
        {
            enemiesQuantity--;
            Transform originalenemy = originalEnemies[enemyIndex];
            Transform copyEnemy = Instantiate(originalenemy, parent);
            copyEnemy.gameObject.SetActive(true);
            enemies.Add(copyEnemy);
            enemyIndex = (enemyIndex + 1) % originalEnemies.Count;
        }

        Transform enemy = enemies[rnd.Next(enemies.Count)];
        float y = rowPositions[rnd.Next(rowPositions.Count)];
        SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
        playerWidth = spriteRenderer.sprite.bounds.size.x;

        enemy.localPosition = new Vector3(screenWeight / 2 + playerWidth, y, 0);
        lastEnemy = enemy;
    }

    private void LoadPlayer()
    {
        if (player != null)
        {
            player.LoadPlayer(rowPositions, gameSettings);
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

        player = p.GetComponent<Player>();
        float screenHeight = 2f * mainCamera.orthographicSize;
        float padding = 0.1f * screenHeight;

        float partHeight = (screenHeight - 2 * padding) / gameSettings.rows;
        float startY = mainCamera.orthographicSize - padding;

        for (int i = 0; i < gameSettings.rows; i++)
        {
            rowPositions.Add(startY - (partHeight / 2 + i * partHeight));
        }

        player.LoadPlayer(rowPositions, gameSettings);
        
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        playerWidth = spriteRenderer.sprite.bounds.size.x;
        player.SeteOnHit(HitPlayer);
        player.gameObject.SetActive(true);
    }

    private void ShowTimerInHUD()
    {
        int t = (int) (gameSettings.gameDurationSeconds - timer);
        timerBlock.text = t.ToString();
    }

    private void MoveBackground()
    {
        float deltaX = -getCurrentDeltaX() * gameSettings.backgroundSpeedScale;

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
        if (timer < gameSettings.noEnemyDurationSeconds)
        {
            return;
        }
        
        float deltaX = -getCurrentDeltaX();

        foreach(Transform e in enemies)
        {
            e.localPosition += new Vector3(deltaX, 0, 0);
        }

        RegenerateEnemies();
    }

    private void RegenerateEnemies() {
        Vector2 screenSize = GetScreenSize();

        if (lastEnemy.localPosition.x < screenSize.x / 2)
        {
            List<Transform> pool = enemies.Where(t => t.localPosition.x < - screenSize.x).ToList();
            int quantity = rnd.Next(rowPositions.Count - 1);
            List<float> positions = rowPositions.ToList();
            ShuffleList(pool);
            ShuffleList(positions);

            float distace = getCurrentDistance();

            for (int i = 0; i < quantity; i++)
            {
                pool[i].localPosition = new Vector3(lastEnemy.localPosition.x + distace, positions[i], 0);
                lastEnemy = pool[i];
            }
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

    private float getCurrentDeltaX()
    {
        float timePart = Mathf.Clamp(timer / gameSettings.gameDurationSeconds, 0, 1);
        float k = gameSettings.speedByTime.Evaluate(timePart);
        return Time.fixedDeltaTime * gameSettings.speed * k;
    }

    private float getCurrentDistance()
    {
        float timePart = Mathf.Clamp(timer / gameSettings.gameDurationSeconds, 0, 1);
        float min = gameSettings.enemyMinDistanceByTime.Evaluate(timePart);
        float max = gameSettings.enemyMaxDistanceByTime.Evaluate(timePart);

        return (float)(playerWidth * (min + rnd.NextDouble() * (max - min)));
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
        gameSettings.enemyMaxDistanceByTime = NormalizeCurve(gameSettings.enemyMaxDistanceByTime);
        gameSettings.enemyMinDistanceByTime = NormalizeCurve(gameSettings.enemyMinDistanceByTime);
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
}
