using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpicBossSountManager : MonoBehaviour
{
    public static EpicBossSountManager Instance;

    public AudioSource audioSource;
    public AudioClip ScreamClip;
    public AudioClip SwingSound;
    public AudioClip Swing360Sound;
    public AudioClip StampSound;
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
    public void Enemy_Swing360()
    {
        audioSource.clip = Swing360Sound;
        audioSource.Play();
    }
    public void Enemy_SwingSound()
    {
        audioSource.clip = SwingSound;
        audioSource.Play();
    }
    public void Enemy_StampSound()
    {
        audioSource.clip = StampSound;
        audioSource.Play();
    }
}
