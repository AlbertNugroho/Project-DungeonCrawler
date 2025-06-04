using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDeath : MonoBehaviour
{
    private void Awake()
    {
        Invoke(nameof(DestroySelf), 1);
    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
