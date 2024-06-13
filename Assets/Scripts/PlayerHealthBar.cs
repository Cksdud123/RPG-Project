using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider healthSlider;

    public float maxHealth = 100f;
    public float health;

    private float lerfSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthSlider.value != health) healthSlider.value = Mathf.Lerp(healthSlider.value, health, lerfSpeed);

        if (Input.GetKeyDown(KeyCode.X)) takeDamage(10);
    }

    void takeDamage(float damage)
    {
        health -= damage;
    }
}
