using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxhealth = 120;
    public Slider Healthbar;
    public Color flashColor = Color.white;
    public float flashDuration = 1f;
    public SpriteRenderer spriteRenderer;
    private float damageTimer = 0;

    void Start()
    {
        setMaxHealth(maxhealth);
        Healthbar.value = maxhealth;
    }

    void Update()
    {
        if (Healthbar.value <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
    }

    public void setMaxHealth(float maxhealth)
    {
        Healthbar.maxValue = maxhealth;
    }

    public void takedamage(int damage)
    {
        if (damageTimer <= 0)
        {
            Healthbar.value -= damage;
            damageTimer = flashDuration;
            Flash(); // Flash happens every time damage is taken
        }
    }

    public void Flash()
    {
        StartCoroutine(FlashEffect());
    }

    private IEnumerator FlashEffect()
    {
        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < 1; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration / 2);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration / 2);
        }
        spriteRenderer.color = originalColor;
    }
}
