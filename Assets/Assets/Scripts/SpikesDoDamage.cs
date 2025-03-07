using System.Collections;
using UnityEngine;

public class SpikesDoDamage : MonoBehaviour
{
    public PlayerHealth playerhealth;
    public float damageCooldown = 1f; // Matches flashDuration in PlayerHealth

    private bool isPlayerInside = false;
    private bool isTakingDamage = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;

            // Start the damage coroutine only if not already running
            if (!isTakingDamage)
            {
                StartCoroutine(DealDamageOverTime());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    private IEnumerator DealDamageOverTime()
    {
        isTakingDamage = true;

        while (isPlayerInside)
        {
            if (playerhealth != null)
            {
                playerhealth.takedamage(10);
            }
            yield return new WaitForSeconds(damageCooldown);
        }

        isTakingDamage = false;
    }
}
