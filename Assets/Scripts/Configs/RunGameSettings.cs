using UnityEngine;

[System.Serializable]
public class RunGameSettings
{
    public int lives = 3;
    public float undeathPeriodSeconds = 2;
    public float gameDurationSeconds = 30;
    public float noEnemyDurationSeconds = 5;
    public AnimationCurve enemyMinDistanceByTime;
    public AnimationCurve enemyMaxDistanceByTime;
    public float speed = 10;
    public float backgroundSpeedScale = 0.8f;
    public AnimationCurve speedByTime;
    public float playerSpeed = 10;
    public int rows = 3;
    public string backName = "river";
    public string playerName = "crok";
    public string enemiesName = "pool-1";
    public string endLevelObjectName = "flag";
}