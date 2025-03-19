//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Ladderwork : MonoBehaviour
//{
//    [Header("Ladder Mechanics")]
//    public float climbSpeed = 4f;

//    private Rigidbody2D rb;
//    private bool isClimbing = false;
//    private bool canClimb = true; // Prevent instant re-climbing

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//    }

//    void Update()
//    {
//        if (isClimbing)
//        {
//            float verticalInput = Input.GetAxisRaw("Vertical");
//            rb.velocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed);
//        }

//        if (isClimbing && Input.GetKeyDown(KeyCode.Space)) // Allow Jumping Off
//        {
//            ExitLadder();
//            rb.velocity = new Vector2(rb.velocity.x, 12f); // Jump off ladder
//            StartCoroutine(ReEnableClimb()); // Prevent re-entering the ladder immediately
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Ladders") && canClimb)
//        {
//            isClimbing = true;
//        }
//    }

//    private void OnTriggerStay2D(Collider2D other)
//    {
//        if (other.CompareTag("Ladders") && canClimb)
//        {
//            isClimbing = true;
//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("Ladders"))
//        {
//            ExitLadder();
//        }
//    }

//    private void ExitLadder()
//    {
//        isClimbing = false;
//    }

//    private IEnumerator ReEnableClimb()
//    {
//        canClimb = false;
//        yield return new WaitForSeconds(0.3f); // Wait before re-enabling climbing
//        canClimb = true;
//    }
//}
