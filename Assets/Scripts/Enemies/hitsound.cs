using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitsound : MonoBehaviour
{
    public AudioManager am;
    private void Awake()
    {
        am = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void hit()
    {
        am.playclip(am.enemyatkfx);
    }
}
