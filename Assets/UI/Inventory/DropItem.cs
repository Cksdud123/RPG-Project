using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField] public Slot DropSlot;
    [SerializeField] TextMeshProUGUI txt_ButtonText;
    [SerializeField] TextMeshProUGUI txt_DropItemName;
    [SerializeField] public Slider DropCountSlider;

    [HideInInspector] public int dropAmount = 0;
    [HideInInspector] public PlayerStatus playerStatus;
    private void Awake()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
    }
    private void OnEnable()
    {
        if (DropSlot != null && DropSlot.ItemInSlot != null)
        {
            txt_DropItemName.text = DropSlot.ItemInSlot.Name;
            DropCountSlider.maxValue = DropSlot.AmountInSlot;  // maxValue�� ���⼭ ������Ʈ
            DropCountSlider.value = 0;  // �����̴� ���� �ʱ�ȭ
            SetButtonText();  // ��ư �ؽ�Ʈ ������Ʈ
        }
    }
    private void Update()
    {
        SetButtonText();
    }
    public void SetButtonText()
    {
        // ����� �������� ������ �����̴��� ������ ����
        // �Ҽ����� ���� ��Ʈ�� �����ϱ� ���� RoundToInt�� ���
        dropAmount = Mathf.RoundToInt(DropCountSlider.value);

        // ��ư �ؽ�Ʈ ������Ʈ
        txt_ButtonText.text = $"{dropAmount}";
    }
    public void minusButton()
    {
        DropCountSlider.value -= 1;
        // ��ư �ؽ�Ʈ ������Ʈ
        SetButtonText();
    }
    public void plusButton()
    {
        DropCountSlider.value += 1;
        // ��ư �ؽ�Ʈ ������Ʈ
        SetButtonText();
    }
    public void DropButon()
    {
        DropSlot.DropItems(DropSlot);
    }
    public void CancelButton()
    {
        DropCountSlider.value = 0;
        txt_ButtonText.text = "";
        DropSlot = null;
        gameObject.SetActive(false);
    }

    public void SellItemButton()
    {
        // ���� ���� �ѷ��� ����
        int MoneyAmount = dropAmount * DropSlot.ItemInSlot.Price;

        // PlayerStatus�� ����
        playerStatus.PlayerMoney += MoneyAmount;
        playerStatus.UpdateMonney();

        if (DropSlot.AmountInSlot == 0) return;

        // ������ ���� ���� �߰�
        if (dropAmount > 0)
        {
            if (dropAmount >= DropSlot.AmountInSlot)
            {
                DropSlot.ItemInSlot = null;
                DropSlot.AmountInSlot = 0;

                DropSlot.SellAmount.text = DropSlot.AmountInSlot.ToString();
                DropSlot.SellAmount.gameObject.SetActive(false);
                DropSlot.SellIcon.texture = null;
                DropSlot.SellIcon.gameObject.SetActive(false);
            }
            else
            {
                DropSlot.AmountInSlot -= dropAmount;
                DropSlot.SellAmount.text = DropSlot.AmountInSlot.ToString();
            }

            DropSlot.UpdateSlot();
        }

        // DropItem�� Slider �ִ� ������ ����
        DropCountSlider.maxValue = DropSlot.AmountInSlot;
    }

}