using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class onewayplatform : MonoBehaviour
{
    private GameObject currentOneWayPlatform;
    private CompositeCollider2D platformCollider;
    [SerializeField] private CapsuleCollider2D playerCollider;
    private float counter;

    private void Start()
    {
        platformCollider = GameObject.FindGameObjectWithTag("OneWayPlatform").GetComponent<CompositeCollider2D>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            counter += Time.deltaTime;
        }
        else
        {
            counter = 0;
        }

        if (counter > 0.5)
        {
            Physics2D.IgnoreCollision(playerCollider, platformCollider);
        }
        else
        {
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        }
    }
}