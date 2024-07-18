using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    public ParticleSystem HealEffect;
    public ParticleSystem SwirlEffect;

    private void Awake()
    {
        Instance = this;
    }

    void On_Swirl()
    {
        SwirlEffect.Play();
    }
    void Off_Swirl()
    {
        SwirlEffect.Stop();
    }
}
