using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCircleHitbox : MonoBehaviour
{
    public static CircleCollider2D DamageColider;
    public static BoxCollider2D DashDamageColider;
    public void Start()
    {
        DashDamageColider = GameObject.FindGameObjectWithTag("DashDamage").GetComponent<BoxCollider2D>();
        DamageColider = GameObject.FindGameObjectWithTag("OmniDamage").GetComponent<CircleCollider2D>();
        DamageColider.enabled = false;
        DashDamageColider.enabled = false;
    }
    public static void startAttack()
    {
        DamageColider.enabled = true;
    }

    public static void endAttack()
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
