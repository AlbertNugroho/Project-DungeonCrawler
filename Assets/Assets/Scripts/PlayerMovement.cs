using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 7f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded = true;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    private float moveInput;
    private SpriteRenderer sr;
    private Animator a;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        a = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        a.SetFloat("speed", Mathf.Abs(moveInput));
        if (!isGrounded && rb.velocity.y < 0)
        {
            a.Play("Base Layer.falling"); // Start falling animation
        }
        else if(isGrounded && a.GetCurrentAnimatorStateInfo(0).IsName("falling"))
        {
            a.Play("Base Layer.idle");
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            a.Play("Base Layer.Jump");
        }

        if (moveInput < 0)
        {
            sr.flipX = true;
        }
        else if(moveInput > 0)
        {
            sr.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

}
