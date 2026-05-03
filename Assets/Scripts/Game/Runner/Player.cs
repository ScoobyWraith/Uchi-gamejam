using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed;
    private float undeathPeriodSeconds;
    private float undeathPeriod;
    private int goalPosition;
    private bool isRun = false;
    private float distance;
    private Animator animator;
    private List<float> positions;
    private BoxCollider2D collider2d;
    private string normalBool = "normal";
    private string upBool = "up";
    private string downBool = "down";
    private string damageBool = "damage";
    private string runSpeedName = "MovingSpeed";
    private Action onHit;

    void Start()
    {
        animator = GetComponent<Animator>();
        collider2d = GetComponent<BoxCollider2D>();
        SetMoving(0);
    }

    public void LoadPlayer(List<float> positions, RunGameSettings gameSettings)
    {
        this.positions = positions;
        this.speed = gameSettings.playerSpeed;
        this.undeathPeriodSeconds = gameSettings.undeathPeriodSeconds;
        
        goalPosition = positions.Count / 2;
        Vector3 v = transform.localPosition;
        transform.localPosition = new Vector3(v.x, positions[goalPosition], v.z);
    }

    public void SeteOnHit(Action h)
    {
        onHit = h;
    }

    public void StartPlayer()
    {
        isRun = true;
        distance = 0;
        undeathPeriod = -1;
        SetMoving(1);
    }

    public void StopPlayer()
    {
        isRun = false;
        UndeathOff();
        SetMoving(0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        HitPlayer();
    }

    private void HitPlayer() {
        onHit?.Invoke();
        undeathPeriod = undeathPeriodSeconds;
    }

    void Update()
    {
        if (!isRun)
        {
            return;
        }

        undeathPeriod -= Time.deltaTime;

        if (undeathPeriod > 0)
        {
            UndeathOn();
        } else
        {
            UndeathOff();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            goalPosition = Mathf.Clamp(goalPosition - 1, 0, positions.Count - 1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            goalPosition = Mathf.Clamp(goalPosition + 1, 0, positions.Count - 1);
        }

        Vector3 p = transform.localPosition;
        float currentY = p.y;
        float goalY = positions[goalPosition];
        float deltaY = Time.deltaTime * speed;
        distance = currentY - goalY;

        if (distance == 0)
        {
            SetAnimationNoraml();
            return;
        }

        // вверх
        if (distance < 0)
        {
            SetAnimationUp();
            distance += deltaY;

            if (distance >= 0)
            {
                transform.localPosition = new Vector3(p.x, goalY, p.z);
            }
            else
            {
                transform.localPosition = new Vector3(p.x, currentY + deltaY, p.z);
            }

            return;
        }

        // вниз
        if (distance > 0)
        {
            SetAnimationDown();
            distance -= deltaY;

            if (distance <= 0)
            {
                transform.localPosition = new Vector3(p.x, goalY, p.z);
            }
            else
            {
                transform.localPosition = new Vector3(p.x, currentY - deltaY, p.z);
            }

            return;
        }
    }

    private void SetAnimationUp()
    {
        animator.SetBool(upBool, true);
        animator.SetBool(normalBool, false);
        animator.SetBool(downBool, false);
    }

     private void SetAnimationDown()
    {
        animator.SetBool(downBool, true);
        animator.SetBool(normalBool, false);
        animator.SetBool(upBool, false);
    }

     private void SetAnimationNoraml()
    {
        animator.SetBool(normalBool, true);
        animator.SetBool(upBool, false);
        animator.SetBool(downBool, false);
    }

    private void UndeathOn()
    {
        animator.SetBool(damageBool, true);
        collider2d.enabled = false;
    }

    private void UndeathOff()
    {
        animator.SetBool(damageBool, false);
        collider2d.enabled = true;
    }

    private void SetMoving(float scale)
    {
        animator.SetFloat(runSpeedName, scale);
    }
}
