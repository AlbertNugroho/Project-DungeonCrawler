using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageEnemies : MonoBehaviour
{
    public PlayerMovement player;
    private HashSet<EnemyAI> enemiesInHitbox = new HashSet<EnemyAI>();
    private Coroutine damageCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemies"))
        {
            EnemyAI ea = collision.GetComponent<EnemyAI>();
            if (ea != null)
            {
                enemiesInHitbox.Add(ea);
                if (damageCoroutine == null)
                {
                    damageCoroutine = StartCoroutine(DamageOverTime());
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemies"))
        {
            EnemyAI ea = collision.GetComponent<EnemyAI>();
            if (ea != null)
            {
                enemiesInHitbox.Remove(ea);
                if (enemiesInHitbox.Count == 0 && damageCoroutine != null)
                {
                    StopCoroutine(damageCoroutine);
                    damageCoroutine = null;
                }
            }
        }
    }

    private IEnumerator DamageOverTime()
    {
        while (enemiesInHitbox.Count > 0)
        {
            for (int i = enemiesInHitbox.Count - 1; i >= 0; i--)
            {
                EnemyAI enemy = enemiesInHitbox.ElementAt(i);

                if (enemy != null)
                {
                    enemy.TakeDamage(player.Damage);
                    Debug.Log($"Damaged for {player.Damage} damage");
                }
            }

            yield return new WaitForSeconds(0.2f);
        }

        damageCoroutine = null;
    }
}
