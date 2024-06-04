using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField] public Slot DropSlot;
    [SerializeField] public Slider DropCountSlider;
    [SerializeField] TextMeshProUGUI txt_ButtonText;
    [SerializeField] TextMeshProUGUI txt_DropItemName;

    [HideInInspector] public int dropAmount = 0;
    private void OnEnable()
    {
        txt_DropItemName.text = DropSlot.ItemInSlot.Name.ToString();
        DropCountSlider.maxValue = DropSlot.AmountInSlot;
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
        gameObject.SetActive(false);
    }
}