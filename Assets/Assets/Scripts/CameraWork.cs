using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{

    public GameObject following;
    public float offsetcamera = -6.3f;
    private float smoothtime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 target = new Vector3(following.transform.position.x, following.transform.position.y + offsetcamera, -6);
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothtime);
    }
}
