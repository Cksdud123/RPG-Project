using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpicBossSountManager : MonoBehaviour
{
    public static EpicBossSountManager Instance;

    public AudioSource audioSource;
    public AudioClip ScreamClip;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
        }
        else 
            Destroy(this.gameObject);
    }

    public void Enemy_Scream()
    {
        audioSource.clip = ScreamClip;
        audioSource.Play();
    }
}
