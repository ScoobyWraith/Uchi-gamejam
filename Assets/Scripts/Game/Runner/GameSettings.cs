using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public int lives = 3;
    public float undeathPeriodSeconds = 2;
    public float gameDurationSeconds = 30;
    public float noEnemyDurationSeconds = 5;
    public AnimationCurve enemyMinDistanceByTime;
    public AnimationCurve enemyMaxDistanceByTime;
    public float speed;
    public AnimationCurve speedByTime;
    public float playerSpeed;
    public int rows = 3;
    public string backName = "river";
    public string playerName = "crok";
    public string enemiesName = "pool-1";
    public GameObject backs;
    public GameObject players;
    public GameObject enemies;

    void Start()
    {
        enemyMaxDistanceByTime = NormalizeX(enemyMaxDistanceByTime);
        enemyMinDistanceByTime = NormalizeX(enemyMinDistanceByTime);
        speedByTime = NormalizeX(speedByTime);
    }

    public static AnimationCurve NormalizeX(AnimationCurve curve)
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
