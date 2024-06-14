using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnterArea : MonoBehaviour
{
    public EpicBoss epicBoss;
    public GameObject EpicBossHealthBar;

    [HideInInspector] public bool isEnterArea;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnterArea = true;
            epicBoss.EquitAxe();
            EpicBossHealthBar.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        EpicBossHealthBar.gameObject.SetActive(false);

        if (other.CompareTag("Player")) isEnterArea = false;
    }
}
