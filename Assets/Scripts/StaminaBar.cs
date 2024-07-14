using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public static StaminaBar instance;

    public Slider StaminaSlider;

    public float maxStamina;
    public float stamina;

    private float lerfSpeed = 0.05f;

    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (StaminaSlider.value != stamina)
            StaminaSlider.value = Mathf.Lerp(StaminaSlider.value, stamina, lerfSpeed);
        
        HeallingStamina(0.5f * Time.deltaTime);
    }

    public void UseStamina(float damage)
    {
        stamina -= damage;
    }

    public void HeallingStamina(float healling)
    {
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
        else if (stamina < 0.0f)
        {
            stamina = 0.0f;
        }

        stamina += healling;
    }
}
