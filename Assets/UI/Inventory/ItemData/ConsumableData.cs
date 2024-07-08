using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "newItem/ConsumableItem")]
[System.Serializable]
public class ConsumableData : ItemData
{
    public float RecoveryAmountHP;
    public float RecoveryAmountMP;
    public void UseHP()
    {
        HealthBar.instance.HeallingHP(RecoveryAmountHP);
    }
}
