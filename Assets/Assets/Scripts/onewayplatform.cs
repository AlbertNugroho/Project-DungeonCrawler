using System.Collections;
using UnityEngine;

public class onewayplatform : MonoBehaviour
{
    private CompositeCollider2D platformCollider;
    [SerializeField] private CapsuleCollider2D playerCollider;
    [SerializeField] private float holdTimeToDrop = 0.3f; // Time needed to hold down to fall
    [SerializeField] private float cooldownTime = 1.0f; // Time before player can drop again

    private float counter;
    private bool canDrop = true; // Prevent immediate re-dropping

    private void Start()
    {
        platformCollider = GameObject.FindGameObjectWithTag("OneWayPlatform").GetComponent<CompositeCollider2D>();
    }

    private void Update()
    {
        if (!canDrop) return; // Stop checking if cooldown is active

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            counter += Time.deltaTime;
        }
        else
        {
            counter = 0;
        }

        if (counter > holdTimeToDrop)
        {
            StartCoroutine(DisableCollisionTemporarily());
        }
    }

    private IEnumerator DisableCollisionTemporarily()
    {
        canDrop = false; // Prevent immediate re-dropping
        Physics2D.IgnoreCollision(playerCollider, platformCollider, true);

        yield return new WaitForSeconds(0.5f); // Time the player falls through

        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);

        yield return new WaitForSeconds(cooldownTime); // Cooldown before dropping again
        canDrop = true; // Allow dropping again
    }
}
