using UnityEngine;

[System.Serializable]
public class JumpGameSettings
{
    public int lives = 3;
    public float undeathPeriodSeconds = 2;
    public float gameDurationSeconds = 30;
    public float noEnemyDurationSeconds = 5;
    public AnimationCurve minUnholeDistance;
    public AnimationCurve maxUnholeDistance;
    public AnimationCurve minHoleSize;
    public AnimationCurve maxHoleSize;
    public AnimationCurve minBreakDistance;
    public AnimationCurve maxBreakDistance;

    public float speed = 10;
    public AnimationCurve speedByTime;
    public float playerJump = 10f;
    public float platformDistanceFromBottom = 0.3f;
    public string backName = "river";
    public string playerName = "crok";
    public string enemiesName = "pool-1";
    public string platformName = "platform";
}