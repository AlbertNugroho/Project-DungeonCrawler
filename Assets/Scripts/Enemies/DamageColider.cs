using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColider : MonoBehaviour
{
    public EnemyAI parent;
    public PlayerMovement player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && player.isBusy == false)
        {
            parent.playerHealth.TakeDamage(parent.damage);
        }
        
    }
}
