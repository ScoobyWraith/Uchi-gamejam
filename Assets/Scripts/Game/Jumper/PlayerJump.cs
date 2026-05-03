using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask groundLayer;
    
    private float undeathPeriod;
    private JumpGameSettings gameSettings;
    private bool isRun = false;
    private Animator animator;
    private BoxCollider2D collider2d;
    private string normalBool = "normal";
    private string upBool = "up";
    private string downBool = "down";
    private string damageBool = "damage";
    private string runSpeedName = "RunSpeed";
    private Action onHit;
    private float playerWidth;
    private float groundCheckRadius = 0.2f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Camera mainCamera;
    private Vector3 initPosition;

    void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider2d = GetComponent<BoxCollider2D>();
        playerWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        initPosition = transform.localPosition;

        rb.bodyType = RigidbodyType2D.Static;
        SetRunning(0);
    }

    public void LoadPlayer(JumpGameSettings gameSettings)
    {
        this.gameSettings = gameSettings;
    }

    public void SeteOnHit(Action h)
    {
        onHit = h;
    }

    public void StartPlayer()
    {
        isRun = true;
        undeathPeriod = -1;
        rb.bodyType = RigidbodyType2D.Dynamic;
        SetRunning(1);
    }

    public void StopPlayer()
    {
        isRun = false;
        UndeathOff();
        rb.bodyType = RigidbodyType2D.Static;
        SetRunning(0);
    }

    public float GetWidth()
    {
        return playerWidth;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (undeathPeriod < 0)
        {
            HitPlayer();
        }
    }

    private void HitPlayer() {
        onHit?.Invoke();
        undeathPeriod = gameSettings.undeathPeriodSeconds;
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
        } 
        else
        {
            UndeathOff();
        }

        Vector2 screenSize = GetScreenSize();

        if (transform.position.y < -screenSize.y / 2)
        {
            HitPlayer();
            Unjump();
            transform.localPosition = initPosition;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            SetRunning(1);

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        } 
        else
        {
            SetRunning(0.4f);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * gameSettings.playerJump, ForceMode2D.Impulse);
    }

    private void Unjump()
    {
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(Vector2.zero, ForceMode2D.Impulse);
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

    private void SetRunning(float scale)
    {
        animator.SetFloat(runSpeedName, scale);
    }

    private void UndeathOn()
    {
        animator.SetBool(damageBool, true);
    }

    private void UndeathOff()
    {
        animator.SetBool(damageBool, false);
    }

    private Vector2 GetScreenSize()
    {
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWeight = screenHeight * aspectRatio;

        return new Vector2(screenWeight, screenHeight);
    }
}
