using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public float durationSeconds = 30;

    public float speed;

    public AnimationCurve speedByTime;

    public int rows = 3;

    public string backName = "river";

    public string playerName = "crok";

    public string enemiesName = "pool-1";

    public GameObject backs;

    public GameObject players;

    public GameObject enemies;
}
