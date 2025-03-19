using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particledash : MonoBehaviour
{
    void Awake()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var main = ps.main;

        // Convert mouse position to world space
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(
            new Vector3(mousePos.x, mousePos.y, transform.position.z - Camera.main.transform.position.z)
        );

        // Compute direction and angle
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x);

        // Directly apply angle with correction for particle system default orientation
        main.startRotation = angle + Mathf.PI / 2f;
    }

}
