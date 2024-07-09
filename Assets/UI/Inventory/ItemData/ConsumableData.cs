using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "newItem/ConsumableItem")]
[System.Serializable]
public class ConsumableData : ItemData
{
    public float RecoveryAmountHP;
    public float RecoveryAmountMP;

    private float coolTime = 5f;
    private float coolTime_Max = 5f;
    private Image coolTimeImage;

    public void UseHP()
    {
        HealthBar.instance.HeallingHP(RecoveryAmountHP);
    }

    IEnumerator CoolTime()
    {
        // 쿨타임 오브젝트를 가져와서 활성화
        coolTimeImage.gameObject.SetActive(true);

        while (coolTime > 0.0f)
        {
            coolTime -= Time.deltaTime;

            coolTimeImage.fillAmount = coolTime / coolTime_Max;

            yield return new WaitForFixedUpdate();
            coolTimeImage.gameObject.SetActive(false);
        }
    }
}
