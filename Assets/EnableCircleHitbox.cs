using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCircleHitbox : MonoBehaviour
{
    public CircleCollider2D DamageColider;
    public static BoxCollider2D DashDamageColider;
    public void Start()
    {
        DashDamageColider = GameObject.FindGameObjectWithTag("DashDamage").GetComponent<BoxCollider2D>();
        DamageColider.enabled = false;
        DashDamageColider.enabled = false;
    }
    public void startAttack()
    {
        DamageColider.enabled = true;
    }

    public void endAttack()
    {
        DamageColider.enabled = false;
    }

    public static void DisableDashDamageColider()
    {
        DashDamageColider.enabled = false;
    }

    public static void EnableDashDamageColider()
    {
        DashDamageColider.enabled = true;
    }
}
