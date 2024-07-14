using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsePortionItem : MonoBehaviour
{

    private float cooltime = 4f;
    private float cooltime_max = 4f;

    public bool isDrinking { get; set; }
    public Image disable;
    public Animator playerAnim;

    [Header("Object")]
    public GameObject PortionInPlayer;

    Slot PortionSlot;
    // Update is called once per frame
    private void Awake()
    {
        PortionSlot = GetComponentInChildren<Slot>();
    }
    void Update()
    {
        // 포션 슬롯에 아이템이 있다면
        if (PortionSlot.ItemInSlot != null)
        {
            PortionInPlayer.SetActive(true);
        }
        else
        {
            PortionInPlayer.SetActive(false);
        }

        if (Input.GetKeyUp(KeyCode.Q) && PortionSlot.ItemInSlot != null && disable.fillAmount == 0)
        {
            playerAnim.SetTrigger("Drinking");
            UseItem();
        }
    }
    void UseItem()
    {
        StartCoroutine(CoolTimeFunc());

        ConsumableData consumableData = PortionSlot.ItemInSlot as ConsumableData;
        consumableData.UseHP();

        PortionSlot.AmountInSlot -= 1;
        PortionSlot.UpdateSlot();
    }

    IEnumerator CoolTimeFunc()
    {
        isDrinking = true;
        cooltime = cooltime_max;
        while (cooltime > 0.0f)
        {
            cooltime -= Time.deltaTime;

            disable.fillAmount = cooltime / cooltime_max;

            yield return null;
        }

        disable.fillAmount = 0.0f;
        isDrinking = false;
    }
}
