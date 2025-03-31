using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableHitbox : MonoBehaviour
{
    public BoxCollider2D DamageColider;

    public void Start()
    {
        DamageColider.enabled = false;

    }
    public void Attack()
    {
        StartCoroutine(attack());
    }

    private IEnumerator attack()
    {
        DamageColider.enabled = true;
        yield return new WaitForSeconds(0.2f);
        DamageColider.enabled = false;
    }
}
