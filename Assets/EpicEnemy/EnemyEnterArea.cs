using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnterArea : MonoBehaviour
{
    public EpicBoss epicBoss;

    [HideInInspector] public bool isEnterArea;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnterArea = true;
            epicBoss.EquitAxe();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isEnterArea = false;
    }
}
