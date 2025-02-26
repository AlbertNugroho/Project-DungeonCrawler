using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxhealth = 120;
    public Slider Healthbar;

    void Start()
    {
        setMaxHealth(maxhealth);
        Healthbar.value = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setMaxHealth(float maxhealth)
    {
        Healthbar.maxValue = maxhealth;
    }
    public void takedamage(int damage)
    {
        Healthbar.value -= damage;
    }
}
