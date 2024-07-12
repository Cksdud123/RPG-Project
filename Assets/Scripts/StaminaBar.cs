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
    public bool isUsed;
    public float ResetTime = 5.0f;

    private float lerfSpeed = 0.05f;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        stamina = maxStamina;
        StaminaSlider.maxValue = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (StaminaSlider.value != stamina) StaminaSlider.value = Mathf.Lerp(StaminaSlider.value, stamina, lerfSpeed);

        if(StaminaSlider.value >= 0.0f && StaminaSlider.value <= maxStamina)
        {
            HeallingStamina(0.5f * Time.deltaTime);
        }
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
