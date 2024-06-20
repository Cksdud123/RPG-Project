using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnterArea : MonoBehaviour
{
    public EpicBoss epicBoss;
    public GameObject EpicBossHealthBar;
    public GameObject MiniMap;

    [HideInInspector] public bool isEnterArea;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnterArea = true;
            epicBoss.EquitAxe();
            EpicBossHealthBar.gameObject.SetActive(true);
            MiniMap.gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        EpicBossHealthBar.gameObject.SetActive(false);
        MiniMap.gameObject.SetActive(true);

        if (other.CompareTag("Player")) isEnterArea = false;
    }
}
