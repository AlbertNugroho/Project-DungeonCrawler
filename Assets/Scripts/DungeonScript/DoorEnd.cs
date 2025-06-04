using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEnd : MonoBehaviour
{
    private PlayerHealth player;
    public AudioManager am;
    public GameObject EndScene;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        am = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        EndScene = GameObject.FindGameObjectWithTag("End");
        EndScene.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E) && lockmanager.count >= 2)
            {
                lockmanager.count = 0;
                am.stopMusic();
                EndScene.SetActive(true);
                Time.timeScale = 0;
            }
        }

    }
}
