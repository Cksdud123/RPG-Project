using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    public ParticleSystem HeallingEffect;
    public GameObject SwirlEffect;
    public GameObject AoEEffect;

    private void Awake()
    {
        instance = this;
    }

    public void On_Tornado()
    {
        SwirlEffect.gameObject.SetActive(true);
    }
    public void Off_Tornado()
    {
        SwirlEffect.gameObject.SetActive(false);
    }

    public void On_AoE()
    {
        AoEEffect.gameObject.SetActive(true);
    }
    public void Off_AoE()
    {
        AoEEffect.gameObject.SetActive(false);
    }
}
