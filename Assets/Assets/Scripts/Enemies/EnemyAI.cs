using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 5f;
    public float patrolSpeed = 3f;
    public float jumpForce = 12f;
    public int health = 130;
    public int damage = 20;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    private CapsuleCollider2D cc;
    public Animator anim;
    public SpriteRenderer sr;
    public BoxCollider2D damageCollider;
    public AudioManager am;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;
    private bool isChasing = false;
    private bool isAttacking = false;
    private float patrolDir = 1;
    private float chaseTimer = 0f;
    private const float chaseDuration = 5f;
    public PlayerHealth playerHealth;
    public TextMeshProUGUI txt;
    public TextMeshProUGUI score;
    public GameObject key;
    public GameObject healthup;
    public int enemycount;
    private void Awake()
    {
        score = GameObject.FindGameObjectWithTag("scoretxt").GetComponent<TextMeshProUGUI>();
        txt = GameObject.FindGameObjectWithTag("keytxt").GetComponent<TextMeshProUGUI>();
        am = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemies");
        enemycount = enemies.Length;
        Debug.Log(enemycount);
        cc = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerHealth = player?.GetComponent<PlayerHealth>();
        IgnoreEnemyCollisions();
    }

    void Update()
    {
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        if (isAttacking) return;

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayer);
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        bool isPlayerNear = Physics2D.OverlapCircle(transform.position + Vector3.up * 2, 10f, playerLayer);
        bool isPlayerAbove = player.position.y > transform.position.y + 2f;
        bool isPlayerBelow = player.position.y < transform.position.y - 2f;
        float flipDirection = transform.localScale.x;

        // Adjusted collider position to flip correctly
        Vector3 flippedOffset = new Vector3(damageCollider.offset.x * flipDirection, damageCollider.offset.y, 0);

        bool isPlayerInRange = Physics2D.OverlapBox(damageCollider.transform.position + flippedOffset,damageCollider.size,0,playerLayer);
        RaycastHit2D wallBetween = Physics2D.BoxCast(transform.position, new Vector2(1f, 3f), 0f, new Vector2(direction, 0), 2f, groundLayer);

        RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 15f, groundLayer);

        if (isPlayerInRange && isGrounded)
        {
            Attack();
            return;
        }
        if (isGrounded)
        {
            if (isPlayerNear)
            {
                isChasing = true;
                chaseTimer = chaseDuration;
            }
            else if (chaseTimer > 0)
            {
                chaseTimer -= Time.deltaTime;
            }
            else
            {
                isChasing = false;
            }

            MoveEnemy(direction, isPlayerAbove, isPlayerBelow, wallBetween, platformAbove);
        }

        FlipSprite(rb.velocity.x);
    }

    private void FixedUpdate()
    {
        if (isAttacking) return;

        if (isGrounded && shouldJump)
        {
            shouldJump = false;
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            rb.velocity = new Vector2(5f * direction, jumpForce);
            am.playclip(am.jumpfx);
        }



        Patrol();
    }

    private void MoveEnemy(float direction, bool isPlayerAbove, bool isPlayerBelow, RaycastHit2D wallBetween, RaycastHit2D platformAbove)
    {
        CompositeCollider2D platformCollider = GameObject.FindGameObjectWithTag("OneWayPlatform").GetComponent<CompositeCollider2D>();
        if (isPlayerBelow && isChasing)
        {
            Physics2D.IgnoreCollision(cc, platformCollider, true);
        }
        else
        {
            Physics2D.IgnoreCollision(cc, platformCollider, false);
        }
        if (isChasing)
        {
            rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);
            if (isPlayerAbove && (wallBetween.collider || platformAbove.collider))
            {
                shouldJump = true;
            }
        }
        else
        {
            rb.velocity = new Vector2(patrolSpeed * patrolDir, rb.velocity.y);
        }
    }

    private void Patrol()
    {
        if (isChasing) return;
        RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, new Vector2(patrolDir, 0), 2f, groundLayer);
        RaycastHit2D patrolGap = Physics2D.Raycast(transform.position + new Vector3(patrolDir, 0, 0), Vector2.down, 0.5f, groundLayer);
        if (wallCheck.collider || !patrolGap.collider)
        {
            patrolDir *= -1;
        }
    }

    private void Attack()
    {
        
        isAttacking = true;
        anim.SetTrigger("Attack");
        Invoke(nameof(ResetAttack), 1f);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public void TakeDamage(int damageAmount)
    {
        if (health <= 0) return;
        am.playclip(am.EnemyTakeDamagefx);
        health -= damageAmount;
        PlayerHealth.ShakeCamera(5, 0.2f);
        if (health > 0)
        {
            anim.ResetTrigger("Attack");
            anim.SetTrigger("TakeDamage");
        }
        else
        {
            if (txt == null)
            {
                Debug.LogError("TextMeshProUGUI component is not found.");
                return;
            }
            if (key == null)
            {
                Debug.LogError("Item prefab is not assigned.");
                return;
            }

            anim.ResetTrigger("TakeDamage");
            anim.ResetTrigger("Attack");
            int before = lockmanager.count;
            lockmanager.Chancetodropkeys(txt, score, key, healthup, transform.position + new Vector3(3, 3, 0), transform.position + new Vector3(0, 1, 0), enemycount);
            am.playclip(am.deathfx);
            if(lockmanager.count > before)
            {
                am.playclip(am.keyfx);
            }
            anim.Play("Die", 0, 0f);
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(WaitForDeathAnimation());

        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }



    private void IgnoreEnemyCollisions()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemies");
        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            Collider2D myCollider = GetComponent<Collider2D>();
            if (enemyCollider != null && myCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, enemyCollider, true);
            }
        }
    }

    private void FlipSprite(float velocityX)
    {
        if (isChasing)
        {
            // Always look at the player while chasing
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            transform.localScale = new Vector3(-direction, 1, 1);
        }
        else
        {
            // Flip normally when patrolling
            if (Mathf.Abs(velocityX) > 0.1f)
            {
                transform.localScale = new Vector3(velocityX < 0 ? 1 : -1, 1, 1);
            }
        }
    }

}