using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    [Header("Item Info")]
    public ItemData ItemInSlot;
    public int AmountInSlot;
    [SerializeField] private DropItem dropItems;

    [HideInInspector] public RawImage icon;
    [HideInInspector] public TextMeshProUGUI txt_amount;

    // ���� ��Ȱ��ȭ
    public void initSlot()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    // ���� ����
    public void SetSlot()
    {
        // Slot�� �ڽİ�ü�� Ȱ��ȭ �� ��
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        // �ڽ� ��ü�� ������Ʈ�� ����
        icon = GetComponentInChildren<RawImage>();
        txt_amount = GetComponentInChildren<TextMeshProUGUI>();

        UpdateSlot();
    }
    // ���� ������Ʈ
    public void UpdateSlot()
    {
        if (ItemInSlot != null)
        {
            icon.texture = ItemInSlot.ITEMICON;
            txt_amount.text = $"{AmountInSlot}";
            txt_amount.enabled = true;
        }
        else
        {
            icon.texture = null;
            txt_amount.text = "0";
            txt_amount.enabled = false;
        }
    }
    // ���� �ʱ�ȭ
    public void ResetSlot()
    {
        ItemInSlot = null;
        AmountInSlot = 0;

        // �����ܰ� �ؽ�Ʈ ��Ȱ��ȭ
        if (icon != null || txt_amount != null)
        {
            icon.gameObject.SetActive(false);
            icon.texture = null;

            txt_amount.gameObject.SetActive(false);
            txt_amount.text = AmountInSlot.ToString();
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragSlot draggableItem = dropped.GetComponent<DragSlot>();

        if (draggableItem == null || draggableItem.originParent == null) return;

        Slot slot = draggableItem.originParent.GetComponent<Slot>();

        // ���� ������ EquipmentPanel�� �ְ� �巡�׵� �������� Equipment Ÿ���� �ƴ� ��� ����
        if (transform.parent.name == "EquipmentPanel")
        {
            // ItemType�� Equipment�� �ƴ� ��� ����
            if (slot.ItemInSlot.ITEMTYPE != ItemType.Equipment) return;

            // ���� ����� Ÿ�԰� �±װ� ����ġ�ϸ� ����
            EquipmentData equipmentData = slot.ItemInSlot as EquipmentData;
            if (equipmentData.equipmentType.ToString() != transform.tag) return;
        }

        // ���� ������ ������ �������
        if (ItemInSlot == null)
        {
            // ����Ʈ ����� ��
            if (draggableItem.ShihtMode) HalfItemAmount(slot);
            // �Ϲ� ��� �϶�
            else ChangeEmptySlot(slot);
        }
        // ���� ������ ������ �ְ� ���̵� �ٸ���
        else if(ItemInSlot != null && ItemInSlot.ID != slot.ItemInSlot.ID) SwapItems(slot);
        // ���� ������ ������ �ְ� ���̵� ������
        else if(ItemInSlot != null && ItemInSlot.ID == slot.ItemInSlot.ID) AddItems(slot);
    }
    // �������� �� �������� ������ ����
    public void ChangeEmptySlot(Slot slot)
    {
        // ���� ������ ������ ������ ����
        ItemInSlot = slot.ItemInSlot;
        AmountInSlot = slot.AmountInSlot;

        SetSlot();
        slot.ResetSlot();
    }
    // �������� ������ �ݸ� �̵���Ŵ
    private void HalfItemAmount(Slot slot)
    {
        // ���� ������ 1�̸� ����
        if (slot.AmountInSlot == 1) return;

        // ���� ������ ������ ������ ����
        ItemInSlot = slot.ItemInSlot;
        AmountInSlot = Mathf.CeilToInt(slot.AmountInSlot / 2f);
        SetSlot();

        // ���� ������ ������ ������ ����
        slot.AmountInSlot -= AmountInSlot;
        slot.UpdateSlot();
    }
    // �� �������� ��ü
    public void SwapItems(Slot slot)
    {
        // ���� ������ ������ �����͸� �ӽ� ������ ����
        ItemData tempItem = ItemInSlot;
        int tempAmount = AmountInSlot;

        // ���� ���Կ� �巡���� ������ �����͸� �Ҵ�
        ItemInSlot = slot.ItemInSlot;
        AmountInSlot = slot.AmountInSlot;
        UpdateSlot();

        // �巡���� ���Կ� �ӽ� ����� ������ �����͸� �Ҵ�
        slot.ItemInSlot = tempItem;
        slot.AmountInSlot = tempAmount;
        slot.UpdateSlot();
    }
    // ���̵� ������ ����
    public void AddItems(Slot slot)
    {
        // �ΰ��� ���Կ� �ִ� �������� ���� �ִ� �����϶�
        if(AmountInSlot == ItemInSlot.MAXSTACK && slot.AmountInSlot == slot.ItemInSlot.MAXSTACK) return;
        // �ΰ��� ���Կ� �ִ� �������� �ϳ��� �ִ� �����϶�
        if(AmountInSlot == ItemInSlot.MAXSTACK || slot.AmountInSlot == slot.ItemInSlot.MAXSTACK) SwapItems(slot);
        // �ΰ��� ���Կ� �ִ� �������� �������� �ִ� ������ �ѱ��� ������
        if(AmountInSlot + slot.AmountInSlot <= ItemInSlot.MAXSTACK)
        {
            // ���� ���Կ� ������ ���� ��
            AmountInSlot += slot.AmountInSlot;
            UpdateSlot();

            slot.ResetSlot();
        }
        // �ΰ��� ���Կ� �ִ� �������� �������� �ִ� ������ �ѱ涧
        else if (AmountInSlot + slot.AmountInSlot >= ItemInSlot.MAXSTACK)
        {
            // �����ģ ������ ���� ��
            int overStack = AmountInSlot + slot.AmountInSlot;

            // ���� ������ ������ �ִ��
            AmountInSlot = ItemInSlot.MAXSTACK;
            UpdateSlot();
            // ���� ���Կ� ���� ������ ����
            slot.AmountInSlot = overStack - AmountInSlot;
            slot.UpdateSlot();
        }
    }
    // �������� ����
    public void DropItems(Slot slot)
    {
        // 0���� ����
        if (slot.AmountInSlot == 0) return;

        // ������ ���� ���� ���ð��� ���ٸ�
        if (dropItems.dropAmount == slot.AmountInSlot) slot.ResetSlot();

        // �ִ뽺���� �ƴ϶��
        else if (dropItems.dropAmount <= slot.ItemInSlot.MAXSTACK)
        {
            // ���� ������ ������ ������ �ʱ�ȭ
            slot.AmountInSlot -= dropItems.dropAmount;

            // DropItem�� Slider �ִ� ������ ����
            dropItems.DropCountSlider.maxValue = slot.AmountInSlot;

            // ���� ������ �ڽİ�ü�� �ִ� �ؽ�Ʈ ���� �ʱ�ȭ
            slot.txt_amount.text = slot.AmountInSlot.ToString();

            slot.UpdateSlot();
        }
    }

    public RawImage SellIcon => icon;
    public TextMeshProUGUI SellAmount => txt_amount;
}
