using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxhealth = 120;
    public Slider Healthbar;
    public Color flashColor = Color.red;
    public float DamageCooldown = 0.3f;
    public SpriteRenderer spriteRenderer;
    private float damageTimer = 0;
    public GameObject deathScreen;
    public Animator anim;
    public AudioManager am;
    public static CinemachineVirtualCamera cvc;
    public static float shaketimer;
    public PlayerMovement player;

    private void Awake()
    {
        cvc = GameObject.FindGameObjectWithTag("Camera").GetComponent<CinemachineVirtualCamera>();
        am = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        Time.timeScale = 1;

        setMaxHealth(maxhealth);
        Healthbar.value = maxhealth;
    }

    void Update()
    {
        if (Healthbar.value <= 0)
        {
            am.playclip(am.deathfx);
            deathScreen.SetActive(true);
            Time.timeScale = 0;
        }
        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;

        if(shaketimer > 0)
        {
            shaketimer -= Time.deltaTime;   
        }
        if(shaketimer <=0)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        }
    }

    public void setMaxHealth(float maxhealth)
    {
        Healthbar.maxValue = maxhealth;
    }

    public void yes()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void No(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void TakeDamage(int damage)
    {
        if (damageTimer <= 0 && !player.isBusy)
        {
            Healthbar.value -= damage;
            damageTimer = DamageCooldown;
            ShakeCamera(5, 0.2f);
            am.playclip(am.Damagedfx);
            anim.SetTrigger("TakeDamage");
        }
    }

    public static void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shaketimer = time;
    }

}
