using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpicHealthBar : MonoBehaviour
{
    public Slider healthSlider;

    public float maxHealth;
    public float health;

    private float lerfSpeed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != health) healthSlider.value = Mathf.Lerp(healthSlider.value, health, lerfSpeed);
    }

    public void takeDamage(float damage)
    {
        health -= damage;
    }
    public void HeallingHP(float healling)
    {
        health += healling;
    }
}
