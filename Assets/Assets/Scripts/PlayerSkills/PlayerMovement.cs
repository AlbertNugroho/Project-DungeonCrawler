using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 8f;              
    public float jumpForce = 12f;         
    public float dashSpeed = 25f;        
    public float dashTime = 0.15f;       
    private float dashTimer;
    public float dashCooldown = 1.5f;      
    public float dashCooldownTimer = 1f;
    public float coyoteTime = 0.2f;       
    private float coyoteTimeCounter;
    public float jumpBufferTime = 0.15f;   
    public float walljumpingtime = 0.15f; 
    public float walljumpingduration = 0.3f;
    private Vector2 walljumpingpower = new Vector2(9, 12);
    private float jumpBufferCounter;

    [Header("Wall Mechanics")]
    public float wallSlideSpeed = 2f;
    public float wallJumpForce = 12f;
    public bool isWallSliding;
    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;

    [Header("Ground & Wall Detection")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public bool isGrounded = true;
    public bool wasGrounded = true;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator a;
    public Animator Scythe;
    public Transform ScytheTransform;
    public GameObject ps;
        
    public AudioManager am;
    public CapsuleCollider2D cc;

    [Header("Combat")]
    private IAttackSkill basicAttack;
    private IChargeAttack holdAttack;
    private IAttackSkill rightAttack;
    private IAttackSkill Omnidirectionalslash;
    public Staminawork StaminaBar;
    public PlayerHealth HealthBar;

    public float leftAttackCooldownTimer = 0f;
    public float rightAttackCooldownTimer = 0f;
    public float leftAttackCooldown = 0f;
    public float rightAttackCooldown = 5f;

    private bool isLeftAttackHolding = false;
    public float leftAttackHoldTimer = 0f;
    public bool leftAttackTransitionPlayed = false;
    public bool scythemaxsize = false;

    public float leftAttackTransitionThreshold = 0.1f;
    public float leftAttackRequiredHoldTime = 1f;
    public float scytheTargetScaleMultiplier = 1.5f;
    public Vector3 BaseScytheScale;
    

    public bool isBusy;
    public bool isDashing;
    public float moveInput;
    public bool isFacingRight = true;

    public bool iswalljumping;
    public float walljumpingdirection;
    
    public float walljumpingcounter;
    public Vector2 attackDirection;

    private void Awake()
    {
        am = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        a = GetComponent<Animator>();
        cc = GetComponent<CapsuleCollider2D>();
        BaseScytheScale = ScytheTransform.localScale;

        AttackFactory attackFactory = new AttackFactory();
        basicAttack = attackFactory.GetBasicAttack();
        holdAttack = attackFactory.GetHoldAttack();
        rightAttack = attackFactory.GetRightAttack();
        Omnidirectionalslash = attackFactory.GetOmniDirectionalSlash();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        a.SetFloat("speed", Mathf.Abs(moveInput));
        HandleCooldowns();
        attackDirection = HandleDirection();
        HandleDashing(attackDirection);
        HandleCombat(attackDirection);
        HandleJump();
        HandleWallSlide();
        walljump();
        fallingdetection();
        if(!iswalljumping)
        {
            Flip();
        }
        

    }
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (!isBusy && !iswalljumping)
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        
    }

    private void walljump()
    {
        if(isWallSliding)
        {
            iswalljumping = false;
            walljumpingdirection = -transform.localScale.x;
            walljumpingcounter = walljumpingtime;

            CancelInvoke(nameof(stopwalljumping));
        }
        else
        {
            walljumpingcounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && walljumpingcounter > 0f)
        {
            Debug.Log("I am pressed");
            iswalljumping = true;
            rb.velocity = new Vector2(walljumpingdirection * walljumpingpower.x, walljumpingpower.y);
            walljumpingcounter = 0;
            if (transform.localScale.x != walljumpingdirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localscale = transform.localScale;
                localscale.x *= -1;
                transform.localScale = localscale;
            }

            Invoke(nameof(stopwalljumping), walljumpingduration);
        }
    }

    private void stopwalljumping()
    {
        iswalljumping = false;
    }

    private bool iswalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckDistance, wallLayer);
    }
    private void HandleWallSlide()
    {
        if (iswalled() && !isGrounded && moveInput != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else isWallSliding = false;
    }

    private void HandleJump()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            a.Play("Base Layer.Jump");
            
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;
        }
    }

    private void Flip()
    {
        
        if (isFacingRight && moveInput < 0 || !isFacingRight && moveInput > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1;
            transform.localScale = localscale;
        }
    }
    private void HandleDashing(Vector2 attackDirection)
    {
        if (Input.GetMouseButtonDown(1) && !isBusy && dashCooldownTimer <= 0 && StaminaBar.staminaBar.value >= 10)
        {
            StaminaBar.useStamina(10);
            StartDash(attackDirection, dashSpeed);
            dashCooldownTimer = dashCooldown;
        }
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0) StopDash();
        }
    }

    private void HandleCombat(Vector2 attackDirection)
    {
        if (Input.GetMouseButtonDown(0) && !isBusy && !isDashing && leftAttackCooldownTimer <= 0)
        {
            ResetScytheScale();
            isLeftAttackHolding = true;
            leftAttackHoldTimer = 0f;
            leftAttackTransitionPlayed = false;
            scythemaxsize = false;
        }

        if (Input.GetMouseButton(0) && isLeftAttackHolding && !isDashing && !isBusy)
        {
            leftAttackHoldTimer += Time.deltaTime;
            holdAttack.ChargeAttack(this);
        }

        if (Input.GetMouseButtonUp(0) && isLeftAttackHolding && !isDashing && !isBusy)
        {
            if (leftAttackHoldTimer >= leftAttackTransitionThreshold)
            {
                holdAttack.ExecuteAttack(this, attackDirection);
            }
            else
            {
                basicAttack.ExecuteAttack(this, attackDirection);
            }
            isLeftAttackHolding = false;
            CombatManager.instance.InputManager();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && !isBusy && !isDashing && rightAttackCooldownTimer <= 0)
        {
            Omnidirectionalslash.ExecuteAttack(this, attackDirection);
            CombatManager.instance.InputManager();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !isBusy && !isDashing && rightAttackCooldownTimer <= 0)
        {
            rightAttack.ExecuteAttack(this, attackDirection);
            CombatManager.instance.InputManager();
        }
    }

    private Vector2 HandleDirection()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePosition - (Vector2)transform.position).normalized;
        return attackDirection;
    }

    private void HandleCooldowns()
    {
        if (leftAttackCooldownTimer > 0)
            leftAttackCooldownTimer -= Time.deltaTime;

        if (rightAttackCooldownTimer > 0)
            rightAttackCooldownTimer -= Time.deltaTime;

        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
    }

    private void fallingdetection()
    {
        if (!isGrounded && rb.velocity.y < 0)
            a.SetTrigger("falling");
        else if (isGrounded && a.GetCurrentAnimatorStateInfo(0).IsName("falling"))
            a.Play("Base Layer.idle");
    }

    public void PlayFootstep()
    {
        am.playclip(am.walkingfx);
    }
    public void PlayJump()
    {
        am.playclip(am.jumpfx);
    }
    public void StartDash(Vector2 direction, float dashSpeed)
    {
        isDashing = true;
        isBusy = true;
        a.SetTrigger("Dashing");

        dashTimer = dashTime;
        rb.velocity = direction * dashSpeed;

        // Flip player sprite if moving left
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isFacingRight = false;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            isFacingRight = true;
        }

        // Start the particle effect coroutine
        StartCoroutine(DashEffectLoop(direction));
    }

    private IEnumerator DashEffectLoop(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        while (isDashing) // Keep spawning effects while dashing
        {
            GameObject effect = Instantiate(ps, transform.position + new Vector3(0, -0.7f, 0), Quaternion.Euler(angle, 90, 0));
            Destroy(effect, 0.3f); // Destroy after 0.5s

            yield return new WaitForSeconds(0.03f); // Spawn every 0.1s
        }
    }

    void StopDash()
    {
        isDashing = false;
        isBusy = false;
        cc.isTrigger = false;
        rb.velocity = Vector2.zero;
        ResetScytheScale();
    }

    public void ResetScytheScale()
    {
        ScytheTransform.localScale = BaseScytheScale;
    }

    public void SetScytheScale(Vector3 newScale)
    {
        ScytheTransform.localScale = newScale;
    }
}